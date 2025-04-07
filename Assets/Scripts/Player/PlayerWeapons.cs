using Coherence;
using Coherence.Toolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Coherence.Core.NativeTransport;




public class PlayerWeapons : MonoBehaviour
{
    

    TinyPlayer m_TinyPlayer;
    PlayerLoadout m_PlayerLoadout;
    Animator m_Animator; 
    internal CoherenceSync m_Sync;



    [Header("Weapon Infos")]
    [SerializeField] internal bool m_TwoHanded = false;
    [SerializeField] internal Vector2 m_LookDirection = Vector2.zero;
    [SerializeField] internal EWeaponDirection m_WeaponDirection = EWeaponDirection.Right;
    [SerializeField] internal EWeaponType m_MainWeaponType = EWeaponType.Hands;
    [SerializeField] internal bool m_Parrying = false;
    [SerializeField] internal bool m_Attacking = false;
    [SerializeField] bool m_CanAttack = true;

    [SerializeField] bool m_LockedAttackRelease = false;

    Coroutine m_AttackReleaseLockRoutine; 


    [Header("Weapon Parameters")]

    [SerializeField] float m_BaseReleaseDelay = 0.15f;
    [SerializeField] float m_BaseAttackCoolDown = 0.2f;
    [SerializeField] float m_BlockedAttackCooldown = 0.5f;

    [SerializeField] int m_BareHanded_Damage = 5; 
    [SerializeField] float m_BareHanded_Range= 2f; 

    [Space(10)]
    [Header("Parry Parameters")]
    [SerializeField] float m_ParryAngle = 20f;

    [SerializeField] public AudioClip[] m_FistHitSounds; 


    public bool InParry => (m_WeaponState == EWeaponState.Parry);

    public bool InShieldParry => (m_WeaponState == EWeaponState.ShieldParry); 

    public bool Throwing { get; set; }

    public bool UsingMagic { get; set; }

    public bool UsingItem { get; set; }


    public enum EWeaponState
    {
        None,
        AttackReady,
        AttackRelease,
        Parry,
        ShieldParry,
    }
    public EWeaponState m_WeaponState = EWeaponState.None;


    private void Awake()
    {
        m_TinyPlayer = GetComponent<TinyPlayer>();
        m_Animator = m_TinyPlayer.m_Animator;
        m_PlayerLoadout = GetComponent<PlayerLoadout>();
        m_Sync = GetComponent<CoherenceSync>();
        

    }
    void Start()
    {
        UpdateMainWeaponType(); 
        AnimatorStateInfo stateInfo = m_Animator.GetCurrentAnimatorStateInfo(1);
        AnimatorStateInfo nextStateInfo = m_Animator.GetNextAnimatorStateInfo(1);
    }

    
    void Update()
    {

        if(m_TinyPlayer.m_IsStunned || m_Animator.GetBool("Stunned"))
        {
            SetWeaponsNeutralState(); 
        }

        if (UsingMagic)
        {

            return; 
        }

        if (Throwing)
        {
            if (m_Attacking)
            {
                m_PlayerLoadout.UseThrowingItem();
                StartCoroutine(LockAttackRoutine(m_BaseAttackCoolDown)) ; 
            }

            return; 
        }
        if (UsingItem)
        {
            return; 
        }

        StateUpdate(); 

        
    }

    private void OnDisable()
    {
        m_PlayerLoadout.DropEverything();
    }

    

    #region WeaponActions


    void SetAnimatorWeaponDirection()
    {
        if(GetSecondaryWeapon() != null &&  m_MainWeaponType == EWeaponType.Hands )
        {
            m_Animator.SetInteger("WeaponDirectionNESO", 0);
            return; 
        }
 
        switch (m_WeaponDirection)
        {
            case EWeaponDirection.Right:
                m_Animator.SetInteger("WeaponDirectionNESO", 1);
                break;
            case EWeaponDirection.Left:
                m_Animator.SetInteger("WeaponDirectionNESO", 3);
                break;
            case EWeaponDirection.Up:
                m_Animator.SetInteger("WeaponDirectionNESO", 0);
                break;
            case EWeaponDirection.Down:
                m_Animator.SetInteger("WeaponDirectionNESO", 2);
                break;
            default:
                break;
        }
    }

