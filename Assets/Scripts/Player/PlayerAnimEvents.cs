using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    [SerializeField] ParticleSystem m_RunParticles;
    [SerializeField] ParticleSystem m_JumpParticles;
    [SerializeField] ParticleSystem m_LandParticles;

    [SerializeField] ParticleSystem m_RightPunchParticles; 
    [SerializeField] ParticleSystem m_LeftPunchParticles;

    [SerializeField]
    AudioClip[] m_FootStepClips;

    [SerializeField]
    AudioClip[] m_jumpClips;

    [SerializeField] AudioSource m_StepAudioSource; 
    [SerializeField] AudioSource m_ActionAudioSource; 

   


    Transform m_LandParticlesTransform;

    private void Awake()
    {
        m_LandParticlesTransform = m_LandParticles.transform;
    }

    public void PlayRunParticles() => m_RunParticles.Play();
    public void StopRunParticles() => m_RunParticles.Stop();
    public void PlayLandParticles()
    {
        // Calculate the landing particles position before playing them,
        // to account for the fact that remote players might still be in the air when this is invoked
        Ray ray = new(transform.position + Vector3.up * .5f, Vector3.down);
        Physics.Raycast(ray, out RaycastHit raycastHit, 1.3f);
        m_LandParticlesTransform.position = raycastHit.point + Vector3.up * .1f;

        m_LandParticles.Play();
    }

    public void PlayJumpParticles()
    {
        m_JumpParticles.Play();
        StopRunParticles();
    }
    public void PlayRightPunch() => m_RightPunchParticles.Play();
    public void PlayLeftPunch() => m_LeftPunchParticles.Play();

    public void PlayFootStep()
    {
        m_StepAudioSource.pitch = 1; 
        m_StepAudioSource.clip = m_FootStepClips[Random.Range(0, m_FootStepClips.Length)];
        m_StepAudioSource.Play(); 
        
    }
    public void PlayJumpSound()
    {
        m_StepAudioSource.pitch = Random.Range(0.8f, 1.2f); // Randomize the pitch for a more natural sound
        m_StepAudioSource.clip = m_jumpClips[Random.Range(0, m_jumpClips.Length)];
        m_StepAudioSource.Play();

    }

}
