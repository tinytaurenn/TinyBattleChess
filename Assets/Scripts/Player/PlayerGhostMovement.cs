using System;
using UnityEngine;
using UnityEngine.XR;

public class PlayerGhostMovement : MonoBehaviour
{

    Rigidbody m_rigidBody;
    public Vector3 MoveInput { get; set; }
    public Vector2 VerticalInput { get; set; }
    [SerializeField] Vector3 m_PreviousMoveInput; 
    public float m_Speed = 5.0f;
    public float m_VerticalSpeed = 5.0f;
    public float m_SmoothingValue = 0.1f;
    public float m_RotationSpeed;

    public float m_Acceleration = 2f;
    public float m_Deceleration = 2f; 

    [SerializeField] CameraManager m_CameraManager;

    [SerializeField] Vector3 m_Velocity;
    [SerializeField] Vector3 m_PreviousVelocity;

    Vector3 m_HorizontalVelocity;

    [SerializeField]float  m_Magnitude; 
    [SerializeField]float  m_PreviousMagnitude;


    public Vector2 MouseDelta { get; set; }
    void Start()
    {
        
    }
    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_CameraManager = CameraManager.Instance;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        MovementUpdate();
    }

    private void MovementUpdate()
    {
        HorizontalMovementUpdate();
        VerticalMovementUpdate();
    }

    void VerticalMovementUpdate()
    {
        if (VerticalInput.x == VerticalInput.y) return; 

        if(VerticalInput.x == 1)
        {
            //transform.position += Vector3.up * Time.deltaTime * m_Speed;


            transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up  * m_VerticalSpeed, Time.deltaTime * m_SmoothingValue);
        }

        if (VerticalInput.y == 1)
        {
            //transform.position -= Vector3.up * Time.deltaTime * m_Speed;

            transform.position = Vector3.Lerp(transform.position, transform.position - Vector3.up * m_VerticalSpeed, Time.deltaTime * m_SmoothingValue);
        }
    }

    private void HorizontalMovementUpdate()
    {
        float magnitude = MoveInput.magnitude;
        float acceleration = 0f;
        if (magnitude > m_PreviousMagnitude)
        {
            acceleration = m_Acceleration;
        }
        else
        {
            acceleration = m_Deceleration;
        }


        Vector3 vectorDelta = Vector3.Lerp(m_PreviousMoveInput, MoveInput, Time.deltaTime * acceleration);
        float magnitudeDelta = vectorDelta.magnitude;



        m_HorizontalVelocity = (vectorDelta) * m_Speed; 

        m_PreviousMoveInput = vectorDelta;
        m_PreviousMagnitude = magnitudeDelta;

        m_rigidBody.linearVelocity = m_HorizontalVelocity;

        float mouseDeltaX = MouseDelta.x;

        Quaternion newRotation = Quaternion.Euler(0, mouseDeltaX, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation * newRotation, Time.fixedDeltaTime * m_RotationSpeed);
    }

    public void StopMovement()
    {
        MouseDelta = Vector2.zero;
        MoveInput = Vector3.zero;
        m_HorizontalVelocity = Vector3.zero;
        m_rigidBody.linearVelocity = Vector3.zero;
    }
}
