using UnityEngine;


public abstract class SO_MeleeWeaponEffect : ScriptableObject
{
    public abstract void OnHitEffect();
    public abstract void OnParryEffect();
    public abstract void OnAttackReadyEffect(); 
    public abstract void OnBlockReadyEffect();    
}
