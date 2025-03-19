using PlayerControls;
using UnityEngine;

public class ParentedCamera : MonoBehaviour
{
    public static ParentedCamera Instance;

    [Space(10)]
    [Header("Cam params")]
    //free cam params
    public Transform m_PlayerTransform;
    [SerializeField] Vector3 m_TargetOffset;

    [SerializeField] float m_RotationSpeed = 10f;
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

        if (transform.parent != m_PlayerTransform)
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

    void LerpCameraDistance()
    {

        Vector3 targetPostion = m_PlayerTransform.position;
        Vector3 cameraPos = transform.position;
        Vector3 direction = m_PlayerTransform.forward;

        // Normalize the direction vector
        direction.Normalize();

        Vector3 newPosition = targetPostion - direction * m_CameraDistance;
        newPosition = newPosition + (Vector3.up * m_CameraUpOffset);


        transform.position = newPosition;
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
        m_LastGroundedYPos = m_PlayerTransform.position.y;

        targetLookAtPosition = m_PlayerTransform.position + localTargetOffset;

        Quaternion targetRotation = Quaternion.LookRotation(targetLookAtPosition - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, m_RotationSpeed * Time.fixedDeltaTime); 
    }


    private void OnDrawGizmos()
    {

    }

    #endregion
}
