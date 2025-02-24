using Coherence;
using Coherence.Toolkit;
using System.Collections;
using UnityEngine;

public class HumanoidNPC : TinyNPC
{
    HumanoidFX m_HumanoidFX; 

    public enum EAttackState
    {
        None = 0,
        Attacking = 1,
        Release = 2,
        Parrying = 3,
    }
    [SerializeField] protected EAttackState m_AttackState = EAttackState.None; 


    [Header("Weapon Infos")]
    [SerializeField] protected BasicWeapon m_MainWeapon;
    [SerializeField] protected BasicWeapon m_SecondaryWeapon;
    public BasicWeapon GetMainWeapon() => m_MainWeapon;
    public BasicWeapon GetSecondaryWeapon() => m_SecondaryWeapon;
    [SerializeField] internal EWeaponDirection m_WeaponDirection = EWeaponDirection.Right;
    [SerializeField] protected EWeaponDirection m_AttackerDirection = EWeaponDirection.Left;

    [SerializeField] float m_BaseReleaseDelay = 0.15f;
    [SerializeField] float m_ParryAngle = 20f;
    [SerializeField] protected bool m_IsParryUnit = true; 
    [SerializeField] float m_HoldAttackDistance = 3f;



    [SerializeField] protected bool m_CanAttack = true;
    [SerializeField] float m_BlockedAttackCooldown = 0.5f;


    [SerializeField] bool m_InShieldParry = false;
    public bool InShieldParry
    {
        get { return m_InShieldParry && m_InParry; }
    }
    [SerializeField] protected bool m_InAttackRelease;

    [SerializeField] protected bool m_CanSwordAttack = true;
    [Sync]
    public bool CanAttack
    {
        get { return m_CanAttack; }
        set { m_CanAttack = value; }
    }
   
    [SerializeField] protected float m_AttackCooldown = 1f;
    [SerializeField] protected float m_AttackDelay = 0.4f;

    [SerializeField] protected float m_AttackCheckTimer = 0f;
    [SerializeField] protected float m_AttackCheckTime = 2f;
    [SerializeField] protected float m_InParryTimer = 0f;
    [SerializeField] protected float m_InParryTime = 3f;


    protected override void Awake()
    {
        base.Awake();
        m_HumanoidFX = GetComponent<HumanoidFX>();
    }
    protected override void OnAttack()
    {
        base.OnAttack();
        Debug.Log("HumanoidNPC attacking their target");
        
    }


    protected override void AttackUpdate()
    {
        base.AttackUpdate();

        if (m_FollowTarget == null)
        {
            SwitchAttackState(EAttackState.None);
            return; 
        }
        

        float distanceFromTarget = Vector3.Distance(transform.position, m_FollowTarget.position);


        switch (m_AttackState)
        {
            case EAttackState.None:
                
                if (distanceFromTarget <= m_HoldAttackDistance && m_CanAttack)
                {
                    SwitchAttackState(EAttackState.Attacking);
                }

                break;
            case EAttackState.Attacking:
                if (m_IsParryUnit) AttackCheckUpdate();

                if (distanceFromTarget <= m_StopDistance && m_CanSwordAttack)
                {
                    //Debug.Log("release attack");
                    SwitchAttackState(EAttackState.Release);
                }
                break;
            case EAttackState.Release:
                if(m_IsParryUnit) AttackCheckUpdate();

                if (distanceFromTarget > m_HoldAttackDistance)
                {
                    //Debug.Log("stop attacking");
                    SwitchAttackState(EAttackState.None);
                }
                if (distanceFromTarget <= m_HoldAttackDistance && m_CanAttack)
                {
                    SwitchAttackState(EAttackState.Attacking);
                }

                break;
            case EAttackState.Parrying:
                m_InParryTimer += Time.deltaTime;
                if (m_InParryTimer >= m_InParryTime)
                {
                    m_InParryTimer = 0f;

                    CheckTargetAttack();
                }
                break;
            default:
                break;
        }



    }

    protected override void OnExitState()
    {
        base.OnExitState();
        switch (m_MovementType)
        {
            case EMovementType.Idle:
                break;
            case EMovementType.Patrol:
                break;
            case EMovementType.Follow:
                break;
            case EMovementType.Flee:
                break;
            case EMovementType.Attack:
                m_AttackCheckTimer = 0f;
                break;
            case EMovementType.Dead:
                break;
            default:
                break;
        }
    }

