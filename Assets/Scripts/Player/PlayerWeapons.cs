using Coherence;
using Coherence.Toolkit;
using System;
using System.Collections;
using UnityEngine;




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
    [SerializeField] internal EWeaponType m_MainWeaponType = EWeaponType.Sword;
    [SerializeField] internal bool m_Parrying = false;
    [SerializeField] bool m_InParry = false; 
    [SerializeField] internal bool m_Attacking = false;
    [SerializeField] bool m_InAttack = false;
    [SerializeField] bool m_InAttackRelease = false;
    [SerializeField] bool m_InShieldParry = false;
    [SerializeField] bool m_CanAttack = true;


    [Header("Weapon Parameters")]

    [SerializeField] float m_BaseReleaseDelay = 0.15f;
    [SerializeField] float m_BaseAttackCoolDown = 0.2f;
    [SerializeField] float m_BlockedAttackCooldown = 0.5f;

    [Space(10)]
    [Header("Parry Parameters")]
    [SerializeField] float m_ParryAngle = 20f;


    public bool InAttackReady => (m_InAttack && m_Attacking && !m_InAttackRelease); 
    public bool InAttackRelease => (m_InAttackRelease); 
    public bool InParry => (m_Parrying && m_InParry);

    public bool InShieldParry => (m_Parrying && m_InParry && m_InShieldParry); 

    public bool Throwing { get; set; }

    public bool UsingMagic { get; set; }

    public bool UsingItem { get; set; }



    private void Awake()
    {
        m_TinyPlayer = GetComponent<TinyPlayer>();
        m_Animator = m_TinyPlayer.m_Animator;
        m_PlayerLoadout = GetComponent<PlayerLoadout>();
        m_Sync = GetComponent<CoherenceSync>();
        

    }
    void Start()
    {

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



        if (m_PlayerLoadout.m_EquippedItems[EStuffSlot.MainWeapon] == null
            && m_PlayerLoadout.m_EquippedItems[EStuffSlot.SecondaryWeapon] == null )
        {
            return; //to change 
        }
        WeaponsUpdate(); 
        
    }

    private void OnDisable()
    {
        m_PlayerLoadout.DropEverything();
    }

    

    #region WeaponActions


    void SetAnimatorWeaponDirection()
    {
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

    void WeaponsUpdate()
    {
        if (m_TinyPlayer.m_IsStunned)
        {
            return;
        }

        ParryUpdate();
        AttackUpdate();
    }

    void ParryUpdate()
    {
        
        if (m_InAttackRelease)
        {
            return;
        }


        if (m_Parrying)
        {

            if (m_InAttack)
            {
                m_InAttack = false;
                m_Animator.SetBool("Attacking", false);
            }

            if (m_InParry)
            {
                return;
            }

            m_InParry = true;

            if (m_PlayerLoadout.m_EquippedItems[EStuffSlot.SecondaryWeapon] != null 
                && m_PlayerLoadout.m_EquippedItems[EStuffSlot.SecondaryWeapon].GetComponent<BasicWeapon>().WeaponParameters.WeaponType == EWeaponType.Shield)
            {
                Debug.Log("got some shield");
                ShieldParry(); 
                return; 
            }

            GetWeaponDirection(); 

            Parry();
        }
        else
        {
            ReleaseParry();
        }

    }
    internal void ShieldParry()
    {
        m_InShieldParry = true;
        m_Parrying = true;
        m_InAttack = false;
        m_Animator.SetBool("ShieldParry", true);
        m_Animator.SetBool("Parry", true);
    }

    internal void Parry()
    {
        //Debug.Log("Parry");
        //Debug.Log("Parry on " + m_WeaponDirection);

        m_Parrying = true;
        m_InAttack = false;

        SetAnimatorWeaponDirection(); 

        m_Animator.SetBool("Parry", true); 

    }

    void ReleaseParry()
    {
        m_Animator.SetBool("ShieldParry", false);
        m_Animator.SetBool("Parry", false);
        m_InParry = false;
        m_InShieldParry = false; 

    }

    void AttackUpdate()
    {
        if (!m_CanAttack)
        {
            return;
        }
        if (m_Parrying)
        {
               return;
        }
        if (m_InAttackRelease)
        {
            return; 
        }
        if (m_PlayerLoadout.m_EquippedItems[EStuffSlot.MainWeapon] == null)
        {
            return; 
        }
        if (m_Attacking)
        {
            if(m_InAttack )
            {
                return; 
            }

            m_InAttack = true;
            GetWeaponDirection(); 
            Attack();
        }
        else
        {
            ReleaseAttack();
        }
    }

    internal void Attack()
    {
        //Debug.Log("Attack");
        //Debug.Log("Attack on " + m_WeaponDirection);

        m_Attacking = true;

        SetAnimatorWeaponDirection();

        m_Animator.SetBool("Attacking", true);
    }

    void ReleaseAttack()
    {
        m_Animator.SetBool("Attacking", false);
        
    }

    public void LockAttackRelease(bool isLocked)=> m_InAttackRelease = isLocked;

    public void LockAttack(bool isLocked) => m_CanAttack = !isLocked;
    public void LockAttack()
    {
        //Debug.Log("locking attack");
        m_InAttackRelease = true;
        if(m_PlayerLoadout.m_EquippedItems[EStuffSlot.MainWeapon] !=null) m_PlayerLoadout.m_EquippedItems[EStuffSlot.MainWeapon].GetComponent<BasicWeapon>().m_HolderTransform = transform;
        if(m_PlayerLoadout.m_EquippedItems[EStuffSlot.SecondaryWeapon] != null) m_PlayerLoadout.m_EquippedItems[EStuffSlot.SecondaryWeapon].GetComponent<BasicWeapon>().m_HolderTransform = transform;


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
        m_InAttack = false; 
        m_InAttackRelease = false;
        m_Attacking = false;
        m_Sync.SendCommand<Animator>(nameof(Animator.SetTrigger), MessageTarget.Other, "Blocked");
        StartCoroutine(AttackCoolDownRoutine(m_BlockedAttackCooldown));
    }


    public void SyncHit()
    {
        Debug.Log("i get sync Hit ");

        m_PlayerLoadout.m_EquippedItems[EStuffSlot.MainWeapon].GetComponent<BasicWeapon>().PlayHitSound();
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

    public void SetTwoHanded(bool twohanded)
    {
        m_TwoHanded = twohanded;
        m_Animator.SetBool("TwoHanded", m_TwoHanded);
    }

    public void SetWeaponParameters(BasicWeapon weapon)
    {
        if (weapon.GetType() == typeof(MeleeWeapon))
        {
            float speed = ((MeleeWeapon)weapon).MeleeWeaponParameters.Speed; 
            m_Animator.SetFloat("WeaponSpeed", speed);
        }
        m_MainWeaponType = weapon.WeaponParameters.WeaponType;  
        
    }

    public void Drop(float throwForce = 5f)
    {
        Debug.Log("dropping"); 
        m_PlayerLoadout.DropWeapons(throwForce);


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
        if (m_InParry || m_Parrying)
        {
            m_Animator.SetBool("Parry", false);
            m_InParry = false;
            m_Parrying = false;
        }
        if (m_InAttack || m_Attacking || m_InAttackRelease)
        {
            m_InAttack = false;
            m_InAttackRelease = false;
            m_Attacking = false;
            m_Animator.SetBool("Attacking", false);
        }
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
