using Coherence.Toolkit;
using System;
using System.Xml;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControls
{
    public class PlayerUse : MonoBehaviour
    {

        PlayerWeapons m_PlayerWeapons; 

        [SerializeField] public Grabbable m_Usable;
        [SerializeField] GameObject m_UsableObject; 
        [SerializeField] float m_UseDistance = 2f;
        [SerializeField] [Sync] public bool m_ItemInUse = false;

        float m_LastUseTime = 0;
        [SerializeField] float m_UseCooldown = 0.3f;

        private void Awake()
        {
            m_PlayerWeapons = GetComponent<PlayerWeapons>();
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

            if(m_Usable.m_IsHeld)
            {
                Debug.Log("item is held");
                return; 
            }
            if (m_ItemInUse)
            {
                Debug.Log("item in  use in player use");
                return; 
            }


            m_Usable.OnGrabValidate += OnGrabValidate;
            m_Usable.TryUse(); 

        }

        internal void DropPerformed()
        {
            if (!m_ItemInUse) return; 
            m_ItemInUse = false;

            m_PlayerWeapons.Drop();
        }

        private void OnGrabValidate(bool validated)
        {
            m_Usable.OnGrabValidate -= OnGrabValidate;

            if (validated)
            {
                if (m_Usable.TryGetComponent<IWeapon>(out IWeapon weapon))
                {
                    Debug.Log("validate Equipping weapon");
                    m_PlayerWeapons.EquipWeapon(m_Usable);
                    m_ItemInUse = true;
                }
            } else
            {

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


                if (nearest.gameObject.TryGetComponent<Grabbable>(out Grabbable usable))
                {
                    m_Usable = usable;
                    m_UsableObject = nearest.gameObject;
                    return; 
                }
                
            }

            m_Usable = null;
            m_UsableObject = null;
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
