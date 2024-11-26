using UnityEngine;
using UnityEngine.InputSystem;


namespace PlayerControls
{
    public class PlayerControls : MonoBehaviour
    {
        [SerializeField] InputActionReference m_MovementInputAction;
        [SerializeField] InputActionReference m_JumpAction;
        [SerializeField] InputActionReference m_SprintAction;
        [SerializeField] InputActionReference m_UseAction;

        //
        PlayerMovement m_PlayerMovement;
        PlayerUse m_PlayerUse;
        Vector2 m_MoveValue;
        Transform m_CameraTransform; 

        private void OnEnable()
        {
            m_MovementInputAction.asset.Enable();
            m_JumpAction.asset.Enable();
            m_SprintAction.asset.Enable();
            m_UseAction.asset.Enable();


            m_JumpAction.action.performed += Jump;
            m_JumpAction.action.canceled += CancelJump;

            m_UseAction.action.performed += Use; 
        }

        private void OnDisable()
        {
            m_MovementInputAction.asset.Disable();
            m_JumpAction.asset.Disable();
            m_SprintAction.asset.Disable();
            m_UseAction.asset.Disable();

            m_JumpAction.action.performed -= Jump;
            m_JumpAction.action.canceled -= CancelJump;

            m_UseAction.action.performed -= Use;
        }

        private void Awake()
        {
            m_PlayerMovement = GetComponent<PlayerMovement>();
            m_PlayerUse = GetComponent<PlayerUse>();

        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            SetMovementValue(m_MovementInputAction.action.ReadValue<Vector2>());
            SetIsSprinting(m_SprintAction.action.IsPressed());
        }

        void SetMovementValue(Vector2 moveInput)
        {
            if(m_CameraTransform == null) m_CameraTransform = Camera.main.transform;
            m_MoveValue = moveInput;
            Vector3 forward = new Vector3(m_CameraTransform.forward.x, 0, m_CameraTransform.forward.z).normalized;
            Vector3 right = new Vector3(m_CameraTransform.right.x, 0, m_CameraTransform.right.z).normalized;
            m_PlayerMovement.MoveInput = right * m_MoveValue.x + forward * m_MoveValue.y;
        }
        void SetIsSprinting(bool isSprinting)
        {
            m_PlayerMovement.IsSprinting = isSprinting;
        }


        private void CancelJump(InputAction.CallbackContext context)
        {

            m_PlayerMovement.InterrupJump();
        }

        private void Jump(InputAction.CallbackContext context)
        {
            m_PlayerMovement.TryJump();
        }

        private void Use(InputAction.CallbackContext context)
        {
           
            m_PlayerUse.UsePerformed();
        }
    } 
}
