using Coherence.Toolkit;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected int m_GameID = -1;
    [Sync]
    public int GameID
    {
        get { return m_GameID; }
        set { m_GameID = value; }
    }

    [SerializeField]protected int m_EntityHealth = 100;
    [Sync]
    public virtual int EntityHealth
    {
        get { return m_EntityHealth; }
        set
        {
            m_EntityHealth = value;
        }
    }
    public abstract void PlayDamageSound(EWeaponType weaponType);

    public abstract void EntityDeath();

    

    public abstract void TakeMeleeSync(int DirectionNESO, CoherenceSync sync, int damage, EEffectType damageType, EWeaponType weaponType, Vector3 attackerPos);

    public abstract void TakeWeaponDamageSync(int damage,EEffectType damageType, EWeaponType weaponType, CoherenceSync Damagersync);


    public abstract void ParrySync(int damage, CoherenceSync DamagerSync);

    public abstract void TakeDamageSync(int damage,EEffectType damageType, CoherenceSync Damagersync);

    public abstract void SyncBlocked();

    public abstract void SyncHit();

    public abstract bool GetAttackState(out EWeaponDirection attackDir);

    public abstract void OnReceiveAttackState(bool isAttacking,EWeaponDirection attackDir);


    public virtual void ApplyEffects(List<FGameEffect> effects, CoherenceSync damagerSync)
    {
        foreach (FGameEffect effect in effects)
        {
            ApplyEffect(effect,damagerSync);
        }
    }
    public virtual void ApplyEffect(FGameEffect effect,CoherenceSync damagerSync)
    {
        Debug.Log("Applying effect " + effect.Effect.ToString());
        switch (effect.Effect)
        {
            case EGameEffect.Healing:
                HealingEffect(effect.Value);
                break;
            case EGameEffect.Regeneration:
                RegenerationEffect(effect.Value, effect.EffectDuration);
                break;
            case EGameEffect.Strength:
                StrengthEffect(effect.Value, effect.EffectDuration);
                break;
            case EGameEffect.Speed:
                SpeedEffect(effect.Value, effect.EffectDuration);
                break;
            case EGameEffect.AttackSpeed:
                AttackSpeedEffect(effect.Value, effect.EffectDuration);
                break;
            case EGameEffect.JumpHeight:
                JumpHeightEffect(effect.Value, effect.EffectDuration);
                break;
            case EGameEffect.Fly:
                FlyEffect(effect.Value, effect.EffectDuration);
                break;
            case EGameEffect.Parry:
                ParryEffect(effect.Value, effect.EffectDuration);
                break;
            case EGameEffect.Invisibility:
                InvisibilityEffect(effect.Value, effect.EffectDuration);
                break;
            case EGameEffect.Damage:
                DamageEffect(effect.Value,effect.EffectType,damagerSync);
                break;
            case EGameEffect.Poison:
                PoisonEffect(effect.Value, effect.EffectDuration);
                break;
            case EGameEffect.Fire:
                FireEffect(effect.Value, effect.EffectDuration);
                break;
            case EGameEffect.Stun:
                StunEffect(effect.EffectDuration);
                break;
            case EGameEffect.Slow:
                SlowEffect(effect.Value, effect.EffectDuration);
                break;
            case EGameEffect.Blind:
                BlindEffect(effect.Value, effect.EffectDuration);
                break;
            case EGameEffect.Grounded:
                GroundedEffect(effect.Value, effect.EffectDuration);
                break;
            case EGameEffect.Bump:
                BumpEffect(effect.Value);
                break;
            case EGameEffect.weakness:
                WeaknessEffect(effect.Value, effect.EffectDuration);
                break;
            default:
                break;
        }
    }
    
    public virtual void HealingEffect(float value)
    {
        Debug.Log("Healing from potion");
        m_EntityHealth += (int)value;
    }

    public virtual void RegenerationEffect(float value, float duration)
    {
        
    }

    public virtual void StrengthEffect(float value, float duration)
    {
        
    }

    public virtual void SpeedEffect(float value, float duration)
    {
        
    }

    public virtual void AttackSpeedEffect(float value, float duration)
    {
        
    }

    public virtual void JumpHeightEffect(float value, float duration)
    {
        
    }

    public virtual void FlyEffect(float value, float duration)
    {
        
    }

    public virtual void ParryEffect(float value, float duration)
    {
        
    }

    public virtual void InvisibilityEffect(float value, float duration)
    {
        
    }

    public virtual void DamageEffect(float value, EEffectType damageType, CoherenceSync damagerSync)
    {
        Debug.Log("Damage from effect");
        //m_EntityHealth -= (int)value;

        TakeDamageSync((int)value, damageType, damagerSync);
    }

    public virtual void PoisonEffect(float value, float duration)
    {
        
    }

    public virtual void FireEffect(float value, float duration)
    {
        
    }

    public virtual void StunEffect(float duration)
    {

    }

    public virtual void SlowEffect(float value, float duration)
    {
        
    }

    public virtual void BlindEffect(float value, float duration)
    {
        
    }

    public virtual void GroundedEffect(float value, float duration)
    {
        
    }
    public virtual void BumpEffect(float value)
    {
        
    }

    public virtual void WeaknessEffect(float value, float duration)
    {
        
    }


}
