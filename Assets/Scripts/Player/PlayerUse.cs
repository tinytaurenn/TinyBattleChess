using Coherence.Toolkit;
using System;
using UnityEngine;

namespace PlayerControls
{
    public class PlayerUse : MonoBehaviour
    {

        PlayerWeapons m_PlayerWeapons; 
        TinyPlayer m_TinyPlayer;

        [SerializeField] public Usable m_Usable;
        [SerializeField] public Grabbable m_Grabbable;
        //[SerializeField] GameObject m_UsableObject; 
        [SerializeField] float m_UseDistance = 2f;
        [SerializeField] [Sync] public bool m_ItemInUse = false;
        //bool m_IsReplacing = false;

        float m_LastUseTime = 0;
        [SerializeField] float m_UseCooldown = 0.3f;

        private void Awake()
        {
            m_PlayerWeapons = GetComponent<PlayerWeapons>();
            m_TinyPlayer = GetComponent<TinyPlayer>();

        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            FindUsable(); 
            
        }


        internal void UsePerformed()
        {
            if(m_LastUseTime + m_UseCooldown > Time.time)
            {
                Debug.Log("use in cooldown  ");
                return;
            }
            m_LastUseTime = Time.time;
            Debug.Log("UsePerformed");

            if(m_Usable == null)
            {
                Debug.Log("usable is null");
                return; 
            }

            if(m_Usable.TryGetComponent<Grabbable>(out Grabbable grabbable))
            {
                m_Usable = grabbable;
                if (m_Grabbable.m_IsHeld)
                {
                    Debug.Log("item is held");
                    return;
                }
                
                m_Usable.OnUseValidate += OnGrabValidate;

                

            }
            if(m_Usable.TryGetComponent<Seat>(out Seat seat))
            {
                m_Usable = seat; 
                if(seat.IsOccupied)
                {
                    Debug.Log("Seat is occupied");
                    return;
                }
                m_Usable.OnUseValidate += OnSeatValidate; 
            }

            m_Usable.TryUse();

            



        }

        

        internal void DropPerformed()
        {
            //if (!m_ItemInUse) return; 
            //m_ItemInUse = false;

            m_PlayerWeapons.Drop();
        }

        private void OnGrabValidate(bool validated)
        {
            Debug.Log("grabbable grab validate"); 
            m_Grabbable.OnUseValidate -= OnGrabValidate;

            if (validated)
            {

                m_TinyPlayer.m_PlayerLoadout.EquipGrabbableItem(m_Grabbable);
            } else
            {

            }

        }

        private void OnSeatValidate(bool validated)
        {
            m_Usable.OnUseValidate -= OnSeatValidate;
            if (validated)
            {
                Debug.Log("sitting on chair"); 
            }
        }

        #region UseDetection


        Collider NearestCollider(Collider[] colliders)
        {
            //find nearest collider
            Collider nearest = null;
            float nearestDistance = float.MaxValue;
            foreach (var collider in colliders)
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < nearestDistance)
                {
                    nearest = collider;
                    nearestDistance = distance;
                }
            }

            return nearest;
         }

        void FindUsable()
        {
            if (UsableDetection(out Collider nearest))
            {


                if (nearest.gameObject.TryGetComponent<Usable>(out Usable usable))
                {
                    m_Usable = usable;

                    if(usable.TryGetComponent<Grabbable>(out Grabbable grabbable))
                    {
                        LocalUI.Instance.ShowUsable("Grab " + grabbable.SO_Item.ItemName);
                    }
                    else
                    {
                        LocalUI.Instance.ShowUsable("Use " + usable.name);
                    }

                    
                   
                    return; 
                }
                
            }
            LocalUI.Instance.HideUsable();
            m_Usable = null;
            //m_UsableObject = null;
        }

        bool UsableDetection(out Collider nearest, out Collider[] allColliders)
        {

            LayerMask usableMask = LayerMask.GetMask("Usable");
            allColliders = Physics.OverlapSphere(transform.position, m_UseDistance, usableMask);
            
            
            bool usableFound = allColliders.Length > 0;

            if (usableFound)
            {
                
                nearest = NearestCollider(allColliders);

            }
            else
            {
                nearest = null;
                allColliders = null;    
            }


            return usableFound; 
        }

        bool UsableDetection(out Collider nearest)
        {

            LayerMask usableMask = LayerMask.GetMask("Usable");
            Collider[] allColliders = Physics.OverlapSphere(transform.position, m_UseDistance, usableMask);


            bool usableFound = allColliders.Length > 0;

            if (usableFound)
            {

                nearest = NearestCollider(allColliders);

            }
            else
            {
                nearest = null;
            }


            return usableFound;
        }

        #endregion

        private void OnDrawGizmos()
        {
            

            if(UsableDetection(out Collider nearest, out Collider[] colliders))
            {
                Gizmos.color = Color.yellow;
                foreach (var collider in colliders)
                {
                    Gizmos.DrawWireCube(collider.transform.position, Vector3.one);
                }

                Gizmos.color = Color.green; 
                Gizmos.DrawWireCube(nearest.transform.position, Vector3.one);

            }
           



        }

    } 
}
