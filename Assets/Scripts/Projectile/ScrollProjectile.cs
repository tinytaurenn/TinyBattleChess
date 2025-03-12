using Coherence.Toolkit;
using System.Collections;
using UnityEngine;

public class ScrollProjectile : Projectile
{
    float m_Timer = 0f;
    float m_ActivationTime = 0.05f;
    [SerializeField] float m_ExplosionDelay = 0.1f;

    [SerializeField] ParticleSystem[] m_ParticleSystems;
    [SerializeField] GameObject m_SpawnSoundPrefab; 
    private void Update()
    {
        if (m_Timer > m_ActivationTime) return;
        m_Timer += Time.deltaTime;    
    }
    private void Start()
    {

    }
    protected override void OnHit(Collider other)
    {
        if(m_Timer < m_ActivationTime) return;
        Debug.Log("scroll projectile hit something!");
        m_Exploded = true;
        StartCoroutine(ExplosionRoutine(m_ExplosionDelay));
    }

    IEnumerator ExplosionRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        InstantiateExplosion();
        m_Sync.SendCommand<ScrollProjectile>(nameof(ScrollProjectile.InstantiateExplosion), Coherence.MessageTarget.Other);

        StopParticles();
        m_Sync.SendCommand<ScrollProjectile>(nameof(ScrollProjectile.StopParticles), Coherence.MessageTarget.Other);

        Destroy(gameObject, 2f);
    }

    [Command]
    public void InstantiateExplosion()
    {
        Debug.Log("instantiating explosion"); 
        Instantiate(m_ExplosionGameobject, transform.position, transform.rotation);
    }
    [Command]
    public void StopParticles()
    {
        foreach (var item in m_ParticleSystems)
        {
            item.Stop();
        }
    }


    
}
