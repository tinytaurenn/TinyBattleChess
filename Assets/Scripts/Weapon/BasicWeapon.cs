using Coherence.Toolkit;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static SO_Weapon;

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
    FWeaponParameters m_WeaponParameters = new FWeaponParameters(10, 1.5f, 10, EWeaponType.Sword, EWeaponSize.Right_Handed);

    AudioSource m_AudioSource; 
    [SerializeField] List<AudioResource> HitSounds;

    internal Transform m_HolderTransform = null; 

    [SerializeField] Collider m_DamageCollider;
    [SerializeField] public Transform m_HitPos; 
   
    List<Collider> HitList = new List<Collider>();

    internal EWeaponType WeaponType => m_WeaponParameters.WeaponType;
    internal EWeaponSize WeaponSize => m_WeaponParameters.WeaponSize;
    internal int WeaponDamage => m_WeaponParameters.Damage; 
    internal float WeaponSpeed => m_WeaponParameters.Speed;
    internal int WeaponCost => m_WeaponParameters.Cost;
    internal Vector3 WeaponPositionOffset => m_WeaponParameters.PositionOffset;
    internal Vector3 WeaponRotationOffset => m_WeaponParameters.RotationOffset;




    protected override void Awake()
    {
        base.Awake();
        m_AudioSource = GetComponent<AudioSource>();    
    }
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update(); 
    }

    public void SetupWeapon(Collider damageCollider, FWeaponParameters weaponParameters)
    {
        m_DamageCollider = damageCollider;
        m_WeaponParameters = weaponParameters;
    }


    private void OnDrawGizmos()
    {
        
    }



    public override void Release()
    {
        base.Release();
        m_HolderTransform = null;
        
    }

    public void ActivateDamage(bool activate)
    {
        Debug.Log("activating damage collider");
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
        if(m_Sync == null || !m_Sync.HasStateAuthority)
        {
            return; 
        }

        if (HitList.Contains(other)) return; 

        HitList.Add(other);
        
       
        if (m_DamageCollider.enabled == false || !m_DamageCollider.isTrigger || other.CompareTag("DamageCollider"))
        {

           
            return; 
        }

       

        if (other.TryGetComponent<CoherenceSync>(out CoherenceSync sync))
        {
            if (m_HolderTransform == other.transform)
            {
                Debug.Log("same holder");
                return;
            }
            Debug.Log("found coSync");

            if (other.TryGetComponent<EntityCommands>(out EntityCommands entCommands))
            {
                int weaponDir = 0; 
                if (m_HolderTransform.TryGetComponent<PlayerWeapons>(out PlayerWeapons weapons))
                {
                    weaponDir = (int)weapons.m_WeaponDirection; 
                }else if (m_HolderTransform.TryGetComponent<HumanoidNPC>(out HumanoidNPC npc))
                {
                    weaponDir = (int)npc.m_WeaponDirection; 
                }
                CoherenceSync holderSync = m_HolderTransform.GetComponent<CoherenceSync>();
              
                Debug.Log("sending commannd to target");
                sync.SendCommand<EntityCommands>(nameof(EntityCommands.TakeMeleeCommand), Coherence.MessageTarget.AuthorityOnly, weaponDir, holderSync, m_WeaponParameters.Damage, m_HolderTransform.transform.position);
            }
        }

       
        

        


    }


}