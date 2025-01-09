using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    [SerializeField] ParticleSystem m_RunParticles;
    [SerializeField] ParticleSystem m_JumpParticles;
    [SerializeField] ParticleSystem m_LandParticles;

   


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

  
}
