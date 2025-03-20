using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace PlayerControls
{
    public class PlayerControls : MonoBehaviour
    {
        public enum EControlState
        {
            Player,
            Ghost,
            Selecting,

        }
        [SerializeField] internal EControlState m_ControlState = EControlState.Player;

        InputSystem_Actions m_InputActions;

        //
        PlayerMovement m_PlayerMovement;
        PlayerGhostMovement m_PlayerGhostMovement;
        PlayerWeapons m_PlayerWeapons;
        TinyPlayer m_TinyPlayer; 
        PlayerUse m_PlayerUse;
        Vector2 m_MoveValue;
        Transform m_CameraTransform;
        ParentedCamera m_ParentedCamera; 
        PlayerLoadout m_PlayerLoadout;

        private void OnEnable()
        {
            //m_InputActions.Enable();
            m_InputActions.Player.Enable();

           
            m_InputActions.Player.Jump.performed += Jump;
            m_InputActions.Player.Jump.canceled += CancelJump;

            m_InputActions.Player.Interact.performed += Use;
            m_InputActions.Player.Drop.performed += Drop;

            m_InputActions.Player.Slot1.performed +=  SlotAction1;
            m_InputActions.Player.Slot2.performed += SlotAction2;
            m_InputActions.Player.Slot3.performed += SlotAction3;
            m_InputActions.Player.Slot4.performed += SlotAction4;

            m_InputActions.Ghost.Interact.performed += GhostUse;

            m_InputActions.PowerSelect.Select_1.performed += ChoiceSelect1;
            m_InputActions.PowerSelect.Select_2.performed += ChoiceSelect2;
            m_InputActions.PowerSelect.Select_3.performed += ChoiceSelect3;

            m_InputActions.PowerSelect.Exit.performed +=  ExitSelectionPanel;

            m_InputActions.Player.Pause.performed += TogglePause;

            m_InputActions.Player.Emote.performed += Emote;



        }

        private void OnDisable()
        {
            m_InputActions.Player.Disable();


            m_InputActions.Player.Jump.performed -= Jump;
            m_InputActions.Player.Jump.canceled -= CancelJump;

            m_InputActions.Player.Interact.performed -= Use;
            m_InputActions.Player.Drop.performed -= Drop;

            m_InputActions.Player.Slot1.performed -= SlotAction1;
            m_InputActions.Player.Slot2.performed -= SlotAction2;
            m_InputActions.Player.Slot3.performed -= SlotAction3;
            m_InputActions.Player.Slot4.performed -= SlotAction4;

            m_InputActions.Ghost.Interact.performed -= GhostUse;

            m_InputActions.PowerSelect.Select_1.performed -= ChoiceSelect1;
            m_InputActions.PowerSelect.Select_2.performed -= ChoiceSelect2;
            m_InputActions.PowerSelect.Select_3.performed -= ChoiceSelect3;

            m_InputActions.PowerSelect.Exit.performed -= ExitSelectionPanel;

            m_InputActions.Player.Pause.performed -= TogglePause;

            m_InputActions.Player.Emote.performed -= Emote;



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
                m_ParentedCamera = ParentedCamera.Instance; 
                m_CameraTransform = m_ParentedCamera.transform;
            }
            m_MoveValue = moveInput;
            Vector3 forward = new Vector3(m_CameraTransform.forward.x, 0, m_CameraTransform.forward.z).normalized;
          
            Vector3 right = new Vector3(m_CameraTransform.right.x, 0, m_CameraTransform.right.z).normalized;

           
            switch (m_ControlState)
            {
                case EControlState.Player:
                    if(m_MoveValue.y < 0)
                    {
                        m_MoveValue.y *= m_PlayerMovement.BackWardspeedModifier;
                    }
                    m_PlayerMovement.MoveInput = right *( m_MoveValue.x  * m_PlayerMovement.StrafeSpeedModifier) + forward * m_MoveValue.y;
                    
                    break;
                case EControlState.Ghost:
                    m_PlayerGhostMovement.MoveInput = right * m_MoveValue.x + forward * m_MoveValue.y;
                    m_PlayerGhostMovement.VerticalInput = new Vector2((m_InputActions.Ghost.Up.IsPressed() ? 1 : 0), (m_InputActions.Ghost.Down.IsPressed() ? 1 : 0));
                    break;
                case EControlState.Selecting:
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

        void SlotAction1(InputAction.CallbackContext context) => m_PlayerLoadout.SlotActionPerformed(EStuffSlot.Slot_1);

        void SlotAction2(InputAction.CallbackContext context) => m_PlayerLoadout.SlotActionPerformed(EStuffSlot.Slot_2);

        void SlotAction3(InputAction.CallbackContext context) => m_PlayerLoadout.SlotActionPerformed(EStuffSlot.Slot_3);

        void SlotAction4(InputAction.CallbackContext context) => m_PlayerLoadout.SlotActionPerformed(EStuffSlot.Slot_4);

       
        void ChoiceSelect1(InputAction.CallbackContext context) => LocalUI.Instance.SelectItem(0);
        void ChoiceSelect2(InputAction.CallbackContext context) => LocalUI.Instance.SelectItem(1);
        void ChoiceSelect3(InputAction.CallbackContext context) => LocalUI.Instance.SelectItem(2);

        void Emote(InputAction.CallbackContext context)
        {
            if (m_PlayerWeapons.m_Parrying || m_PlayerWeapons.m_Attacking || m_PlayerWeapons.UsingMagic || m_PlayerWeapons.UsingItem || m_PlayerWeapons.Throwing) return; 
            m_PlayerMovement.Emote();
        }

        private void TogglePause(InputAction.CallbackContext context)
        {
            LocalUI.Instance.TogglePause();
        }


        void SelectItem(int index)
        {
            m_InputActions.PowerSelect.Exit.performed += ExitSelectionPanel;
            LocalUI.Instance.SelectItem(index);
        }

        public void ExitSelectionPanel(InputAction.CallbackContext context)
        {
            //not sure using it 
            //LocalUI.Instance.CloseSelection();
            
        }

        public void EnterReplaceInventorySlotControls()
        {
            m_InputActions.PowerSelect.Replace1.performed += ReplaceSlot1;
            m_InputActions.PowerSelect.Replace2.performed += ReplaceSlot2;
            m_InputActions.PowerSelect.Replace3.performed += ReplaceSlot3;
            m_InputActions.PowerSelect.Replace4.performed += ReplaceSlot4; 


            m_InputActions.PowerSelect.Exit.performed -= ExitSelectionPanel;


        }

        public void ExitReplaceInventorySlotControls()
        {
            m_InputActions.PowerSelect.Replace1.performed -= ReplaceSlot1;
            m_InputActions.PowerSelect.Replace2.performed -= ReplaceSlot2;
            m_InputActions.PowerSelect.Replace3.performed -= ReplaceSlot3;
            m_InputActions.PowerSelect.Replace4.performed -= ReplaceSlot4;


            m_InputActions.PowerSelect.Exit.performed += ExitSelectionPanel;
        }
        void ReplaceSlot1(InputAction.CallbackContext context) => ReplaceSlot(0);
        void ReplaceSlot2(InputAction.CallbackContext context) => ReplaceSlot(1);
        void ReplaceSlot3(InputAction.CallbackContext context) => ReplaceSlot(2);
        void ReplaceSlot4(InputAction.CallbackContext context) => ReplaceSlot(3);

        private void ReplaceSlot(int slotIndex)
        {
            m_PlayerLoadout.ReplaceInventorySlotLoadout(slotIndex);

            ExitReplaceInventorySlotControls(); 
        }

        

        void ControlStateUpdate()
        {
            switch (m_ControlState)
            {
                case EControlState.Player:
                    SetMovementValue(m_InputActions.Player.Move.ReadValue<Vector2>());
                    SetIsSprinting(m_InputActions.Player.Sprint.IsPressed());
                    m_PlayerWeapons.m_LookDirection = m_InputActions.Player.Look.ReadValue<Vector2>();
                    m_PlayerWeapons.m_Parrying = m_InputActions.Player.Parry.IsPressed();
                    m_PlayerWeapons.m_Attacking = m_InputActions.Player.Attack.IsPressed();
                    m_PlayerMovement.MouseDelta = m_InputActions.Player.Look.ReadValue<Vector2>();
                    break;
                case EControlState.Ghost:
                    SetMovementValue(m_InputActions.Ghost.Move.ReadValue<Vector2>());
                    m_PlayerGhostMovement.MouseDelta = m_InputActions.Ghost.Look.ReadValue<Vector2>();



                    break;
                case EControlState.Selecting:
                    break;
                default:
                    break;
            }
        }

        internal void SwitchState(EControlState state)
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
                case EControlState.Player:
                    m_InputActions.Player.Enable();
                    m_PlayerGhostMovement.enabled = false;
                    Cursor.visible = false;
                    break;
                case EControlState.Ghost:
                    m_PlayerMovement.enabled = false;
                    m_InputActions.Ghost.Enable(); 
                    Cursor.visible = false;

                    break;
                case EControlState.Selecting:
                    m_InputActions.PowerSelect.Enable();
                    m_PlayerMovement.StopMovement();
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
                case EControlState.Player:
                    m_InputActions.Player.Disable();
                    break;
                case EControlState.Ghost:
                    m_InputActions.Ghost.Disable();
                    break;
                case EControlState.Selecting:
                    m_InputActions.PowerSelect.Disable();
                    break;
                default:
                    break;
            }
        }
    } 
}
