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

    // Update is called once per frame
    [Command]
    public void TakeMeleeCommand(int DirectionNESO, CoherenceSync sync, int damage,int damageType, Vector3 attackerPos)//cant use interfaces or abtract class direct commands
    {
        if(TryGetComponent<Entity>(out Entity ent))
        {
            ent.TakeMeleeSync(DirectionNESO, sync, damage,(EDamageType)damageType, attackerPos);
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
    public  void PotionEffect(string potionID)
    {
        if (TryGetComponent<Entity>(out Entity ent))
        {
            SO_Potion potion  = ItemRetriever.Instance.GetItem(potionID) as SO_Potion;
            if (potion == null)
            {
                Debug.Log("no such item in retriever");
                return;

            }

            ent.ApplyEffects(potion.Effects);
        }
    }
}