    void GetWeaponDirection()
    {
        if (MathF.Abs(m_LookDirection.x) > MathF.Abs(m_LookDirection.y))
        {
            m_WeaponDirection = m_LookDirection.x > 0 ? EWeaponDirection.Right : EWeaponDirection.Left;
        }
        else
        {
            m_WeaponDirection = m_LookDirection.y > 0 ? EWeaponDirection.Up : EWeaponDirection.Down;
        }


    }



    void StateUpdate()
    {
        if (m_TinyPlayer.m_IsStunned)
        {
            return;
        }

        switch (m_WeaponState)
        {
            case EWeaponState.None:

                if (m_Parrying)
                {

                    SwitchWeaponState(DoPossesShield() ?EWeaponState.ShieldParry : EWeaponState.Parry);

                    return; 
                }
                if(m_Attacking)
                {
                    if (!m_CanAttack) return;

                    //if (m_PlayerLoadout.m_EquippedItems[EStuffSlot.MainWeapon] == null) return;
                    SwitchWeaponState(EWeaponState.AttackReady);
                    return; 
                }

                break;
            case EWeaponState.AttackReady:
                if (!m_Attacking)
                {
                    SwitchWeaponState(EWeaponState.AttackRelease); 
                }
                break;
            case EWeaponState.AttackRelease:

                if (m_Parrying)
                {
                    if (m_LockedAttackRelease) return;
                    SwitchWeaponState(DoPossesShield() ? EWeaponState.ShieldParry : EWeaponState.Parry);
                }
                break;
            case EWeaponState.Parry:
                if (!m_Parrying)
                {
                    SwitchWeaponState(EWeaponState.None);
                }
                break;
            case EWeaponState.ShieldParry:
                if (!m_Parrying)
                {
                    SwitchWeaponState(EWeaponState.None);
                }
                break;
            default:
                break;
        }
    }
    void OnEnterWeaponState()
    {
        switch (m_WeaponState)
        {
            case EWeaponState.None:
                break;
            case EWeaponState.AttackReady:
                

                GetWeaponDirection();
                SetAnimatorWeaponDirection();
                

                m_Animator.SetBool("Attacking", true);
                //GetMainWeapon().RaiseAttackEffect();

                switch (m_MainWeaponType)
                {
                    case EWeaponType.Hands:
                        break;
                    case EWeaponType.Melee:
                        break;
                    case EWeaponType.Staff:
                        break;
                    case EWeaponType.Ranged:
                        break;
                    case EWeaponType.Shield:
                        break;
                    default:
                        break;
                }
                break;
            case EWeaponState.AttackRelease:

                m_Animator.SetBool("Attacking", false);
                //GetMainWeapon().ReleaseAttackEffect();
                break;
            case EWeaponState.Parry:
                GetWeaponDirection();
                SetAnimatorWeaponDirection();
                //GetMainWeapon().RaiseBlockEffect();

                m_Animator.SetBool("Parry", true);
                break;
            case EWeaponState.ShieldParry:
                //GetMainWeapon().RaiseBlockEffect();
                m_Animator.SetBool("ShieldParry", true);
                m_Animator.SetBool("Parry", true);
                break;
            default:
                break;
        }
    }

    void OnExitWeaponState()
    {
        switch (m_WeaponState)
        {
            case EWeaponState.None:
                
                break;
            case EWeaponState.AttackReady:
                break;
            case EWeaponState.AttackRelease:
                if(m_AttackReleaseLockRoutine != null) StopCoroutine(m_AttackReleaseLockRoutine);

                m_LockedAttackRelease = false; 
                break;
            case EWeaponState.Parry:
                m_Animator.SetBool("Parry", false);
                break;
            case EWeaponState.ShieldParry:
                m_Animator.SetBool("ShieldParry", false);
                m_Animator.SetBool("Parry", false);
                break;
            default:
                break;
        }
    }

    public void SwitchWeaponState(EWeaponState state)
    {
        if (state == m_WeaponState)
        {
            return;
        }

        //Debug.Log("Switching weapon state from " + m_WeaponState + " to " + state); 

        OnExitWeaponState();
        m_WeaponState = state;
        OnEnterWeaponState();
    }

    bool DoPossesShield()
    {
        return m_PlayerLoadout.m_EquippedItems[EStuffSlot.SecondaryWeapon] != null
               && m_PlayerLoadout.m_EquippedItems[EStuffSlot.SecondaryWeapon].GetComponent<BasicWeapon>().WeaponParameters.WeaponType == EWeaponType.Shield; 
    }
        

