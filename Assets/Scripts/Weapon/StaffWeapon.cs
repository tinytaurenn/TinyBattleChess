using UnityEngine;

public class StaffWeapon : BasicWeapon,IWeapon
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void SetupWeapon(Collider damageCollider, FWeaponParameters weaponParameters)
    {
        m_DamageCollider = damageCollider;
        m_WeaponParameters = weaponParameters;
    }

    public override void Release()
    {
        base.Release();

    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