    void SwitchAttackState(EAttackState attackState)
    {
        if (attackState == m_AttackState) return; 
        OnExitAttackState();
        m_AttackState = attackState;
        OnEnterAttackState();


    }
    void OnEnterAttackState()
    {
        switch (m_AttackState)
        {
            case EAttackState.None:
                InAttack = false;
                m_InParry = false;
                m_Animator.SetBool("Parry", false);
                m_CanAttack = true;
                m_Animator.SetBool("Attacking", false);
                break;
            case EAttackState.Attacking:
                if (m_CanAttack)
                {
                    m_CanAttack = false;
                    InAttack = true;
                    m_WeaponDirection = (EWeaponDirection)UnityEngine.Random.Range(0, 4); // temporary


                    m_Animator.SetInteger("WeaponDirectionNESO", (int)m_WeaponDirection);

                    m_Animator.SetBool("Attacking", InAttack);

                    
                }
                
                break;
            case EAttackState.Release:
                InAttack = false;
                StartCoroutine(ReleaseDelayRoutine(m_AttackDelay));
                break;
            case EAttackState.Parrying:
                m_InAttack = false;
                m_Animator.SetBool("Attacking", false);
                Debug.Log("starting in parry mode NPC ");
                SetParry(m_AttackerDirection);
                break;
            default:
                break;
        }





    }

    void OnExitAttackState()
    {
        switch (m_AttackState)
        {
            case EAttackState.None:
                break;
            case EAttackState.Attacking:
                break;
            case EAttackState.Release:
                break;
            case EAttackState.Parrying:
                break;
            default:
                break;
        }
    }

    public void LockAttackRelease(bool isLocked) => m_InAttackRelease = isLocked;

    public void LockAttack()
    {
        //Debug.Log("locking attack");
        m_InAttackRelease = true;
        if (m_MainWeapon!= null) m_MainWeapon.GetComponent<BasicWeapon>().m_HolderTransform = transform;
        if (m_SecondaryWeapon != null) m_SecondaryWeapon.GetComponent<BasicWeapon>().m_HolderTransform = transform;


        float normalizedTime = m_Animator.GetCurrentAnimatorStateInfo(1).normalizedTime;
        float animLenght = m_Animator.GetCurrentAnimatorStateInfo(1).length;

        // 0.3 normal => length = 2.4 ___________ time = 0.3 * 2.4 = 0.72 so 1.68 is the right time
        // 1 - normalizedTime * animLenght = time remaining 

        StartCoroutine(LockAttackRoutine((1 - normalizedTime) * animLenght));
    }

    IEnumerator LockAttackRoutine(float time)
    {
        float timeToWait = time + m_BaseReleaseDelay;
        yield return new WaitForSeconds(timeToWait);
        //Debug.Log("Unlocking attack");
        m_InAttackRelease = false;
        m_InAttack = false;
    }

    IEnumerator AttackDelayRoutine(float time)
    {

        
        yield return new WaitForSeconds(time);

        m_CanAttack = true;
        

    }

    IEnumerator ReleaseDelayRoutine(float time)
    {
        
        yield return new WaitForSeconds(time);
        m_Animator.SetBool("Attacking", false);
        StartCoroutine(AttackDelayRoutine(m_AttackCooldown));
    }

    public override void Stun()
    {
        base.Stun(); 
        if (m_MainWeapon != null) m_MainWeapon.ActivateDamage(false);
        if (m_SecondaryWeapon != null) m_SecondaryWeapon.ActivateDamage(false);

    }


    public override void EntityDeath()
    {
        base.EntityDeath();
        Debug.Log("NPC death");


    }

    IEnumerator AttackCoolDownRoutine(float time)
    {
        m_CanSwordAttack = false;
        yield return new WaitForSeconds(time);
        m_CanSwordAttack = true;
    }

    void SetParry(EWeaponDirection attackerDirection)
    {
        
        
        switch (attackerDirection)
        {
            case EWeaponDirection.Up:
                m_Animator.SetInteger("WeaponDirectionNESO", 0);
                m_WeaponDirection = EWeaponDirection.Up;
                break;
            case EWeaponDirection.Right:
                m_Animator.SetInteger("WeaponDirectionNESO", 3);
                m_WeaponDirection = EWeaponDirection.Left;
                break;
            case EWeaponDirection.Down:
                m_Animator.SetInteger("WeaponDirectionNESO", 2);
                m_WeaponDirection = EWeaponDirection.Down;
                break;
            case EWeaponDirection.Left:
                m_Animator.SetInteger("WeaponDirectionNESO", 1);
                m_WeaponDirection = EWeaponDirection.Right;
                break;
            default:
                break;
        }

        m_InParry = true;
       
        m_Animator.SetBool("Parry", true);
    }


    public override void SyncHit()
    {
        Debug.Log("i get sync Hit ");

        m_MainWeapon.PlayHitSound();
    }

