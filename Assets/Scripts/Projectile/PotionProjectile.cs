using Coherence.Toolkit;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PotionProjectile : Projectile
{

    public SO_Potion SO_Potion; 
    protected override void OnHit(Collider other)
    {
        Debug.Log("potion projectile hit something");
      
        if(SO_Potion == null || SO_Potion.GameEffectContainer ==null || SO_Potion.GameEffectContainer.Effects.Count<=0 )
        {
            Debug.Log("no potion effects");
            return;
        }
        m_Collider.enabled = false;
        InstantiateExplosion(transform.position + Vector3.up/4 ); 
        
        Collider[] HitList = Physics.OverlapSphere(transform.position, SO_Potion.ExplosionRadius,SO_Potion.HitMask);
        if(HitList.Length > 0)
        {
            
            foreach (Collider item in HitList)
            {
                if(item.TryGetComponent<EntityCommands>(out EntityCommands entityCommands))
                {
                    //see entity commands and potions effects etc
                    Debug.Log("potion projectile hit entity");
                    if(item.TryGetComponent<CoherenceSync>(out CoherenceSync sync))
                    {
                        ApplyEffectsOnEntity(sync);
                    }
                }
            }
        }
        else
        {
            Debug.Log("potion projectile hit nothing");
        }

        Destroy(gameObject,1);
    }

    void ApplyEffectsOnEntity(CoherenceSync entSync)
    {
        entSync.SendCommand<EntityCommands>(nameof(EntityCommands.GameEffect), Coherence.MessageTarget.AuthorityOnly,SO_Potion.GameEffectContainer.GameEffectID,m_Sync);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    public void SetupPotionProjectile(SO_Potion so_potion,Mesh potionMesh, Material mat)
    {
        //base.SetupProjectile();

        SO_Potion = so_potion;
        m_MeshFilter.mesh = potionMesh;
        m_Renderer.material = mat;

    }

    public override void InstantiateExplosion(Vector3 pos)
    {
        Debug.Log("potion projectile explosion");
        GameObject potionExplosionVFX = Instantiate(SO_Potion.ExplosionEffect, pos, SO_Potion.ExplosionEffect.transform.rotation);

    }

    public override void StopParticles()
    {
        Debug.Log("potion projectile stop particles");
    }
}
