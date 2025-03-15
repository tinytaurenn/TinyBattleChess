using UnityEngine;

[CreateAssetMenu(fileName = "SO_Scroll", menuName = "Scriptable Objects/Items/SO_Scrolls/ScrollEffect/ScrollEffect_Projectile")]
public class SO_ScrollEffectProjectile : SO_ScrollEffect
{

    [SerializeField] GameObject m_Projectile;
    public override void OnActivate(Transform parent)
    {
        Debug.Log("launching projectile ");
        GameObject proj = Instantiate(m_Projectile, parent.position, parent.rotation);
        if (proj.TryGetComponent<ScrollProjectile>(out ScrollProjectile projectile))
        {
            projectile.SO_GameEffectContainer = SO_GameEffect_Container;
            projectile.Launch(parent.forward);
        }
        else
        {
            Debug.LogError("Projectile component not found on projectile");
        }
    }
}
