using Coherence.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.Tilemaps;

public class SplineSlider : MonoBehaviour
{
    [SerializeField] SplineContainer m_Spline;
    [SerializeField] AnimationCurve m_Curve;

    [SerializeField]
    [Range(0f, 1f)][Sync]public float m_Time;

    [SerializeField] float m_SlideTime = 2f;
    [SerializeField] float m_WaitDelay = 0f;
    [SerializeField] bool m_PingPong = false;
    [SerializeField] bool m_SplineRotating = false;
    [SerializeField] bool m_OnlyYRotate = false;

    Vector3 m_StartPos;
    float m_WaitTime;
    bool m_IsBackWard = false;
    [SerializeField] Vector3 m_PlatformVelocity;

    private void Awake()
    {
        m_StartPos = transform.position;
        m_WaitTime = m_WaitDelay; 

    }


    private void OnValidate()
    {
        if (m_Spline) Slide(); 
    }

    private void FixedUpdate()
    {
        if(m_WaitTime > 0)
        {
            m_WaitTime -= Time.deltaTime;
        }
        else
        {
            m_Time += Time.deltaTime / m_SlideTime; 
            if(m_Time >= 1)
            {
                m_Time = 0f; 

                if (m_PingPong) m_IsBackWard = !m_IsBackWard;
                m_WaitTime = m_WaitDelay;
            }

            
        }

        Slide(); 
    }

    void Slide()
    {

        Vector3 previousPos = transform.position;
        float timer = m_Time;

        if (m_PingPong) timer = m_IsBackWard ? 1 - m_Time : m_Time; 
        float delta = m_Curve.Evaluate(timer);
        if(m_Spline != null)
        {
            transform.position = m_Spline.EvaluatePosition(delta);
            if (m_SplineRotating) RotateOnSlide(delta);

        }
        else
        {
            transform.position = m_StartPos; 
        }
        Vector3 newPlatFormVelocity = (transform.position - previousPos) * (1f / Time.deltaTime);


        m_PlatformVelocity = Vector3.Lerp(m_PlatformVelocity, newPlatFormVelocity, 0.25f);  
        


    }

    void RotateOnSlide(float delta)
    {
        Vector3 rotateAxis = m_Spline.EvaluateTangent(delta); 
        if(m_OnlyYRotate) rotateAxis = new Vector3(rotateAxis.x,0, rotateAxis.z);
        if(rotateAxis.sqrMagnitude > 0) transform.rotation = Quaternion.LookRotation(rotateAxis, Vector3.up);
       
    }
}
