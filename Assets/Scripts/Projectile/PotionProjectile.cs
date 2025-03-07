using UnityEngine;

public class PotionProjectile : Projectile
{
    protected override void OnHit(Collider other)
    {
        Debug.Log("potion projectile hit something");
        Collider[] HitList = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_ExplosionMask);
        if(HitList.Length > 0)
        {
            
            foreach (Collider item in HitList)
            {
                if(item.TryGetComponent<EntityCommands>(out EntityCommands entityCommands))
                {
                    //see entity commands and potions effects etc
                    Debug.Log("potion projectile hit entity");
                }
            }
        }
        else
        {
            Debug.Log("potion projectile hit nothing");
        }

        Destroy(gameObject);
    }
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
