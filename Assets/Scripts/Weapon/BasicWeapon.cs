using Coherence.Toolkit;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public abstract class BasicWeapon : Grabbable, IWeapon
{

    [SerializeField]
    protected FWeaponParameters m_WeaponParameters;

    public FWeaponParameters WeaponParameters {
        get
        {
            return m_WeaponParameters; 
        } set 
        { 
            m_WeaponParameters = value; 
        } 
    }

    public List<SO_WeaponEffect> WeaponEffects;

    AudioSource m_AudioSource; 
    public List<AudioResource> HitSounds;

    public List<AudioResource> m_ParryAudios;

    internal Transform m_HolderTransform = null; 

    [SerializeField]protected  Collider m_DamageCollider;
    [SerializeField] public Transform m_HitPos; 
   
   

    protected override void Awake()
    {
        base.Awake();
        m_AudioSource = GetComponent<AudioSource>();    
    }
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update(); 
    }

    public virtual void SetupWeapon(Collider damageCollider, FWeaponParameters weaponParameters)
    {
        m_DamageCollider = damageCollider;
        m_WeaponParameters = weaponParameters;
    }


    public override void Release()
    {
        base.Release();
        m_HolderTransform = null;
        
    }

    

    public void PlayHitSound()
    {
        if (!m_Sync.HasStateAuthority) return; 
        if(HitSounds.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, HitSounds.Count);
            m_AudioSource.resource = HitSounds[randomIndex];
            m_AudioSource.Play(); 
            m_Sync.SendCommand<BasicWeapon>(nameof(SyncHitSound), Coherence.MessageTarget.Other, randomIndex);
        }

    }
    [Command]
    public void SyncHitSound(int index)

    {
        m_AudioSource.resource = HitSounds[index];
        m_AudioSource.Play();


    }
    [Command]
    public void PlayParryFX(int choiceIndex)
    {
        m_AudioSource.resource = m_ParryAudios[choiceIndex];
        m_AudioSource.Play();

    }


    protected virtual void OnTriggerEnter(Collider other)
    {


    }

    public abstract void ApplyDamage(CoherenceSync damageTargetSync);
    public abstract void BlockAttackEffect(CoherenceSync damagerSync);
    public abstract void RaiseAttackEffect();
    public abstract void RaiseBlockEffect();
    public abstract void ReleaseAttackEffect(); 


}