using Coherence.Toolkit;
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

    public virtual void PotionEffect(EPotionEffect effect, float value, float duration)
    {

    }
    public virtual void PotionHealing(float value)
    {
        Debug.Log("Healing from potion");
        m_EntityHealth += (int)value;
    }

    public virtual void PotionRegeneration(float value, float duration)
    {
        
    }
}
