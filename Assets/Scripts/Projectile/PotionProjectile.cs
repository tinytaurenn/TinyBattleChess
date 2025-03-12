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
        if(SO_Potion.Effects.Count<=0)
        {
            Debug.Log("no potion effects");
            return;
        }
        m_Collider.enabled = false;
        Collider[] HitList = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_ExplosionMask);
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
        entSync.SendCommand<EntityCommands>(nameof(EntityCommands.PotionEffect), Coherence.MessageTarget.AuthorityOnly, SO_Potion.ItemID);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
