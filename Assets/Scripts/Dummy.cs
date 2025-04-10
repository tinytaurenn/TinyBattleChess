using Coherence.Toolkit;
using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Dummy : Entity, IDamageable
{

    [SerializeField] bool m_InParry = false;
    [SerializeField] EWeaponDirection m_WeaponDirection = EWeaponDirection.Right;
    [SerializeField] float m_ParryAngle = 90f; 
    CoherenceSync m_Sync; 

    [SerializeField] [Sync] int Health = 100;

    Animator m_Animator;

    [SerializeField] float m_ChangeTime = 2f;
    float m_ChangeTimer = 0f; 

    

    private void Awake()
    {
        m_Sync = GetComponent<CoherenceSync>(); 
        m_Animator = GetComponent<Animator>();

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( m_ChangeTimer > m_ChangeTime)
        {
            ChangeParry(); 
            m_ChangeTimer = 0f;

        }
        m_ChangeTimer += Time.deltaTime;
    }

    void ChangeParry()
    {
        int randomInt = UnityEngine.Random.Range(0, 4);
        m_WeaponDirection = (EWeaponDirection)randomInt;
        m_Animator.SetInteger("DummyParry", randomInt);
        m_InParry = true;
    }

    

  
    public override void TakeMeleeSync(int DirectionNESO, CoherenceSync sync,int damage,EEffectType damageType, EWeaponType weaponType, Vector3 attackerPos)
    {

        EWeaponDirection direction = (EWeaponDirection)DirectionNESO; 
        Debug.Log(" take melee sync");
        Debug.Log(" strike " + direction.ToString() + " direction!");

        bool parry = false;

        switch (direction)
        {
            case EWeaponDirection.Right:
                parry = m_WeaponDirection == EWeaponDirection.Left;
                break;
            case EWeaponDirection.Left:
                parry = m_WeaponDirection == EWeaponDirection.Right;
                break;
            case EWeaponDirection.Up:
                parry = m_WeaponDirection == EWeaponDirection.Up;
                break;
            case EWeaponDirection.Down:
                parry = m_WeaponDirection == EWeaponDirection.Down;
                break;
        }

        if (m_InParry && parry && transform.IsInAngle(m_ParryAngle, attackerPos))
        {
            ParrySync(damage, sync);
            
        }
        else
        {
            TakeWeaponDamageSync(damage,damageType,weaponType,sync); 
            

        }

    }
    public override void StunEffect(float duration)
    {
        Debug.Log("cannot be stunned");
    }

    public override void EntityDeath()
    {
        Debug.Log("dummy died");    
    }

    public override void TakeWeaponDamageSync(int damage,EEffectType damageType, EWeaponType weaponType, CoherenceSync Damagersync)
    {
        Debug.Log("sync Dummy took " + damage + " damage!");

        Debug.Log("dummy sending synchit comand ");
        Damagersync.SendCommand<PlayerWeapons>(nameof(PlayerWeapons.SyncHit), Coherence.MessageTarget.AuthorityOnly);
    }
    public override void ParrySync(int damage,CoherenceSync DamagerSync)
    {
        Debug.Log("sync Dummy parried "); 
        //Debug.Log(DamagerSync.transform.name + " parried!"); // cant get a transform from serv side
        DamagerSync.SendCommand<PlayerWeapons>(nameof(PlayerWeapons.SyncBlocked), Coherence.MessageTarget.AuthorityOnly);
        int soundVariationIndex = UnityEngine.Random.Range(0, 3); 
        m_Sync.SendCommand<DummyFX>(nameof(DummyFX.PlayParryFX), Coherence.MessageTarget.All, soundVariationIndex); 
    }

    public override void TakeDamageSync(int damage,EEffectType damageType, CoherenceSync Damagersync)
    {
        throw new NotImplementedException();
    }

    public override void SyncBlocked()
    {
        throw new NotImplementedException();
    }

    public override void SyncHit()
    {
        throw new NotImplementedException();
    }

    public override bool GetAttackState(out EWeaponDirection attackDir)
    {
        attackDir = EWeaponDirection.Up;
        return true;
    }

    public override void OnReceiveAttackState(bool isAttacking, EWeaponDirection attackDir)
    {
        throw new System.NotImplementedException();
    }

    public override void PlayDamageSound(EWeaponType weaponType)
    {
        throw new NotImplementedException();
    }
}
