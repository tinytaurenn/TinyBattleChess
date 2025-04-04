using PlayerControls;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{

    public static CameraManager Instance;


    public Transform m_PlayerTransform;
    //[SerializeField] InputActionReference m_MouseCamInputRef;
    


    [Space(10)]
    [Header("Free Cam params")]
    //free cam params

    [SerializeField] float m_FollowSpeed = 10f;
    [SerializeField] float m_UpFollowSpeed = 10f;
    [SerializeField] float m_RotationSpeed = 10f;
    [SerializeField] Vector3 m_TargetOffset;

    [SerializeField] float m_CameraDistance = 10f;
    [SerializeField] float m_CameraUpOffset = 3f;
    [SerializeField] float m_FallingDownOffSet = 3f;
    [SerializeField] float m_LastGroundedYPos = 0f;

   



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

    }

    private void OnEnable()
    {
        //m_MouseCamInputRef.action.Enable();

    }
    private void OnDisable()
    {
        //m_MouseCamInputRef.action.Disable();

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_PlayerTransform == null) return;

        if(transform.parent != m_PlayerTransform)
        {

           transform.SetParent(m_PlayerTransform);
        }


    }

    private void FixedUpdate()
    {

        if (m_PlayerTransform == null) return;
        //SimpleFollowCamera(); 
        ThirdPersoMouseControlCamera();

    }

    #region CameraFunctions

    void ThirdPersoMouseControlCamera()
    {
        //MouseControlCamera();
        LookAtPlayer(); 
        LerpCameraDistance();


    }

    void LookAtPlayer()
    {
        Vector3 targetPostion = m_PlayerTransform.position;

        Vector3 targetLookAtPosition;
        PlayerMovement playerMove = m_PlayerTransform.GetComponent<PlayerMovement>();
        Vector3 newPlayerPos = new Vector3(m_PlayerTransform.position.x, transform.position.y, m_PlayerTransform.position.z);
        Vector3 customForward = (newPlayerPos - transform.position).normalized;
        Vector3 customRight = Vector3.Cross(Vector3.up, customForward).normalized;

        Vector3 localTargetOffset = customForward * m_TargetOffset.z + customRight * m_TargetOffset.x + Vector3.up * m_TargetOffset.y;
        if (!playerMove.m_Isgrounded || playerMove.m_IsFalling)
        {
            if(m_LastGroundedYPos < m_PlayerTransform.position.y + m_FallingDownOffSet)
            {
                Vector3 lastGroundedPlayerPos = new Vector3(m_PlayerTransform.position.x, m_LastGroundedYPos, m_PlayerTransform.position.z);
                targetLookAtPosition = lastGroundedPlayerPos + localTargetOffset;
                
            }
            else
            {
                targetLookAtPosition = m_PlayerTransform.position + localTargetOffset + Vector3.down * m_FallingDownOffSet;
            }
            
        }
        else
        {
            m_LastGroundedYPos = m_PlayerTransform.position.y;

            targetLookAtPosition = m_PlayerTransform.position + localTargetOffset;

        }

        Quaternion targetRotation = Quaternion.LookRotation(targetLookAtPosition - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, m_RotationSpeed * Time.fixedDeltaTime);
    }


    void LerpCameraDistance2()
    {

        Vector3 targetPostion = m_PlayerTransform.position;
        Vector3 cameraPos = transform.position;
        Vector3 direction = new Vector3(targetPostion.x, cameraPos.y, targetPostion.z) - cameraPos;

        // Normalize the direction vector
        direction.Normalize();

        Vector3 newPosition = targetPostion - direction * m_CameraDistance;
        newPosition = newPosition + (Vector3.up * m_CameraUpOffset);

        float xPositionLerp = Mathf.Lerp(transform.position.x, newPosition.x, m_FollowSpeed * Time.fixedDeltaTime);
        float yPositionLerp = Mathf.Lerp(transform.position.y, newPosition.y, m_UpFollowSpeed * Time.fixedDeltaTime);
        float zPositionLerp = Mathf.Lerp(transform.position.z, newPosition.z, m_FollowSpeed * Time.fixedDeltaTime);

        transform.position = new Vector3(xPositionLerp, yPositionLerp, zPositionLerp);
    }

    void LerpCameraDistance()
    {

        Vector3 targetPostion = m_PlayerTransform.position;
        Vector3 cameraPos = transform.position;
        Vector3 direction = m_PlayerTransform.forward; 

        // Normalize the direction vector
        direction.Normalize();

        Vector3 newPosition = targetPostion - direction * m_CameraDistance;
        newPosition = newPosition + (Vector3.up * m_CameraUpOffset);

        float xPositionLerp = Mathf.Lerp(transform.position.x, newPosition.x, m_FollowSpeed * Time.fixedDeltaTime);
        float yPositionLerp = Mathf.Lerp(transform.position.y, newPosition.y, m_UpFollowSpeed * Time.fixedDeltaTime);
        float zPositionLerp = Mathf.Lerp(transform.position.z, newPosition.z, m_FollowSpeed * Time.fixedDeltaTime);

        transform.position = new Vector3(xPositionLerp, yPositionLerp, zPositionLerp);
    }


    private void OnDrawGizmos()
    {
        if(m_PlayerTransform == null) return;
        Gizmos.color = Color.blue;
        Vector3 newPlayerPos = new Vector3(m_PlayerTransform.position.x, transform.position.y, m_PlayerTransform.position.z);
        Vector3 customForward = (newPlayerPos - transform.position).normalized;
        Gizmos.DrawRay(transform.position, customForward * 2);
        Gizmos.color = Color.red;
        Vector3 customRight = Vector3.Cross(Vector3.up, customForward).normalized;
        Gizmos.DrawRay(transform.position, customRight * 2);


        Vector3 localTargetOffset = customForward * m_TargetOffset.z + customRight * m_TargetOffset.x + Vector3.up * m_TargetOffset.y;
        Gizmos.DrawWireSphere(m_PlayerTransform.position + localTargetOffset, 2); 
    }

    #endregion

}
