using Coherence.Toolkit;
using UnityEngine;
using static BasicWeapon;

public class RotatingSpikeHit : MonoBehaviour
{
    Collider m_Collider;

    CoherenceSync m_Sync; 

    private void Awake()
    {
        m_Sync = GetComponent<CoherenceSync>();
        m_Collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Spike trigger " + other.name);

        if (other.TryGetComponent<CoherenceSync>(out CoherenceSync sync))
        {
            

            if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                Debug.Log("Spike hit " + other.name);

                //check defense 

               
                if (other.TryGetComponent<TinyPlayer>(out TinyPlayer tinyPlayer))
                {
                    Debug.Log("sending Spikedamage commannd to player");
                    sync.SendCommand<EntityCommands>(nameof(EntityCommands.TakeDamageCommand), Coherence.MessageTarget.AuthorityOnly,12,(int)EEffectType.Physical,m_Sync );
                }


            }
        }
    }

}
