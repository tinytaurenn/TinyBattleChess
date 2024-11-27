using System;
using TMPro;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform m_PlayerTransform;

    public static CameraManager Instance;
    [SerializeField] float m_FollowSpeed = 10f;
    [SerializeField] float m_UpFollowSpeed = 10f;
    [SerializeField] float m_RotationSpeed = 10f;
    [SerializeField] Vector3 m_TargetOffset;

    [SerializeField] float m_CameraDistance = 10f;
    [SerializeField] float m_CameraUpOffset = 3f;
    [SerializeField] float m_CameraRotateSpeed = 7f; 
    

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
        // Check if the right mouse button is pressed
        if (Input.GetMouseButton(1))
        {
            // Get the mouse movement delta
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            Vector3 targetPostion = m_PlayerTransform.position; 

            // Rotate the camera around the target based on mouse movement
            
            
            transform.RotateAround(targetPostion, Vector3.up, mouseX * m_CameraRotateSpeed);



          

        }

        Vector3 localTargetOffset = m_PlayerTransform.forward * m_TargetOffset.z + m_PlayerTransform.right * m_TargetOffset.x + m_PlayerTransform.up * m_TargetOffset.y;

        Vector3 targetLookAtPosition = m_PlayerTransform.position + localTargetOffset;
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

    #endregion
}
