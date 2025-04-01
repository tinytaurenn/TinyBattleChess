using Coherence.Toolkit;
using UnityEngine;

public class StaffWeapon : BasicWeapon,IWeapon
{
    [SerializeField] Transform m_StaffSpellPos; 
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

    public override void ApplyDamage(CoherenceSync damageTargetSync)
    {
        //throw new System.NotImplementedException();
    }

    public override void BlockAttackEffect(CoherenceSync damagerSync)
    {
        //throw new System.NotImplementedException();
    }

    public override void RaiseAttackEffect()
    {
        //throw new System.NotImplementedException();
    }

    public override void RaiseBlockEffect()
    {
        //throw new System.NotImplementedException();
    }

    public override void ReleaseAttackEffect()
    {
        foreach (var weaponEffect in WeaponEffects)
        {
            weaponEffect.OnAttackReleaseEndEffect(m_StaffSpellPos,ParentedCamera.Instance.transform.forward);
        }
    }
}
