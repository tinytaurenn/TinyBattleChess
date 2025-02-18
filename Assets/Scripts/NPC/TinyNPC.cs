using Coherence.Connection;
using Coherence.Toolkit;
using System.Collections;
using UnityEngine;


public abstract class TinyNPC : Entity, IDamageable
{
    public enum EMovementType
    {
        Idle = 0,
        Patrol = 1,
        Follow= 2,
        Flee= 3,
        Attack= 4,
        Dead = 5
    }

    public enum ENPCBehavior
    {
       Aggressive = 0,
       Neutral = 1,
       Friendly = 2
    }

    [Header("Setup Settings")]
    [SerializeField] protected Animator m_Animator;

    protected CoherenceSync m_Sync;

    protected NPC_Movement m_Movement;

    [Space(10)]
    [Header("Global")]

    [SerializeField] EMovementType m_MovementType = EMovementType.Idle;
    [Sync]
    public int IntMovementType
    {
        get { return (int)m_MovementType; }
        set { m_MovementType = (EMovementType)value; }
    }

    [SerializeField] ENPCBehavior m_NPCBehavior = ENPCBehavior.Neutral;

    [Sync]
    public int IntNPCBehavior
    {
        get { return (int)m_NPCBehavior; }
        set { m_NPCBehavior = (ENPCBehavior)value; }
    }

    //team ID
   
    [SerializeField] Transform m_ClosestTarget;
    [SerializeField] protected Transform m_FollowTarget;

    [SerializeField] protected float m_StopDistance = 1f;
    [SerializeField] Vector3 m_StartPosition;


    [Space(10)]
    [Header("Stats")]
    [SerializeField] int m_Health = 100;
    [Sync]
    public int Health
    {
        get { return m_Health; }
        set { m_Health = value; }
    }

    [Space(10)]
    [Header("Summoned")]
    [SerializeField] Transform m_MasterTransform = null; 

    [Space(10)]
    [Header("Detection")]
    [SerializeField] float m_DetectionRadius = 10f;
    [SerializeField] LayerMask m_DetectionLayer;
    [SerializeField] bool m_IsPatrolUnit = false;


    [Space(10)]
    [Header("Patrol")]
    [SerializeField] bool m_IsPatrolWaiting = false;
    [SerializeField] bool m_IsFreeRoam = true;
    [SerializeField] float m_PatrolRadius = 10f;
    [SerializeField] Vector3 m_NextPatrolPosition;
    [SerializeField] Vector3 m_PreviousPatrolPosition;
    [SerializeField] float m_PatrolTimer = 0f;
    [SerializeField] float m_PatrolWaitTime = 5f;

    [Space(10)]
    [Header("Aggro")]
    [SerializeField] float m_AggroTimer = 0f;
    [SerializeField] float m_AggroTime = 5f;
    [SerializeField] bool m_AfterAggroStanding = false;
    [SerializeField] float m_AfterAggroStandingTime = 3f;
    [SerializeField] float m_AfterAggroStandingTimer = 0f;
    Coroutine m_AfterAggroStandingRoutine;

    [SerializeField] protected bool m_InAttack = false;
    public bool InAttack
    {
        get { return m_InAttack; }
        set { m_InAttack = value; }
    }
    [SerializeField] protected bool m_InParry = false;
    public bool InParry
    {
        get { return m_InParry; }
        set { m_InParry = value; }
    }

    [SerializeField] protected bool m_IsStunned = false;
    [Sync]
    public bool IsStunned
    {
        get { return m_IsStunned; }
        set { m_IsStunned = value; }
    }

    

    void Awake()
    {
        m_Movement = GetComponent<NPC_Movement>();
        m_Sync = GetComponent<CoherenceSync>();

        m_Sync.CoherenceBridge.onDisconnected.AddListener(OnDisconnected);

        
    }

    private void OnDisconnected(CoherenceBridge arg0, ConnectionCloseReason arg1)
    {
        Debug.Log("Skeleton disconnected");
        if (m_Sync.CoherenceBridge != null) m_Sync.CoherenceBridge.onDisconnected.RemoveListener(OnDisconnected);
        if (this.gameObject != null) Destroy(gameObject);
    }

    void Start()
    {

        if (m_MovementType == EMovementType.Dead)
        {
            m_Movement.enabled = false;
            this.enabled = false;
            return;
        }

        m_StartPosition = transform.position;


        m_MovementType = m_IsPatrolUnit ? EMovementType.Patrol : EMovementType.Idle;
        OnEnterState(); 
    }

    // Update is called once per frame
    void Update()
    {
        if ((EMovementType)IntMovementType == EMovementType.Dead) return;
        UpdateState();
        EnemyDetection(); 
        
    }

