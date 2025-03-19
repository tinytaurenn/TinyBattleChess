using PlayerControls;
using UnityEngine;

public class ParentedCamera : MonoBehaviour
{
    public static ParentedCamera Instance;

    [Space(10)]
    [Header("Cam params")]
    //free cam params
    public PlayerMovement m_PlayerMovement;
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

        if (m_PlayerMovement == null) return;

        if (transform.parent != m_PlayerMovement)
        {

            transform.SetParent(m_PlayerMovement.transform);
        }

    }

    private void FixedUpdate()
    {
        if (m_PlayerMovement == null) return;
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

        Vector3 targetPostion = m_PlayerMovement.transform.position;
        Vector3 cameraPos = transform.position;
        Vector3 direction = m_PlayerMovement.transform.forward;

        // Normalize the direction vector
        direction.Normalize();

        Vector3 newPosition = targetPostion - direction * m_CameraDistance;
        newPosition = newPosition + (Vector3.up * m_CameraUpOffset);

        //newPosition += Vector3.down *( ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerMovement.LookValue*3); 


        transform.position = newPosition;
    }

    void LookAtPlayer()
    {
        Vector3 targetPostion = m_PlayerMovement.transform.position;

        Vector3 targetLookAtPosition;
        PlayerMovement playerMove = m_PlayerMovement.GetComponent<PlayerMovement>();
        Vector3 newPlayerPos = new Vector3(m_PlayerMovement.transform.position.x, transform.position.y, m_PlayerMovement.transform.position.z);
        Vector3 customForward = (newPlayerPos - transform.position).normalized;
        Vector3 customRight = Vector3.Cross(Vector3.up, customForward).normalized;

        Vector3 localTargetOffset = customForward * m_TargetOffset.z + customRight * m_TargetOffset.x + Vector3.up * m_TargetOffset.y;
        m_LastGroundedYPos = m_PlayerMovement.transform.position.y;

        targetLookAtPosition = m_PlayerMovement.transform.position + localTargetOffset + (Vector3.up * m_PlayerMovement.LookValue);

        Quaternion targetRotation = Quaternion.LookRotation(targetLookAtPosition - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, m_RotationSpeed * Time.fixedDeltaTime); 
    }


    private void OnDrawGizmos()
    {

    }

    #endregion
}
