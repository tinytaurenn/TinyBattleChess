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
    
    public Vector2 MouseDelta { get; set; }


    [Space(10)]
    [Header("Free Cam params")]
    //free cam params

    [SerializeField] float m_FollowSpeed = 10f;
    [SerializeField] float m_UpFollowSpeed = 10f;
    [SerializeField] float m_RotationSpeed = 10f;
    [SerializeField] Vector3 m_TargetOffset;

    [SerializeField] float m_CameraDistance = 10f;
    [SerializeField] float m_CameraUpOffset = 3f;
    [SerializeField] float m_CameraRotateSpeed = 7f;
    [SerializeField] float m_FallingDownOffSet = 3f;

   



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
        MouseControlCamera();
        LerpCameraDistance();


    }

    void MouseControlCamera()
    {
        float mouseDeltaX = MouseDelta.x;
        //float mouseDeltaY = m_MouseCamInputRef.action.ReadValue<Vector2>().y; 



        Vector3 targetPostion = m_PlayerTransform.position;


        transform.RotateAround(targetPostion, Vector3.up, mouseDeltaX * m_CameraRotateSpeed);
        //transform.RotateAround(targetPostion, transform.right, -mouseDeltaY * m_CameraRotateSpeed);

        Vector3 localTargetOffset = m_PlayerTransform.forward * m_TargetOffset.z + m_PlayerTransform.right * m_TargetOffset.x + m_PlayerTransform.up * m_TargetOffset.y;
        Vector3 targetLookAtPosition;
        PlayerMovement playerMove = m_PlayerTransform.GetComponent<PlayerMovement>();
        if (!playerMove.m_Isgrounded || playerMove.m_IsFalling)
        {
            targetLookAtPosition = m_PlayerTransform.position + localTargetOffset + Vector3.down * m_FallingDownOffSet;
        }
        else
        {


            targetLookAtPosition = m_PlayerTransform.position + localTargetOffset;
            
        }



        Quaternion targetRotation = Quaternion.LookRotation(targetLookAtPosition - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, m_RotationSpeed * Time.fixedDeltaTime);
    }

    void LerpCameraDistance()
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

    internal void LookUpdate(InputAction.CallbackContext context)
    {
        Debug.Log("LookUpdate " + context.ReadValue<Vector2>().ToString());
        MouseDelta = context.ReadValue<Vector2>();
    }

    #endregion

}