    public void SwitchState(EMovementType state)
    {
        if (m_MovementType == state) return;

        OnExitState();
        m_MovementType = state;
        OnEnterState();

    }
    private void FixedUpdate()
    {
        if ((EMovementType)IntMovementType == EMovementType.Dead) return;
        FixedUpdateState(); 
    }

    void FixedUpdateState()
    {
        switch (m_MovementType)
        {
            case EMovementType.Idle:
                break;
            case EMovementType.Patrol:
                PatrolMovementUpdate(); 
                break;
            case EMovementType.Follow:
                break;
            case EMovementType.Flee:
                break;
            case EMovementType.Attack:
                break;
            case EMovementType.Dead:
                break;
        }
    }
    void UpdateState()
    {

        switch (m_MovementType)
        {
            case EMovementType.Idle:
                IdleUpdate();
                break;
            case EMovementType.Patrol:
                PatrolingUpdate(); 
    
                break;
            case EMovementType.Follow:
                FollowUpdate();
                
                break;
            case EMovementType.Flee:
                break;
            case EMovementType.Attack:
                AttackUpdate();
                
                break;
            case EMovementType.Dead:
                break;
        }
    }

    void OnExitState()
    {
        switch (m_MovementType)
        {
            case EMovementType.Idle:
                break;
            case EMovementType.Patrol:
                m_PatrolTimer = 0f;
                break;
            case EMovementType.Follow:
                break;
            case EMovementType.Flee:
                break;
            case EMovementType.Attack:
                break;
            case EMovementType.Dead:
                break;
        }
    }

    void OnEnterState()
    {
        switch (m_MovementType)
        {
            case EMovementType.Idle:
                break;
            case EMovementType.Patrol:
                m_PatrolTimer = 0f;
                GetNewPatrolPosition(); 
                break;
            case EMovementType.Follow:
                break;
            case EMovementType.Flee:
                break;
            case EMovementType.Attack:
             
                break;
            case EMovementType.Dead:
                m_Animator.SetBool("Dead", true);
                GetComponent<Collider>().enabled = false;
                GetComponent<Rigidbody>().isKinematic = true;
                m_Movement.enabled = false; 
                break;
        }
    }

    void PatrolingUpdate()
    {

        OnEnemyDetect(); 

        if (m_IsPatrolWaiting)
        {
            m_PatrolTimer += Time.deltaTime;
            if (m_PatrolTimer >= m_PatrolWaitTime)
            {
                //Debug.Log("finding position to go "); 
                m_IsPatrolWaiting = false;
                m_PatrolTimer = 0f;
            }
        }
        else
        {
            if(Vector3.Distance(transform.position, m_NextPatrolPosition) <= m_StopDistance)
            {
                m_IsPatrolWaiting = true;
                m_Movement.StopMovement();
                
                GetNewPatrolPosition(); 
            }
        }
        
    }
    void IdleUpdate()
    {
        OnEnemyDetect(); 

        if(!m_IsFreeRoam
            && Vector3.Distance(transform.position, m_StartPosition) >= m_StopDistance)
        {
            m_Movement.MoveOnPosition(m_StartPosition);
            return; 
        }

        if (m_Movement.MoveInput.magnitude > 0.1f) m_Movement.StopMovement(); 
    }

    void PatrolMovementUpdate()
    {
        if (m_IsPatrolWaiting) return; 
     
        m_Movement.MoveOnPosition(m_NextPatrolPosition);

        
    }

    void FollowUpdate()
    {

        if (m_MasterTransform == null)
        {
            SwitchState(m_IsPatrolUnit ? EMovementType.Patrol : EMovementType.Idle);
            return; 
        }


        if (Vector3.Distance(transform.position, m_MasterTransform.position) <= m_StopDistance *2 )
        {
            m_Movement.StopMovement();
        }
        else
        {
            FollowTarget(m_MasterTransform);
        }
    }

    protected virtual void AttackUpdate()
    {
        AfterAggroCheck();

        if (m_FollowTarget == null) return;

        if (m_FollowTarget.TryGetComponent<TinyPlayer>(out TinyPlayer player))
        {
            if(player.m_IntPlayerState != 0)
            {
                m_ClosestTarget = null;
                m_FollowTarget = null;
                m_AggroTimer = 0f;

                m_AfterAggroStanding = true;

                return; 
                
            }
        }
        

        if (Vector3.Distance(transform.position, m_FollowTarget.position) <= m_StopDistance)
        {
            //attack
            m_Movement.StopMovement();
            //OnAttack(); 
        }
        else
        {
            FollowTarget(m_FollowTarget);
            if(m_ClosestTarget == null)
            {
                m_AggroTimer += Time.deltaTime;
                if(m_AggroTimer >= m_AggroTime)
                {

                    m_FollowTarget = null;
                    m_AggroTimer = 0f;

                    m_AfterAggroStanding = true; 

                    
                }

            }
            else
            {
                m_AggroTimer = 0f;
            }
            
        }
    }

