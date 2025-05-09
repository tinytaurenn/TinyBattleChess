using Coherence.Toolkit;
using System.Collections;
using UnityEngine;

public class ScrollProjectile : Projectile
{
    public SO_ScrollEffectProjectile so_ScrollEffectProjectile;

    [SerializeField] float m_ExplosionDelay = 0.1f;

    [SerializeField] ParticleSystem[] m_ParticleSystems;
    [SerializeField] GameObject m_ExplosionGameobject;


    public SO_GameEffect_Container SO_GameEffectContainer; 

    protected override void OnHit(Collider other)
    {
        if(m_Timer < m_ActivationTime) return;
        Debug.Log("scroll projectile hit something!");
        m_Exploded = true;
        StartCoroutine(ExplosionRoutine(m_ExplosionDelay));

        m_Collider.enabled = false;
       
    }

    IEnumerator ExplosionRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);


        InstantiateExplosion(transform.position);
        m_Sync.SendCommand<ScrollProjectile>(nameof(ScrollProjectile.InstantiateExplosion), Coherence.MessageTarget.Other, transform.position);

        StopParticles();
        m_Sync.SendCommand<ScrollProjectile>(nameof(ScrollProjectile.StopParticles), Coherence.MessageTarget.Other);


        Collider[] HitList = Physics.OverlapSphere(transform.position, so_ScrollEffectProjectile.m_ExplosionRadius, so_ScrollEffectProjectile.HitMask);
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

                        sync.SendCommand<EntityCommands>(nameof(EntityCommands.GameEffect), Coherence.MessageTarget.AuthorityOnly, SO_GameEffectContainer.GameEffectID,m_Sync);

                    }
                }
            }
        }
        else
        {
            Debug.Log("potion projectile hit nothing");
        }

        Destroy(gameObject, 2f);
    }

    
    public override void InstantiateExplosion(Vector3 pos)
    {
        Debug.Log("instantiating explosion"); 
        Instantiate(m_ExplosionGameobject, pos, transform.rotation);
    }
    public override void StopParticles()
    {
        foreach (var item in m_ParticleSystems)
        {
            item.Stop();
        }
    }


    
}