    public void LockAttackRelease(bool isLocked)=> m_LockedAttackRelease = isLocked;

    public void LockAttack(bool isLocked) => m_CanAttack = !isLocked;
    public void LockAttack()
    {
        //Debug.Log("locking attack");
        m_LockedAttackRelease = true;
        if(m_PlayerLoadout.m_EquippedItems[EStuffSlot.MainWeapon] !=null) m_PlayerLoadout.m_EquippedItems[EStuffSlot.MainWeapon].GetComponent<BasicWeapon>().m_HolderTransform = transform;
        if(m_PlayerLoadout.m_EquippedItems[EStuffSlot.SecondaryWeapon] != null) m_PlayerLoadout.m_EquippedItems[EStuffSlot.SecondaryWeapon].GetComponent<BasicWeapon>().m_HolderTransform = transform;


        float normalizedTime = m_Animator.GetCurrentAnimatorStateInfo(1).normalizedTime;
        float animLenght = m_Animator.GetCurrentAnimatorStateInfo(1).length;

        // 0.3 normal => length = 2.4 ___________ time = 0.3 * 2.4 = 0.72 so 1.68 is the right time
        // 1 - normalizedTime * animLenght = time remaining 

        m_AttackReleaseLockRoutine =  StartCoroutine(LockAttackRoutine((1 - normalizedTime) * animLenght));
    }

    IEnumerator LockAttackRoutine(float time)
    {
        //Debug.Log("lock attack routine "); 
        float timeToWait = time + m_BaseReleaseDelay;
        yield return new WaitForSeconds(timeToWait);
        //Debug.Log("Unlocking attack");
        m_LockedAttackRelease = false;
        if(m_WeaponState == EWeaponState.AttackRelease)
        {
            SwitchWeaponState(m_Parrying ? DoPossesShield() ? EWeaponState.ShieldParry : EWeaponState.Parry : m_Attacking ? EWeaponState.AttackReady : EWeaponState.None);
        }
    }
        //
    IEnumerator AttackCoolDownRoutine(float time)
    {
        m_CanAttack = false;
        yield return new WaitForSeconds(time);
        m_CanAttack = true;
    }

   

    public void SyncBlocked()
    {
        Debug.Log("i get sync blocked ");
        m_Animator.SetTrigger("Blocked");
        //m_InAttack = false; 
        m_LockedAttackRelease = false;
        m_Attacking = false;
        m_Sync.SendCommand<Animator>(nameof(Animator.SetTrigger), MessageTarget.Other, "Blocked");
        StartCoroutine(AttackCoolDownRoutine(m_BlockedAttackCooldown));
    }


    public void SyncHit()
    {
        Debug.Log("i get sync Hit ");



        //sounds

        m_Sync.SendCommand<EntityCommands>(nameof(EntityCommands.PlayDamageSoundCommand), MessageTarget.Other, (int)m_MainWeaponType);
        m_TinyPlayer.PlayDamageSound(m_MainWeaponType);

    }

    public bool IsInParryAngle(Vector3 enemyPosition)
    {

        return transform.IsInAngle(m_ParryAngle, enemyPosition);

    }





    #endregion

    #region EquipAndDrop
    public void EquipGrabbable(Grabbable grabbable)
    {

        m_PlayerLoadout.EquipGrabbableItem(grabbable);

    }

    public void SetWeaponParameters(BasicWeapon weapon)
    {
        if (weapon.GetType() == typeof(MeleeWeapon))
        {
            float speed = ((MeleeWeapon)weapon).MeleeWeaponParameters.Speed; 
            m_Animator.SetFloat("WeaponSpeed", speed);
        }
        UpdateMainWeaponType();
        
    }
    public void UpdateMainWeaponType()
    {
        if(GetMainWeapon() == null)
        {
            m_MainWeaponType = EWeaponType.Hands;
            m_Animator.SetBool("TwoHanded", false);
            m_Animator.SetFloat("WeaponSpeed", 1f);
        }
        else
        {
            m_Animator.SetBool("TwoHanded", GetMainWeapon().WeaponParameters.WeaponSize == EWeaponSize.Two_Handed);
            m_MainWeaponType = GetMainWeapon().WeaponParameters.WeaponType;
            
        }
        m_Animator.SetBool("BareHanded", m_MainWeaponType == EWeaponType.Hands);
    }

