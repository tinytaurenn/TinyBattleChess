using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coherence;
using Coherence.Toolkit;
using NUnit.Framework.Internal.Filters;


namespace PlayerControls
{
    public class PlayerMovement : CharacterMovement
    {

        [SerializeField] CameraManager m_CameraManager;

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

            if (m_CameraManager != null)
            {
                Vector3 cameraForward = new Vector3(m_CameraManager.transform.forward.x, 0, m_CameraManager.transform.forward.z).normalized;
                Quaternion newRotation = Quaternion.LookRotation(cameraForward * 100f);
                transform.rotation = Quaternion.Lerp(m_rigidBody.rotation, newRotation, Time.deltaTime * m_RotationSpeed);

            }
  
        }


        

    } 
}
