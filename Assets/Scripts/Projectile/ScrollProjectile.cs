using Coherence.Toolkit;
using System.Collections;
using UnityEngine;

public class ScrollProjectile : Projectile
{

    [SerializeField] float m_ExplosionDelay = 0.1f;

    [SerializeField] ParticleSystem[] m_ParticleSystems;
    [SerializeField] GameObject m_SpawnSoundPrefab; 

    protected override void OnHit(Collider other)
    {
        if(m_Timer < m_ActivationTime) return;
        Debug.Log("scroll projectile hit something!");
        m_Exploded = true;
        StartCoroutine(ExplosionRoutine(m_ExplosionDelay));

        m_Collider.enabled = false;
        Collider[] HitList = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_ExplosionMask);
        if (HitList.Length > 0)
        {

            foreach (Collider item in HitList)
            {
                if (item.TryGetComponent<EntityCommands>(out EntityCommands entityCommands))
                {
                    //see entity commands and potions effects etc
                    Debug.Log("scroll projectile hit entity");
                    if (item.TryGetComponent<CoherenceSync>(out CoherenceSync sync))
                    {
                        //Damage and stuff
                    }
                }
            }
        }
        else
        {
            Debug.Log("potion projectile hit nothing");
        }
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

    
    public override void InstantiateExplosion()
    {
        Debug.Log("instantiating explosion"); 
        Instantiate(m_ExplosionGameobject, transform.position, transform.rotation);
    }
    public override void StopParticles()
    {
        foreach (var item in m_ParticleSystems)
        {
            item.Stop();
        }
    }


    
}
