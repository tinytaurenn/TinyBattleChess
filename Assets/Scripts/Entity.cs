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

    [Sync][SerializeField]protected int m_EntityHealth = 100;

    public virtual int EntityHealth
    {
        get { return m_EntityHealth; }
        set
        {
            m_EntityHealth = value;
        }
    }

    public abstract void EntityDeath();

    public abstract void Stun(); 

    public abstract void TakeMeleeSync(int DirectionNESO, CoherenceSync sync, int damage, EDamageType damageType,Vector3 attackerPos);

    public abstract void TakeWeaponDamageSync(int damage,EDamageType damageType, CoherenceSync Damagersync);


    public abstract void ParrySync(int damage, CoherenceSync DamagerSync);

    public abstract void TakeDamageSync(int damage,EDamageType damageType, CoherenceSync Damagersync);

    public abstract void SyncBlocked();

    public abstract void SyncHit();

    public abstract bool GetAttackState(out EWeaponDirection attackDir);

    public abstract void OnReceiveAttackState(bool isAttacking,EWeaponDirection attackDir);


    public virtual void ApplyEffects(List<FGameEffect> effects)
    {
        foreach (FGameEffect effect in effects)
        {
            ApplyEffect(effect);
        }
    }
    public virtual void ApplyEffect(FGameEffect effect)
    {
        switch (effect.Effect)
        {
            case EPotionEffect.Healing:
                HealingEffect(effect.Value);
                break;
            case EPotionEffect.Regeneration:
                RegenerationEffect(effect.Value, effect.EffectDuration);
                break;
            case EPotionEffect.Strength:
                StrengthEffect(effect.Value, effect.EffectDuration);
                break;
            case EPotionEffect.Speed:
                SpeedEffect(effect.Value, effect.EffectDuration);
                break;
            case EPotionEffect.AttackSpeed:
                AttackSpeedEffect(effect.Value, effect.EffectDuration);
                break;
            case EPotionEffect.JumpHeight:
                JumpHeightEffect(effect.Value, effect.EffectDuration);
                break;
            case EPotionEffect.Fly:
                FlyEffect(effect.Value, effect.EffectDuration);
                break;
            case EPotionEffect.Parry:
                ParryEffect(effect.Value, effect.EffectDuration);
                break;
            case EPotionEffect.Invisibility:
                InvisibilityEffect(effect.Value, effect.EffectDuration);
                break;
            case EPotionEffect.Damage:
                DamageEffect(effect.Value);
                break;
            case EPotionEffect.Poison:
                PoisonEffect(effect.Value, effect.EffectDuration);
                break;
            case EPotionEffect.Fire:
                FireEffect(effect.Value, effect.EffectDuration);
                break;
            case EPotionEffect.Slow:
                SlowEffect(effect.Value, effect.EffectDuration);
                break;
            case EPotionEffect.Blind:
                BlindEffect(effect.Value, effect.EffectDuration);
                break;
            case EPotionEffect.Grounded:
                Grounded(effect.Value, effect.EffectDuration);
                break;
            case EPotionEffect.weakness:
                Weakness(effect.Value, effect.EffectDuration);
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

    public virtual void DamageEffect(float value)
    {
        m_EntityHealth -= (int)value;
    }

    public virtual void PoisonEffect(float value, float duration)
    {
        
    }

    public virtual void FireEffect(float value, float duration)
    {
        
    }

    public virtual void SlowEffect(float value, float duration)
    {
        
    }

    public virtual void BlindEffect(float value, float duration)
    {
        
    }

    public virtual void Grounded(float value, float duration)
    {
        
    }

    public virtual void Weakness(float value, float duration)
    {
        
    }


}
