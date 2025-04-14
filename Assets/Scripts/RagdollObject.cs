using Coherence.Toolkit;
using UnityEngine;

public class RagdollObject : MonoBehaviour, ICleanable
{
    CoherenceSync m_Sync;
    [SerializeField] float m_DestructionTimer = 150f;

    private void Awake()
    {
        m_Sync = GetComponent<CoherenceSync>();
    }
    private void FixedUpdate()
    {
        if (!m_Sync.HasStateAuthority) return; 

        m_DestructionTimer -= Time.fixedDeltaTime;
        if (m_DestructionTimer <= 0)
        {
            CleanObject();
        }
    }
    public void CleanObject()
    {
        if(m_Sync != null)
        {
            if (m_Sync.HasStateAuthority)
            {
                Destroy(gameObject);
            }
            else
            {
                m_Sync.RequestAuthority(Coherence.AuthorityType.Full);
                m_Sync.OnStateAuthority.AddListener(OnStateAuthority);
            }
        }


    }


    public void OnStateAuthority()
    {

        if (GetComponent<CoherenceSync>().HasStateAuthority)
        {
            Debug.Log("onstateauth destroy ragdoll");
            m_Sync.OnAuthTransferComplete.RemoveAllListeners();
            Destroy(gameObject) ;
        }
        else
        {
            Debug.Log("ragdoll state auth destroy doesnt work ");
        }
    }

}
