using Coherence.Toolkit;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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

    AudioSource m_AudioSource; 
    [SerializeField] List<AudioResource> HitSounds;

    internal PlayerWeapons m_HolderPlayerWeapons = null; 

    [SerializeField] Collider m_DamageCollider;
    [SerializeField] public Transform m_HitPos; 
   
    List<Collider> HitList = new List<Collider>(); 

    protected override void Awake()
    {
        base.Awake();
        m_AudioSource = GetComponent<AudioSource>();    
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
        if(activate)
        {
            HitList.Clear();
        }
        
    }

    public void PlayHitSound()
    {
        if (!m_Sync.HasStateAuthority) return; 
        if(HitSounds.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, HitSounds.Count);
            m_AudioSource.resource = HitSounds[randomIndex];
            m_AudioSource.Play(); 
            m_Sync.SendCommand<BasicWeapon>(nameof(SyncHitSound), Coherence.MessageTarget.Other, randomIndex);
        }

    }
    public void SyncHitSound(int index)
    {
        m_AudioSource.resource = HitSounds[index];
        m_AudioSource.Play();


    }

    


private void OnTriggerEnter(Collider other)
    {
        if (HitList.Contains(other)) return; 

        HitList.Add(other);
        
       
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

            if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                Debug.Log("Weapon hit " + other.name);

                //check defense 

                //sync.SendCommand<IDamageable>(nameof(IDamageable.TakeMeleeSync), Coherence.MessageTarget.AuthorityOnly,m_Sync,(int)m_HolderPlayerWeapons.m_WeaponDirection);
                if(other.TryGetComponent<Dummy>(out Dummy dummy))
                {
                    Debug.Log("sending commannd to dummy");
                    sync.SendCommand<Dummy>(nameof(Dummy.TakeMeleeSync), Coherence.MessageTarget.AuthorityOnly, (int)m_HolderPlayerWeapons.m_WeaponDirection, m_HolderPlayerWeapons.m_Sync, m_WeaponParameters.Damage, m_HolderPlayerWeapons.transform.position);
                }

                if(other.TryGetComponent<TinyPlayer>(out TinyPlayer tinyPlayer)) 
                {
                    Debug.Log("sending commannd to player");
                    sync.SendCommand<TinyPlayer>(nameof(TinyPlayer.TakeMeleeSync), Coherence.MessageTarget.AuthorityOnly, (int)m_HolderPlayerWeapons.m_WeaponDirection, m_HolderPlayerWeapons.m_Sync, m_WeaponParameters.Damage,m_HitPos.position);
                }


            }
        }

       
        

        


    }


}