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

    public void ChangeGameID(int gameID)
    {
        GameID = gameID; 
    }

    public abstract void EntityDeath();

    public abstract void Stun(); 

    public abstract void TakeMeleeSync(int DirectionNESO, CoherenceSync sync, int damage, Vector3 attackerPos);

    public abstract void TakeWeaponDamageSync(int damage, CoherenceSync Damagersync);


    public abstract void ParrySync(int damage, CoherenceSync DamagerSync);

    public abstract void TakeDamageSync(int damage, CoherenceSync Damagersync);

    public abstract void SyncBlocked();

    public abstract void SyncHit();
}
