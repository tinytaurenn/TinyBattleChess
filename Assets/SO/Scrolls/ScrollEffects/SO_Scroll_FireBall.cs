using UnityEngine;

[CreateAssetMenu(fileName = "SO_Scroll", menuName = "Scriptable Objects/Items/SO_Scrolls/ScrollEffect/FireBall_Effect")]
public class SO_Scroll_FireBall : SO_ScrollEffect
{
    [SerializeField] GameObject m_FireBallProjectile; 
    public override void OnActivate(Transform parent)
    {
        Debug.Log("launching fireball");
        GameObject proj = Instantiate(m_FireBallProjectile, parent.position, parent.rotation);
        if(proj.TryGetComponent<Projectile>(out Projectile projectile))
        {
            projectile.Launch(parent.forward);
        }
        else
        {
            Debug.LogError("Projectile component not found on fireball projectile");
        }
    }
}
