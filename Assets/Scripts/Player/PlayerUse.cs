using Coherence.Toolkit;
using System;
using UnityEngine;

namespace PlayerControls
{
    public class PlayerUse : MonoBehaviour
    {

        TinyPlayer m_TinyPlayer;

        [SerializeField] public Usable m_Usable;
        [SerializeField] public Grabbable m_LastGrabbable; 
        [SerializeField] public Seat m_LastSeat; 
        //[SerializeField] GameObject m_UsableObject; 
        [SerializeField] float m_UseDistance = 2f;
        [SerializeField] [Sync] public bool m_ItemInUse = false;
        //bool m_IsReplacing = false;

        float m_LastUseTime = 0;
        [SerializeField] float m_UseCooldown = 0.3f;


        private void Awake()
        {
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
         void FixedUpdate()
        {
            if(m_LastSeat!=null &&  Vector3.Distance(transform.position, m_LastSeat.transform.position) > m_UseDistance/2 )
            {
                m_LastSeat.ReleaseSeat(); 
                m_LastSeat = null; 
            }
        }


        internal void UsePerformed()
        {
            if(m_TinyPlayer.m_PlayerWeapons.Throwing) return;

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
                if (grabbable.m_IsHeld)
                {
                    Debug.Log("item is held");
                    return;
                }
                m_LastGrabbable = grabbable;

                m_LastGrabbable.OnUseValidate += OnGrabValidate;

                

            }
            if(m_Usable.TryGetComponent<Seat>(out Seat seat))
            {
                if(seat.IsOccupied)
                {
                    Debug.Log("Seat is occupied");
                    return;
                }

                if(m_LastSeat != null)m_LastSeat.ReleaseSeat();
                m_LastSeat = seat; 
                m_LastSeat.OnUseValidate += OnSeatValidate; 
            }

            m_Usable.TryUse();

            



        }

        

        internal void DropPerformed()
        {
            //if (!m_ItemInUse) return; 
            //m_ItemInUse = false;

            m_TinyPlayer.m_PlayerWeapons.Drop();
        }

        private void OnGrabValidate(bool validated)
        {
            Debug.Log("grabbable grab validate"); 
            m_LastGrabbable.OnUseValidate -= OnGrabValidate;

            if (validated)
            {

                m_TinyPlayer.m_PlayerLoadout.EquipGrabbableItem(m_LastGrabbable);
            } else
            {

            }

        }

        private void OnSeatValidate(bool validated)
        {
            m_LastSeat.OnUseValidate -= OnSeatValidate;
            if (validated)
            {
                Debug.Log("sitting on chair"); 
                m_TinyPlayer.m_PlayerMovement.SitOnTarget(m_LastSeat.m_SeatPosition);
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

        public void CleanUsableOutline()
        {
            if (m_Usable != null)
            {
                m_Usable.EnableOutline(false);
            }
        }
        void FindUsable()
        {
            if (UsableDetection(out Collider nearest))
            {


                if (nearest.gameObject.TryGetComponent<Usable>(out Usable usable))
                {
                    if (m_Usable != null)
                    {
                        m_Usable.EnableOutline(false);
                    }
                    m_Usable = usable;
                    usable.EnableOutline(true);

                    

                    if(usable.TryGetComponent<Grabbable>(out Grabbable grabbable))
                    {
                        LocalUI.Instance.ShowUsable("Grab (E) ");
                    }
                    else
                    {
                        LocalUI.Instance.ShowUsable("Use (E) ");
                    }

                    
                   
                    return; 
                }
                
            }
            LocalUI.Instance.HideUsable();
            if(m_Usable != null)
            {
                m_Usable.EnableOutline(false);
                m_Usable = null;
            }
            
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