    public override void SyncBlocked()
    {
        Debug.Log("i get sync blocked ");
        base.SyncBlocked();

        m_Animator.SetTrigger("Blocked");
        m_InAttack = false;
        m_InAttackRelease = false;
        InAttack = false;
        m_Sync.SendCommand<Animator>(nameof(Animator.SetTrigger), MessageTarget.Other, "Blocked");
        StartCoroutine(AttackCoolDownRoutine(m_BlockedAttackCooldown));
    }

    

    public override void TakeDamageSync(int damage, CoherenceSync Damagersync)
    {
        base.TakeDamageSync(damage, Damagersync);

    }

    public override void TakeMeleeSync(int DirectionNESO, CoherenceSync sync, int damage, Vector3 attackerPos)
    {
        //base.TakeMeleeSync(DirectionNESO, sync, damage, attackerPos);
        bool rightParry = false; 
        switch (DirectionNESO)
        {
            case 0:
                rightParry = m_WeaponDirection == EWeaponDirection.Up;
                break;
            case 1:
                rightParry = m_WeaponDirection == EWeaponDirection.Left;
                break;
            case 2:
                rightParry = m_WeaponDirection == EWeaponDirection.Down;
                break;
            case 3:
                rightParry = m_WeaponDirection == EWeaponDirection.Right;
                break;
            default:
                break;
        }

        Debug.Log("humanoid in parry : " + InParry);
        Debug.Log("humanoid right parry : " + rightParry + "attacker is " + (EWeaponDirection)DirectionNESO + " and my weapon direction is " + m_WeaponDirection);
        Debug.Log("humanoid angle parry : " + transform.IsInAngle(m_ParryAngle, attackerPos));

        rightParry = InParry && rightParry && transform.IsInAngle(m_ParryAngle, attackerPos);


        if (rightParry)
        {

            Debug.Log(" Humanoid NPC parry in angle");
            Debug.Log(" Humanoid NPC parry calculations");

            

            ParrySync(damage, sync);
        }
        else if (InShieldParry &&  transform.IsInAngle(m_ParryAngle, attackerPos) )
        {
            ParrySync(damage, sync);
        }
        else
        {
            TakeWeaponDamageSync(damage, sync);
        }
    }

    public override void ParrySync(int damage, CoherenceSync DamagerSync)
    {
        Debug.Log("sync humanoid NPC  parried ");
        Debug.Log(DamagerSync.transform.name + " parried!");
        DamagerSync.SendCommand<EntityCommands>(nameof(EntityCommands.SyncBlockedCommand), Coherence.MessageTarget.AuthorityOnly);

        int soundVariationIndex = UnityEngine.Random.Range(0, 3);
        m_HumanoidFX.PlayParryFX(soundVariationIndex);
        m_Sync.SendCommand<HumanoidFX>(nameof(HumanoidFX.PlayParryFX), Coherence.MessageTarget.Other, soundVariationIndex);
    }

    public override void TakeWeaponDamageSync(int damage, CoherenceSync Damagersync)
    {
        Debug.Log("sync humanoid took " + damage + " weapon damage!");

        m_HumanoidFX.PlayHurtFX(0);
        m_Sync.SendCommand<HumanoidFX>(nameof(HumanoidFX.PlayHurtFX), Coherence.MessageTarget.Other, 0);
        Damagersync.SendCommand<EntityCommands>(nameof(EntityCommands.SyncHitCommand), Coherence.MessageTarget.AuthorityOnly); //

        TakeDamageSync(damage, Damagersync);

    }

    void AttackCheckUpdate()
    {
        m_AttackCheckTimer += Time.deltaTime;
        if (m_AttackCheckTimer >= m_AttackCheckTime)
        {
            m_AttackCheckTimer = 0f;
            CheckTargetAttack();


        }
    }

    void CheckTargetAttack()
    {
        if (m_FollowTarget.TryGetComponent<CoherenceSync>(out CoherenceSync entSync))
        {
            if (m_FollowTarget.TryGetComponent<EntityCommands>(out EntityCommands entityCommands))
            {
                entSync.SendCommand<EntityCommands>(nameof(EntityCommands.GetAttackState), Coherence.MessageTarget.AuthorityOnly, m_Sync);
                //entityCommands.GetAttackState(m_Sync);
            }
        }
    }

    public override bool GetAttackState(out EWeaponDirection attackDir)
    {
        attackDir = m_WeaponDirection;
        return true;
    }
    public override void OnReceiveAttackState(bool isAttacking, EWeaponDirection attackDir)
    {
        base.OnReceiveAttackState(isAttacking, attackDir);
        if (isAttacking)
        {
            m_AttackerDirection = attackDir; 
            if(m_AttackState == EAttackState.Parrying) SetParry(attackDir);
            SwitchAttackState(EAttackState.Parrying);

        }
        else
        {

            if (m_AttackState == EAttackState.Parrying) SwitchAttackState(EAttackState.None);
        }
       

    }

}
