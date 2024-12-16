using System;
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
        [SerializeField] InputActionReference m_MouseLeftClick;
        [SerializeField] InputActionReference m_MouseRightClick;
        [SerializeField] InputActionReference m_DropAction;
        [SerializeField] InputActionReference m_MouseLookAction;
       

        //
        PlayerMovement m_PlayerMovement;
        PlayerWeapons m_PlayerWeapons; 
        PlayerUse m_PlayerUse;
        Vector2 m_MoveValue;
        Transform m_CameraTransform;
        CameraManager m_CameraManager; 

        private void OnEnable()
        {
            m_MovementInputAction.asset.Enable();
            m_JumpAction.asset.Enable();
            m_SprintAction.asset.Enable();
            m_UseAction.asset.Enable();
            m_MouseLeftClick.asset.Enable();
            m_MouseRightClick.asset.Enable();
            m_DropAction.asset.Enable();
            m_MouseLookAction.asset.Enable();





            m_JumpAction.action.performed += Jump;
            m_JumpAction.action.canceled += CancelJump;

            m_UseAction.action.performed += Use;
            m_DropAction.action.performed += Drop;

            //m_MouseRightClick.action.performed += m_PlayerWeapons.Parry;
        }

        private void OnDisable()
        {
            m_MovementInputAction.asset.Disable();
            m_JumpAction.asset.Disable();
            m_SprintAction.asset.Disable();
            m_UseAction.asset.Disable();
            m_MouseLeftClick.asset.Disable();
            m_MouseRightClick.asset.Disable();
            m_DropAction.asset.Disable();
            m_MouseLookAction.asset.Disable();




            m_JumpAction.action.performed -= Jump;
            m_JumpAction.action.canceled -= CancelJump;

            m_UseAction.action.performed -= Use;
            m_DropAction.action.performed -= Drop;

            //m_MouseRightClick.action.performed -= m_PlayerWeapons.Parry;
        }

        

        private void Awake()
        {
            m_PlayerMovement = GetComponent<PlayerMovement>();
            m_PlayerUse = GetComponent<PlayerUse>();
            m_PlayerWeapons = GetComponent<PlayerWeapons>();

        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            SetMovementValue(m_MovementInputAction.action.ReadValue<Vector2>());
            SetIsSprinting(m_SprintAction.action.IsPressed());
            m_PlayerWeapons.m_LookDirection = m_MouseLookAction.action.ReadValue<Vector2>();
            m_PlayerWeapons.m_Parrying = m_MouseRightClick.action.IsPressed();
            m_PlayerWeapons.m_Attacking = m_MouseLeftClick.action.IsPressed();
        }

        void SetMovementValue(Vector2 moveInput)
        {
            if (m_CameraTransform == null)
            {
                m_CameraManager = CameraManager.Instance; 
                m_CameraTransform = m_CameraManager.transform;
            }
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
        private void Drop(InputAction.CallbackContext context)
        {
            m_PlayerUse.DropPerformed(); 
        }
    } 
}
