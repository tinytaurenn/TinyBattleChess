using UnityEngine;

[CreateAssetMenu(fileName = "SO_Scroll", menuName = "Scriptable Objects/Items/SO_Scrolls/ScrollEffect/ScrollEffect_Projectile")]
public class SO_ScrollEffectProjectile : SO_ScrollEffect
{
    public float m_ExplosionRadius = 5f;
    public float m_ThrowForce = 10f; 

    [SerializeField] GameObject m_Projectile;
    public override void OnActivate(Transform parent, Vector3 dir)
    {
        Debug.Log("launching projectile ");
        GameObject proj = Instantiate(m_Projectile, parent.position, parent.rotation);
        if (proj.TryGetComponent<ScrollProjectile>(out ScrollProjectile projectile))
        {
            projectile.so_ScrollEffectProjectile = this;
            projectile.SO_GameEffectContainer = SO_GameEffect_Container;
            projectile.Launch(dir, m_ThrowForce);
        }
        else
        {
            Debug.LogError("Projectile component not found on projectile");
        }
    }
}
