using Coherence.Toolkit;
using UnityEngine;


public class TinyNPC : MonoBehaviour, IDamageable
{
    public enum EMovementType
    {
        Idle,
        Patrol,
        Follow,
        Flee,
        Attack
    }

    [SerializeField] EMovementType m_MovementType = EMovementType.Idle;
    public EMovementType MovementType => m_MovementType;

    NPC_Movement m_Movement;

    [SerializeField] Transform m_Target;
    [Space(10)]
    [Header("Global")]
    [SerializeField] float m_StopDistance = 1f;
    [SerializeField] Vector3 m_StartPosition;

    [Space(10)]
    [Header("Patrol")]
    [SerializeField] bool m_IsPatrolWaiting = false;
    [SerializeField] bool m_IsFreeRoam = true;
    [SerializeField] float m_PatrolRadius = 10f;
    [SerializeField] Vector3 m_NextPatrolPosition;
    [SerializeField] Vector3 m_PreviousPatrolPosition;
    [SerializeField] float m_PatrolTimer = 0f;
    [SerializeField] float m_PatrolWaitTime = 5f;

    void Awake()
    {
        m_Movement = GetComponent<NPC_Movement>();
    }
    void Start()
    {
        m_StartPosition = transform.position;
        GetNewPatrolPosition();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState(); 
        
    }

    public void SwitchState(EMovementType state)
    {
        if (MovementType == state) return;

        OnExitState();
        m_MovementType = state;
        OnEnterState();

    }
    private void FixedUpdate()
    {
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
        }
    }
    void UpdateState()
    {
        switch (m_MovementType)
        {
            case EMovementType.Idle:
                break;
            case EMovementType.Patrol:
                PatrolingUpdate(); 
    
                break;
            case EMovementType.Follow:
                break;
            case EMovementType.Flee:
                break;
            case EMovementType.Attack:
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
                break;
            case EMovementType.Follow:
                break;
            case EMovementType.Flee:
                break;
            case EMovementType.Attack:
                break;
        }
    }

    void PatrolingUpdate()
    {
        if (m_IsPatrolWaiting)
        {
            m_PatrolTimer += Time.deltaTime;
            if (m_PatrolTimer >= m_PatrolWaitTime)
            {
                Debug.Log("finding position to go "); 
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

    void PatrolMovementUpdate()
    {
        if (m_IsPatrolWaiting) return; 
     
        m_Movement.MoveOnPosition(m_NextPatrolPosition);
    }

    void GetNewPatrolPosition()
    {
        m_PreviousPatrolPosition = m_NextPatrolPosition;
        m_NextPatrolPosition = Random.insideUnitSphere * m_PatrolRadius;
        Vector3 centerPos = m_IsFreeRoam ? transform.position : m_StartPosition;
        m_NextPatrolPosition += centerPos;
        m_NextPatrolPosition.y = m_IsFreeRoam ? transform.position.y : m_StartPosition.y;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Vector3 pos = m_IsFreeRoam ? transform.position : m_StartPosition;  
        Gizmos.DrawWireSphere(pos,m_PatrolRadius);
    }

    public void TakeMeleeSync(int DirectionNESO, CoherenceSync sync, int damage, Vector3 attackerPos)
    {
        throw new System.NotImplementedException();
    }

    public void TakeWeaponDamageSync(int damage, CoherenceSync Damagersync)
    {
        throw new System.NotImplementedException();
    }

    public void ParrySync(int damage, CoherenceSync DamagerSync)
    {
        throw new System.NotImplementedException();
    }
}
