using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControls
{
    public class PlayerUse : MonoBehaviour
    {

        float m_LastUseTime = 0;
        [SerializeField] float m_UseCooldown = 0.3f;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        internal void UsePerformed()
        {
            if(m_LastUseTime + m_UseCooldown > Time.time)
            {
                Debug.Log("use in cooldown  ");
                return;
            }
            m_LastUseTime = Time.time;
            Debug.Log("UsePerformed");
        }

    } 
}
