
using UnityEngine;



namespace PlayerControls
{
    public class PlayerMovement : CharacterMovement
    {

        [SerializeField] CameraManager m_CameraManager;

        public Vector2 MouseDelta { get; set; }
        [Space(10)]
        [Header("player movement speeds ")]
        [SerializeField] float m_BackWardspeedModifier = 0.5f;
        [SerializeField] float m_StrafeSpeedModifier = 0.75f;

        public float BackWardspeedModifier => m_BackWardspeedModifier; 
        public float StrafeSpeedModifier => m_StrafeSpeedModifier;

        protected override void Awake()
        {
            base.Awake();
            
            m_CameraManager = CameraManager.Instance;

        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void MovementUpdate()
        {
            base.MovementUpdate();

            if (IsLocked) return; 
            float mouseDeltaX = MouseDelta.x;

            Quaternion newRotation = Quaternion.Euler(0, mouseDeltaX, 0); 
            transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation *  newRotation, Time.fixedDeltaTime * m_RotationSpeed);

        }


        public override void StopMovement()
        {
            base.StopMovement();
            MouseDelta = Vector2.zero;
        }

        public override void SitOnTarget(Transform target)
        {
            base.SitOnTarget(target);

        }



    } 
}
