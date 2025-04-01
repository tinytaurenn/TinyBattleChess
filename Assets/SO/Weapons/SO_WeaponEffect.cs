using UnityEngine;


public abstract class SO_WeaponEffect : ScriptableObject
{
    public abstract void OnHitEffect();
    public abstract void OnAttackReadyEffect(); 
    public abstract void OnAttackReleaseStartEffect(); 
    public abstract void OnAttackReleaseEndEffect(); 
    public abstract void OnBlockEffect();
    public abstract void OnBlockReadyEffect();    

    public abstract void OnThrowWeaponEffect();
    public abstract void OnGrabWeaponEffect();


}
