using UnityEngine;

[CreateAssetMenu(fileName = "SO_WeaponStaffEffect", menuName = "Scriptable Objects/Items/Weapons/WeaponEffect/LightningStaffWeaponEffect")]



public class SO_WeaponLightningStaffEffect : SO_WeaponEffect
{

    [SerializeField] GameObject LightningProjectile;

    [SerializeField] float m_ThrowForce = 10f;


    public override void OnAttackReadyEffect()
    {
        throw new System.NotImplementedException();
    }

    public override void OnAttackReleaseEndEffect(Transform parentPos,Vector3 direction)
    {
        Debug.Log("Lightning staff attack release end effect");
        GameObject projectile = Instantiate(LightningProjectile, parentPos.position, Quaternion.identity);
        if(projectile.TryGetComponent<Projectile>(out Projectile projectileComponent))
        {
            projectileComponent.Launch(direction, m_ThrowForce);
        }
    }

    public override void OnAttackReleaseStartEffect()
    {
        throw new System.NotImplementedException();
    }

    public override void OnBlockEffect()
    {
        throw new System.NotImplementedException();
    }

    public override void OnBlockReadyEffect()
    {
        throw new System.NotImplementedException();
    }

    public override void OnGrabWeaponEffect()
    {
        throw new System.NotImplementedException();
    }

    public override void OnHitEffect()
    {
        throw new System.NotImplementedException();
    }

    public override void OnThrowWeaponEffect()
    {
        throw new System.NotImplementedException();
    }
}
