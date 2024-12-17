using Coherence.Toolkit;
using System;
using Unity.Multiplayer.Center.Common;
using UnityEngine;
using static SO_BasicWeapon;

public class BasicWeapon : Grabbable, IWeapon
{
    [Serializable]
    public struct FWeaponParameters
    {
        public int Damage;
        public float Speed;
        public int Cost;
        public EWeaponType WeaponType;
        public EWeaponSize WeaponSize;
        public Vector3 PositionOffset;
        public Vector3 RotationOffset;


     public FWeaponParameters(int damage, float speed, int cost, EWeaponType weaponType, EWeaponSize weaponSize, Vector3 positionOffset, Vector3 rotationOffset)
        {
            Damage = damage;
            Speed = speed;
            Cost = cost;
            WeaponType = weaponType;
            WeaponSize = weaponSize;
            PositionOffset = positionOffset;
            RotationOffset = rotationOffset;
        }
        public FWeaponParameters(int damage, float speed, int cost, EWeaponType weaponType, EWeaponSize weaponSize)
        {
            Damage = damage;
            Speed = speed;
            Cost = cost;
            WeaponType = weaponType;
            WeaponSize = weaponSize;
            PositionOffset = Vector3.zero;
            RotationOffset = Vector3.zero;


        }

    }

    

    [SerializeField]
    FWeaponParameters m_WeaponParameters = new FWeaponParameters(10, 1.5f, 10, EWeaponType.Sword, EWeaponSize.One_Handed);

    internal PlayerWeapons m_HolderPlayerWeapons = null; 

    [SerializeField] Collider m_DamageCollider; 

    protected override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnDrawGizmos()
    {
        
    }

    #region WeaponStats

    internal EWeaponType GetWeaponType() => m_WeaponParameters.WeaponType;
    internal EWeaponSize GetWeaponSize() => m_WeaponParameters.WeaponSize;
    internal int GetWeaponDamage() => m_WeaponParameters.Damage;
    internal float GetWeaponSpeed() => m_WeaponParameters.Speed;
    internal int GetWeaponCost() => m_WeaponParameters.Cost;
    internal Vector3 GetWeaponPositionOffset() => m_WeaponParameters.PositionOffset;
    internal Vector3 GetWeaponRotationOffset() => m_WeaponParameters.RotationOffset;

    
    #endregion


    public override void Release()
    {
        base.Release();
        m_HolderPlayerWeapons = null;
        
    }

    public void ActivateDamage(bool activate)
    {
        m_DamageCollider.enabled = activate;
        
    }

    


private void OnTriggerEnter(Collider other)
    {
        
       
        if (m_DamageCollider.enabled == false || !m_DamageCollider.isTrigger || other.CompareTag("DamageCollider"))
        {

           
            return; 
        }

       

        if (other.TryGetComponent<CoherenceSync>(out CoherenceSync sync))
        {
            if (sync.HasStateAuthority)
            {
                return;
            }
        }

        if(other.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            Debug.Log("Weapon hit " + other.name);

            //check defense 
            
            damageable.TakeMelee(m_HolderPlayerWeapons,m_WeaponParameters.Damage);
            damageable.OnParryEvent += m_HolderPlayerWeapons.OnParryEvent;
        }
        

        


    }


}