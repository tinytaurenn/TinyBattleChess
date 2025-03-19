using Coherence.Toolkit;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : BasicWeapon,IWeapon
{
    public List<SO_MeleeWeaponEffect> MeleeWeaponEffects;

    List<Collider> HitList = new List<Collider>();
    [SerializeField]

    protected FMeleeWeaponParameters m_MeleeWeaponParameters = new FMeleeWeaponParameters(10,1);

    public FMeleeWeaponParameters MeleeWeaponParameters
    {
        get
        {
            return m_MeleeWeaponParameters;
        }
        set
        {
            m_MeleeWeaponParameters = value;
        }
    }
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

    public void ActivateDamage(bool activate)
    {
        Debug.Log("activating damage collider");
        m_DamageCollider.enabled = activate;
        if (activate)
        {
            HitList.Clear();
        }

    }

    public override void Release()
    {
        base.Release();

    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (m_Sync == null || !m_Sync.HasStateAuthority)
        {
            return;
        }
        if (m_HolderTransform == null)
        {
            return;
        }
        if (m_DamageCollider.enabled == false || !m_DamageCollider.isTrigger || other.CompareTag("DamageCollider"))
        {


            return;
        }
        if (!other.TryGetComponent<IDamageable>(out IDamageable damageable) && other.CompareTag("Untagged") && HitList.Count <= 0)
        {
            if (m_HolderTransform.TryGetComponent<PlayerWeapons>(out PlayerWeapons weapons))
            {
                ActivateDamage(false);
                weapons.SyncBlocked();
            }
        }

        if (m_HolderTransform == other.transform)
        {
            Debug.Log("same holder");
            return;
        }

        if (HitList.Contains(other)) return;

        HitList.Add(other);




        if (other.TryGetComponent<CoherenceSync>(out CoherenceSync sync))
        {

            Debug.Log("found coSync");

            if (other.TryGetComponent<EntityCommands>(out EntityCommands entCommands))
            {
                ApplyDamage(sync);
            }
        }
    }

    public override void ApplyDamage(CoherenceSync damageTargetSync)
    {
        int weaponDir = 0;
        if (m_HolderTransform.TryGetComponent<PlayerWeapons>(out PlayerWeapons weapons))
        {
            weaponDir = (int)weapons.m_WeaponDirection;
        }
        else if (m_HolderTransform.TryGetComponent<HumanoidNPC>(out HumanoidNPC npc))
        {
            weaponDir = (int)npc.m_WeaponDirection;
        }
        CoherenceSync holderSync = m_HolderTransform.GetComponent<CoherenceSync>();
        damageTargetSync.SendCommand<EntityCommands>(nameof(EntityCommands.TakeMeleeCommand), Coherence.MessageTarget.AuthorityOnly, weaponDir, holderSync, m_MeleeWeaponParameters.Damage, (int)m_WeaponParameters.DamageType, m_HolderTransform.transform.position);
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
        //throw new System.NotImplementedException();
    }
}
