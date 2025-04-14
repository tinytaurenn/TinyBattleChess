using Coherence.Toolkit;
using UnityEngine;
using UnityEngine.InputSystem;

public class EntityCommands : MonoBehaviour
{
    CoherenceSync m_Sync; 
    private void Awake()
    {
        m_Sync = GetComponent<CoherenceSync>();
    }
    [Command]
    public  void PlayDamageSoundCommand(int intWeaponType)
    {
        if (TryGetComponent<Entity>(out Entity ent))
        {
            ent.PlayDamageSound((EWeaponType)intWeaponType);
        }
    }

    // Update is called once per frame
    [Command]
    public void TakeMeleeCommand(int DirectionNESO, CoherenceSync sync, int damage,int damageType,int weaponType, Vector3 attackerPos)//cant use interfaces or abtract class direct commands
    {
        if(TryGetComponent<Entity>(out Entity ent))
        {
            ent.TakeMeleeSync(DirectionNESO, sync, damage,(EEffectType)damageType,(EWeaponType)weaponType, attackerPos);
        }
    }
    [Command]
    public void TakeDamageCommand(int damage, int damageType, CoherenceSync Damagersync)
    {
        if (TryGetComponent<Entity>(out Entity ent))
        {
            ent.TakeDamageSync(damage,(EEffectType)damageType,Damagersync);
        }
    }
    [Command]
    public void SyncBlockedCommand()
    {
        if (TryGetComponent<Entity>(out Entity ent))
        {
            ent.SyncBlocked();
        }
    }
    [Command]
    public void SyncHitCommand()
    {
        if (TryGetComponent<Entity>(out Entity ent))
        {
            ent.SyncHit();
        }
    }

    [Command]
    public void ChangeGameIDCommand(int gameID)
    {
        if (TryGetComponent<Entity>(out Entity ent))
        {
            //ent.ChangeGameID(gameID);
            ent.GameID = gameID;
        }
    }
    [Command]
    public void GetAttackState(CoherenceSync AskerSync)
    {
        if (TryGetComponent<Entity>(out Entity ent))
        {
            bool isAttacking = ent.GetAttackState(out EWeaponDirection attackDir);

            if (AskerSync.GetComponent<EntityCommands>())
            {
                AskerSync.SendCommand<EntityCommands>(nameof(OnReceiveAttackState), Coherence.MessageTarget.AuthorityOnly,isAttacking, (int)attackDir);
            }
        }
    }
    [Command]
    public void OnReceiveAttackState(bool isAttacking,int intAttackDir)
    {
        if (TryGetComponent<Entity>(out Entity ent))
        {

            ent.OnReceiveAttackState(isAttacking,(EWeaponDirection)intAttackDir);
        }
    }
    [Command]
    public  void GameEffect(string GameEffectID, CoherenceSync damagerSync)
    {
        Debug.Log("game effect id : " + GameEffectID); 
        if (TryGetComponent<Entity>(out Entity ent))
        {
            SO_GameEffect_Container container  = ItemRetriever.Instance.GetEffect(GameEffectID);
            if (container == null)
            {
                Debug.Log("no such item in retriever");
                return;

            }

            ent.ApplyEffects(container.Effects, damagerSync);
        }
    }
    [Command]
    public void OneGameEffect(int IntEGameEffect,float value, float duration, int IntEEffectType, CoherenceSync damagerSync)
    {
        if (TryGetComponent<Entity>(out Entity ent))
        {
            ent.ApplyEffect(new FGameEffect((EGameEffect)IntEGameEffect, value, duration, (EEffectType)IntEEffectType), damagerSync);
        }
    }
}
