using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapons : MonoBehaviour
{
    enum EWeaponDirection
    {
        Right,
        Left,
        Up,
        Down,
    }

    TinyPlayer m_TinyPlayer;
    [SerializeField] Animator m_Animator; 

    [SerializeField] Grabbable m_GrabbedItem;

    [SerializeField] BasicWeapon m_MainWeapon; 
    [SerializeField] BasicWeapon m_SecondaryWeapon;

    [Header("Weapon Infos")]
    [SerializeField] internal Vector2 m_LookDirection = Vector2.zero;
    [SerializeField] EWeaponDirection m_WeaponDirection = EWeaponDirection.Right;
    [SerializeField] internal bool m_Parrying = false;
    [SerializeField] bool m_InParry = false; 
    [SerializeField] internal  bool m_Attacking = false;
    [SerializeField] bool m_InAttack = false;
    [SerializeField] bool m_InAttackRelease = false;


    [Header("Weapon Parameters")]

    [SerializeField] float m_BaseReleaseDelay = 0.15f;



    private void Awake()
    {
        m_TinyPlayer = GetComponent<TinyPlayer>();
        

    }
    void Start()
    {
        
    }

    
    void Update()
    {
        if(m_MainWeapon == null)
        {
            return; 
        }
        ParryUpdate();
        AttackUpdate(); 
        
    }

    private void OnDisable()
    {
        if (m_GrabbedItem != null)
        {
            Drop();
        }
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

    void ParryUpdate()
    {
        if (m_InAttackRelease)
        {
            return;
        }

        if (m_Parrying)
        {
            if (m_InParry)
            {
                return;
            }

            m_InParry = true;

            GetWeaponDirection(); 

            Parry();
        }
        else
        {
            ReleaseParry();
        }

    }

    internal void Parry()
    {
        Debug.Log("Parry");
        Debug.Log("Parry on " + m_WeaponDirection);

        m_Parrying = true;

        SetAnimatorWeaponDirection(); 

        m_Animator.SetBool("Parry", true);

    }

    void ReleaseParry()
    {
        m_Animator.SetBool("Parry", false);
        m_InParry = false;

    }

    void AttackUpdate()
    {
        if (m_Parrying)
        {
               return;
        }
        if (m_InAttackRelease)
        {
            return; 
        }
        if (m_Attacking)
        {
            if(m_InAttack)
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
        Debug.Log("Attack");
        Debug.Log("Attack on " + m_WeaponDirection);

        m_Attacking = true;

        SetAnimatorWeaponDirection();

        m_Animator.SetBool("Attacking", true);
    }

    void ReleaseAttack()
    {
        m_Animator.SetBool("Attacking", false);
        m_InAttack = false;
    }

    public void LockAttackRelease(bool isLocked)=> m_InAttackRelease = isLocked;

    public void LockAttack()
    {
        Debug.Log("locking attack");
        m_InAttackRelease = true;
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
        Debug.Log("Unlocking attack");
        m_InAttackRelease = false;
    }
    

    
    #endregion

    #region EquipAndDrop
    public void EquipWeapon(Grabbable weapon)
    {
        
        m_GrabbedItem = weapon;
        m_GrabbedItem.m_Rigidbody.isKinematic = true;
        m_GrabbedItem.m_Collider.enabled = false;
        m_GrabbedItem.transform.SetParent(m_TinyPlayer.m_PlayerRightHandSocket, false);

        m_GrabbedItem.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        if(m_GrabbedItem.TryGetComponent<BasicWeapon>(out BasicWeapon basicWeapon))
        {
            m_MainWeapon = basicWeapon;
            
        }

        

    }

    public void Drop(float throwForce = 0f)
    {
        if(m_GrabbedItem == null)
        {
            return; 
        }

        m_GrabbedItem.transform.SetParent(null, true);
        m_GrabbedItem.m_Rigidbody.isKinematic = false;
        m_GrabbedItem.m_Rigidbody.AddForce(throwForce * transform.forward, ForceMode.VelocityChange);
        m_GrabbedItem.m_Rigidbody.AddTorque(-transform.right * throwForce * 1f, ForceMode.VelocityChange);

        m_GrabbedItem.Release();

        m_GrabbedItem.m_Collider.enabled = true; 
        m_GrabbedItem = null;
        m_MainWeapon = null;


    }

   
    #endregion

}