    public void Drop(float throwForce = 5f)
    {
        Debug.Log("dropping"); 
        m_PlayerLoadout.DropWeapons(throwForce);
        UpdateMainWeaponType();


    }
    #endregion

    public BasicWeapon GetMainWeapon()
    {
        if(m_PlayerLoadout.m_EquippedItems[EStuffSlot.MainWeapon] == null) 
        {
            Debug.Log("no main weapon ");
            return null;
        }
        return m_PlayerLoadout.m_EquippedItems[EStuffSlot.MainWeapon].GetComponent<BasicWeapon>();
    }

    public BasicWeapon GetSecondaryWeapon()
    {
        if (m_PlayerLoadout.m_EquippedItems[EStuffSlot.SecondaryWeapon] == null)
        {
            Debug.Log("no secondary weapon ");
            return null;
        }
        return m_PlayerLoadout.m_EquippedItems[EStuffSlot.SecondaryWeapon].GetComponent<BasicWeapon>();
    }

    public void SetWeaponsNeutralState()
    {
        m_Attacking = false;
        m_Parrying = false;
        m_CanAttack = true;

        SwitchWeaponState(EWeaponState.None);
        UpdateMainWeaponType(); 
    }

   
    public void DoBareHandedDamage(bool isLeft)
    {
        if(m_Sync.HasStateAuthority == false)
        {
            return;
        }
        Vector3 pos = isLeft ? m_PlayerLoadout.m_PlayerLeftHandSocket.position : m_PlayerLoadout.m_PlayerRightHandSocket.position;

        foreach (Collider collider in Physics.OverlapSphere(pos, m_BareHanded_Range))
        {
            if(collider.TryGetComponent<Entity>(out Entity ent))
            {
                if(collider.transform == transform)
                {
                    Debug.Log("cant punch itself i'm sorry ");
                    continue;
                }

                Debug.Log("fist punch found entity");
                if(collider.TryGetComponent<CoherenceSync>(out CoherenceSync sync))
                {
                    if(transform.IsInAngle(m_ParryAngle, collider.transform.position))
                    {
                        sync.SendCommand<EntityCommands>(nameof(EntityCommands.TakeMeleeCommand), Coherence.MessageTarget.AuthorityOnly, (int)m_WeaponDirection, m_Sync, m_BareHanded_Damage, (int)EEffectType.Physical, transform.position);
                    }
                }
                  
            }
        }

    }

    public void ReleaseAttackWeaponEffect()
    {
        if (m_Sync.HasStateAuthority == false)
        {
            return;
        }
        if (GetMainWeapon() == null)
        {
            return;
        }

        GetMainWeapon().ReleaseAttackEffect();
    }
   


    private void OnDrawGizmos()
    {
        float parryAngleRadians = Mathf.Deg2Rad * m_ParryAngle;
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // Draw lines for positive and negative parry angles
        Vector3 leftParryDirection = Quaternion.AngleAxis(-m_ParryAngle, Vector3.up) * forward;
        Vector3 rightParryDirection = Quaternion.AngleAxis(m_ParryAngle, Vector3.up) * forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftParryDirection * 2f);
        Gizmos.DrawLine(transform.position, transform.position + rightParryDirection * 2f);


        if (Physics.OverlapSphere(transform.position, 15f).Length>0)
        {
            foreach (var item in Physics.OverlapSphere(transform.position, 15f))
            {
                if(item.TryGetComponent<Dummy>(out Dummy dummy) && m_PlayerLoadout.m_EquippedItems[EStuffSlot.MainWeapon] != null)
                {

                    float dot = Vector3.Dot(dummy.transform.forward.normalized, (dummy.transform.position - transform.position).normalized);


                    float dotInDeg = Mathf.Acos(dot) * Mathf.Rad2Deg;

                    if (dotInDeg <= m_ParryAngle)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawLine(dummy.transform.position, m_PlayerLoadout.m_EquippedItems[EStuffSlot.MainWeapon].GetComponent<BasicWeapon>().m_HitPos.position);

                    }
                    else
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawLine(dummy.transform.position, m_PlayerLoadout.m_EquippedItems[EStuffSlot.MainWeapon].GetComponent<BasicWeapon>().m_HitPos.position);
                    }

                    

                    
                }
                

                

            }
        }
        


        
    }

}
