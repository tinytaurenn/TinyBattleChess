using Coherence.Toolkit;
using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Dummy : MonoBehaviour, IDamageable
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

    

  
    public void TakeMeleeSync(int DirectionNESO, CoherenceSync sync,int damage, Vector3 attackerPos)
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

        if (m_InParry && parry && IsInParryAngle(attackerPos))
        {
            ParrySync(damage, sync);
            
        }
        else
        {
            TakeDamageSync(damage,sync);
            

        }

    }

    public bool IsInParryAngle(Vector3 enemyPosition) 
    {


        float dot = Vector3.Dot(transform.forward.normalized, (enemyPosition - transform.position).normalized);


        float dotInDeg = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (dotInDeg <= m_ParryAngle)
        {
            Debug.Log(" parried! " + "angle is : " + dotInDeg); 
            return true; 

        }
        else
        {
            Debug.Log(" not parried! " + "angle is : " + dotInDeg);
            return false; 
        }


    }

    public void TakeDamageSync(int damage, CoherenceSync Damagersync)
    {
        Debug.Log("sync Dummy took " + damage + " damage!");
    }
    public void ParrySync(int damage,CoherenceSync DamagerSync)
    {
        Debug.Log("sync Dummy parried "); 
        //Debug.Log(DamagerSync.transform.name + " parried!"); // cant get a transform from serv side
        DamagerSync.SendCommand<PlayerWeapons>(nameof(PlayerWeapons.SyncBlocked), Coherence.MessageTarget.AuthorityOnly);
        int soundVariationIndex = UnityEngine.Random.Range(0, 3); 
        m_Sync.SendCommand<DummyFX>(nameof(DummyFX.PlayParryFX), Coherence.MessageTarget.All, soundVariationIndex); 
    }

    


}
