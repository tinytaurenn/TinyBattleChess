using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace PlayerControls
{
    public class PlayerControls : MonoBehaviour
    {
        public enum ECrontrolState
        {
            Player,
            Ghost,
            Selecting,

        }
        [SerializeField] ECrontrolState m_ControlState = ECrontrolState.Player;

        InputSystem_Actions m_InputActions;

        //
        PlayerMovement m_PlayerMovement;
        PlayerGhostMovement m_PlayerGhostMovement;
        PlayerWeapons m_PlayerWeapons;
        TinyPlayer m_TinyPlayer; 
        PlayerUse m_PlayerUse;
        Vector2 m_MoveValue;
        Transform m_CameraTransform;
        CameraManager m_CameraManager; 
        PlayerLoadout m_PlayerLoadout;

        private void OnEnable()
        {
            //m_InputActions.Enable();
            m_InputActions.Player.Enable();

            m_InputActions.Player.Look.performed += CameraManager.Instance.LookUpdate;
            m_InputActions.Ghost.Look.performed += CameraManager.Instance.LookUpdate;
           
            m_InputActions.Player.Jump.performed += Jump;
            m_InputActions.Player.Jump.canceled += CancelJump;

            m_InputActions.Player.Interact.performed += Use;
            m_InputActions.Player.Drop.performed += Drop;
            m_InputActions.Player.Slot1.performed += Slot1Action; 
            m_InputActions.Player.Slot2.performed += Slot2Action;
            m_InputActions.Player.Slot3.performed += Slot3Action;
            m_InputActions.Player.Slot4.performed += Slot4Action;

            m_InputActions.Ghost.Interact.performed += GhostUse;

          
          



            //m_MouseRightClick.action.performed += m_PlayerWeapons.Parry;
        }

        

        private void OnDisable()
        {
            m_InputActions.Player.Disable();

            m_InputActions.Player.Look.performed -= CameraManager.Instance.LookUpdate;
            m_InputActions.Ghost.Look.performed -= CameraManager.Instance.LookUpdate;

            m_InputActions.Player.Jump.performed -= Jump;
            m_InputActions.Player.Jump.canceled -= CancelJump;

            m_InputActions.Player.Interact.performed -= Use;
            m_InputActions.Player.Drop.performed -= Drop;
            m_InputActions.Player.Slot1.performed -= Slot1Action;
            m_InputActions.Player.Slot2.performed -= Slot2Action;
            m_InputActions.Player.Slot3.performed -= Slot3Action;
            m_InputActions.Player.Slot4.performed -= Slot4Action;

            m_InputActions.Ghost.Interact.performed -= GhostUse;
            




            //m_MouseRightClick.action.performed -= m_PlayerWeapons.Parry;
        }

        

        private void Awake()
        { 
            m_PlayerMovement = GetComponent<PlayerMovement>();
            m_PlayerGhostMovement = GetComponent<PlayerGhostMovement>();
            m_PlayerUse = GetComponent<PlayerUse>();
            m_PlayerWeapons = GetComponent<PlayerWeapons>();
            m_TinyPlayer = GetComponent<TinyPlayer>();
            m_PlayerLoadout = GetComponent<PlayerLoadout>();

            m_InputActions = new InputSystem_Actions();

        }

        void Start()
        {
            OnEnterState(); 
        }

        // Update is called once per frame
        void Update()
        {
            

            ControlStateUpdate(); 
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

           
            switch (m_ControlState)
            {
                case ECrontrolState.Player:
                    m_PlayerMovement.MoveInput = right * m_MoveValue.x + forward * m_MoveValue.y;
                    break;
                case ECrontrolState.Ghost:
                    m_PlayerGhostMovement.MoveInput = right * m_MoveValue.x + forward * m_MoveValue.y;
                    m_PlayerGhostMovement.VerticalInput = new Vector2((m_InputActions.Ghost.Up.IsPressed() ? 1 : 0), (m_InputActions.Ghost.Down.IsPressed() ? 1 : 0));
                    break;
                case ECrontrolState.Selecting:
                    break;
                default:
                    break;
            }

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

        private void GhostUse(InputAction.CallbackContext context)
        {

            Debug.Log("Ghost Use");
        }
        private void Drop(InputAction.CallbackContext context)
        {
            m_PlayerUse.DropPerformed(); 
        }

        private void Slot1Action(InputAction.CallbackContext context)
        {
            m_PlayerLoadout.SlotActionPerformed(PlayerLoadout.ESlot.Slot_1); 
        }
        private void Slot4Action(InputAction.CallbackContext context)
        {
            m_PlayerLoadout.SlotActionPerformed(PlayerLoadout.ESlot.Slot_2);
        }

        private void Slot3Action(InputAction.CallbackContext context)
        {
            m_PlayerLoadout.SlotActionPerformed(PlayerLoadout.ESlot.Slot_3);
        }

        private void Slot2Action(InputAction.CallbackContext context)
        {
            m_PlayerLoadout.SlotActionPerformed(PlayerLoadout.ESlot.Slot_4);
        }

        void ControlStateUpdate()
        {
            switch (m_ControlState)
            {
                case ECrontrolState.Player:
                    SetMovementValue(m_InputActions.Player.Move.ReadValue<Vector2>());
                    SetIsSprinting(m_InputActions.Player.Sprint.IsPressed());
                    m_PlayerWeapons.m_LookDirection = m_InputActions.Player.Look.ReadValue<Vector2>();
                    m_PlayerWeapons.m_Parrying = m_InputActions.Player.Parry.IsPressed();
                    m_PlayerWeapons.m_Attacking = m_InputActions.Player.Attack.IsPressed();
                    break;
                case ECrontrolState.Ghost:
                    SetMovementValue(m_InputActions.Ghost.Move.ReadValue<Vector2>());
                   
                    

                    break;
                case ECrontrolState.Selecting:
                    break;
                default:
                    break;
            }
        }

        internal void SwitchState(ECrontrolState state)
        {
            if(m_ControlState == state) return;

            OnExitState();
            m_ControlState = state;
            OnEnterState();

        }

        private void OnEnterState()
        {
            switch (m_ControlState)
            {
                case ECrontrolState.Player:
                    m_InputActions.Player.Enable();
                    Cursor.visible = false;
                    break;
                case ECrontrolState.Ghost:
                    m_InputActions.Ghost.Enable();
                    Cursor.visible = false;

                    break;
                case ECrontrolState.Selecting:
                    m_InputActions.PowerSelect.Enable();
                    CameraManager.Instance.MouseDelta = Vector2.zero;
                    Cursor.visible = true;
                    break;
                default:
                    break;
            }
        }

        private void OnExitState()
        {
            switch (m_ControlState)
            {
                case ECrontrolState.Player:
                    m_InputActions.Player.Disable();
                    break;
                case ECrontrolState.Ghost:
                    m_InputActions.Ghost.Disable();
                    break;
                case ECrontrolState.Selecting:
                    m_InputActions.PowerSelect.Disable();
                    break;
                default:
                    break;
            }
        }
    } 
}
