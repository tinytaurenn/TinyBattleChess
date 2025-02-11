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
    public void TakeMeleeCommand(int DirectionNESO, CoherenceSync sync, int damage, Vector3 attackerPos)//cant use interfaces or abtract class direct commands
    {
        if(TryGetComponent<Entity>(out Entity ent))
        {
            ent.TakeMeleeSync(DirectionNESO, sync, damage, attackerPos);
        }
    }

    public void SyncBlockedCommand()
    {
        if (TryGetComponent<Entity>(out Entity ent))
        {
            ent.SyncBlocked();
        }
    }

    public void SyncHitCommand()
    {
        if (TryGetComponent<Entity>(out Entity ent))
        {
            ent.SyncHit();
        }
    }
}
