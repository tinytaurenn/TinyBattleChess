using UnityEngine;

[CreateAssetMenu(fileName = "SO_Scroll", menuName = "Scriptable Objects/Items/SO_Scrolls/ScrollEffect/FireBall_Effect")]
public class SO_Scroll_FireBall : SO_ScrollEffectProjectile
{
    public override void OnActivate(Transform parent, Vector3 dir)
    {
        base.OnActivate(parent, dir);
       
    }
}