    protected virtual void OnAttack()
    {
        Debug.Log("npc attacking their target"); 
    }

    void FollowTarget(Transform targetTransform)
    {
        m_Movement.MoveOnTarget(targetTransform);
    }

    void GetNewPatrolPosition()
    {
        m_PreviousPatrolPosition = m_NextPatrolPosition;
        m_NextPatrolPosition = Random.insideUnitSphere * m_PatrolRadius;
        Vector3 centerPos = m_IsFreeRoam ? transform.position : m_StartPosition;
        m_NextPatrolPosition += centerPos;
        m_NextPatrolPosition.y = m_IsFreeRoam ? transform.position.y : m_StartPosition.y;
    }

    void EnemyDetection()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, m_DetectionRadius, m_DetectionLayer);
        
        var closestEnemy = Utils.FindClosestCollider(transform.position, enemies,m_MasterTransform);
        if(closestEnemy == null)
        {
            m_ClosestTarget = null;

        }
        else
        {
            m_ClosestTarget = closestEnemy.transform;
            m_FollowTarget = closestEnemy.transform;
        }

    }

    void OnEnemyDetect()
    {
        if(m_FollowTarget == null) return;

        m_AfterAggroStanding = false;
        m_AfterAggroStandingTimer = 0f;

        switch (m_NPCBehavior)
        {
            case ENPCBehavior.Aggressive:
                SwitchState(EMovementType.Attack);
                break;
            case ENPCBehavior.Neutral:
                //nothing
                break;
            case ENPCBehavior.Friendly:
                //wave ?
                break;
            default:
                break;
        }


    }

    void AfterAggroCheck()
    {

        if (m_AfterAggroStanding)
        {

            if (m_Movement.MoveInput.magnitude > 0.1f) m_Movement.StopMovement();

            m_AfterAggroStandingTimer += Time.deltaTime;

            if (m_AfterAggroStandingTimer >= m_AfterAggroStandingTime)
            {
                m_AfterAggroStandingTimer = 0f;
                m_AfterAggroStanding = false;
                SwitchState(m_IsPatrolUnit ? EMovementType.Patrol : EMovementType.Idle);
            }

            OnEnemyDetect();

            return;
        }
    }
    public override void EntityDeath()
    {
        //
        Debug.Log("NPC death");
        SwitchState(EMovementType.Dead);

    }

    void HitStun()
    {
        StartCoroutine(TimedStun(0.6f));
    }

    public override void Stun()
    {

        IsStunned = true;
        m_Movement.Stun();

    }

    void UnStun()
    {
        IsStunned = false;
        m_Movement.UnStun();

    }

    IEnumerator TimedStun(float time)
    {
        Stun();
        yield return new WaitForSeconds(time);
        UnStun();
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Vector3 pos = m_IsFreeRoam ? transform.position : m_StartPosition;  
        Gizmos.DrawWireSphere(pos,m_PatrolRadius);

        //detexction

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_DetectionRadius);

        if(m_ClosestTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, m_ClosestTarget.position);
            Gizmos.DrawWireCube(m_ClosestTarget.position, Vector3.one); 
        }
    }

    

    public override void TakeMeleeSync(int DirectionNESO, CoherenceSync sync, int damage, Vector3 attackerPos)
    {
        Debug.Log("not implemented TakeMeleeSync");
    }

    public override void TakeWeaponDamageSync(int damage, CoherenceSync Damagersync)
    {
        Debug.Log("not implemented TakeWeaponDamageSync");
    }

    public override void ParrySync(int damage, CoherenceSync DamagerSync)
    {
        Debug.Log("not implemented ParrySync");
    }

    public override void TakeDamageSync(int damage, CoherenceSync Damagersync)
    {
        Debug.Log("simulator not found, game is not hosted");

        HitStun();

        Health -= damage;

        if (Health <= 0)
        {
            Health = 0;
            Debug.Log("must die now");

            EntityDeath();
        }
        else
        {
            Debug.Log("sending weapon synchit comand ");

        }
    }

    public override void SyncBlocked()
    {
        Debug.Log("not implemented SyncBlocked");
    }

    public override void SyncHit()
    {
        Debug.Log("not implemented SyncHit");
    }
}
