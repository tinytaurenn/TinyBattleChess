using UnityEngine;

[CreateAssetMenu(fileName = "SO_Scroll", menuName = "Scriptable Objects/Items/SO_Scrolls/ScrollEffect/FireBall_Effect")]
public class SO_Scroll_FireBall : SO_ScrollEffect
{
    public override void OnActivate(Transform parent)
    {
        Debug.Log("launching fireball"); 
    }
}
