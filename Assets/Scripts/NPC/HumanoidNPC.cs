using Coherence;
using Coherence.Toolkit;
using System.Collections;
using UnityEngine;

public class HumanoidNPC : TinyNPC
{

    public enum EWeaponDirection
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
    }

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

    [SerializeField] float m_BaseReleaseDelay = 0.15f;
    [SerializeField] float m_ParryAngle = 20f;
    [SerializeField] float m_HoldAttackDistance = 3f; 


    [SerializeField] bool m_InShieldParry = false;
    public bool InShieldParry
    {
        get { return m_InShieldParry && m_InParry; }
    }
    [SerializeField] protected bool m_InAttackRelease;

    [SerializeField] protected bool m_CanAttack = true;
    [Sync]
    public bool CanAttack
    {
        get { return m_CanAttack; }
        set { m_CanAttack = value; }
    }
   
    [SerializeField] protected float m_AttackDelay = 1f;

    protected override void OnAttack()
    {
        base.OnAttack();
        Debug.Log("HumanoidNPC attacking their target");
        
    }


    protected override void AttackUpdate()
    {
        base.AttackUpdate();

        if (m_FollowTarget == null) return;
        float distanceFromTarget = Vector3.Distance(transform.position, m_FollowTarget.position);


        if(distanceFromTarget <= m_StopDistance)
        {
            //Debug.Log("release attack");
            SwitchAttackState(EAttackState.Release);
        }
        else if (distanceFromTarget <= m_HoldAttackDistance)
        {
            SwitchAttackState(EAttackState.Attacking);
        }
        else
        {
            //Debug.Log("stop attacking");
            SwitchAttackState(EAttackState.None);
        }


        


        switch (m_AttackState)
        {
            case EAttackState.None:
               
                break;
            case EAttackState.Attacking:
                break;
            case EAttackState.Release:
                
                if (m_CanAttack && !InAttack)
                {
                    SwitchAttackState(EAttackState.Attacking);
                }
                break;
            case EAttackState.Parrying:
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
                break;
            case EAttackState.Attacking:
                if (m_CanAttack)
                {
                    InAttack = true;
                    m_WeaponDirection = (EWeaponDirection)UnityEngine.Random.Range(0, 4); // temporary


                    m_Animator.SetInteger("WeaponDirectionNESO", (int)m_WeaponDirection);

                    m_Animator.SetBool("Attacking", InAttack);

                    StartCoroutine(AttackDelayRoutine(m_AttackDelay));
                }
                
                break;
            case EAttackState.Release:
                InAttack = false;
                StartCoroutine(ReleaseDelayRoutine(m_BaseReleaseDelay));
                break;
            case EAttackState.Parrying:
                InAttack = false;
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

        m_CanAttack = false;
        yield return new WaitForSeconds(time);

        m_CanAttack = true;
    }

    IEnumerator ReleaseDelayRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        m_Animator.SetBool("Attacking", false);
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
    }

    public override void ParrySync(int damage, CoherenceSync DamagerSync)
    {
        base.ParrySync(damage, DamagerSync);
    }

    public override void TakeDamageSync(int damage, CoherenceSync Damagersync)
    {
        base.TakeDamageSync(damage, Damagersync);
    }

    public override void TakeMeleeSync(int DirectionNESO, CoherenceSync sync, int damage, Vector3 attackerPos)
    {
        base.TakeMeleeSync(DirectionNESO, sync, damage, attackerPos);
    }

    public override void TakeWeaponDamageSync(int damage, CoherenceSync Damagersync)
    {
        base.TakeWeaponDamageSync(damage, Damagersync);
    }

}
