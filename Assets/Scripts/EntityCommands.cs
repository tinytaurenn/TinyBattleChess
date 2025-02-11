using Coherence.Toolkit;
using UnityEngine;

public class EntityCommands : MonoBehaviour
{
    CoherenceSync m_Sync; 
    private void Awake()
    {
        m_Sync = GetComponent<CoherenceSync>();
    }

    // Update is called once per frame
    [Command]
    public void TakeMeleeCommand(int DirectionNESO, CoherenceSync sync, int damage, Vector3 attackerPos)//cant use interfaces or abtract class direct commands
    {
        if(TryGetComponent<Entity>(out Entity ent))
        {
            ent.TakeMeleeSync(DirectionNESO, sync, damage, attackerPos);
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
            ent.ChangeGameID(gameID);
        }
    }
}
