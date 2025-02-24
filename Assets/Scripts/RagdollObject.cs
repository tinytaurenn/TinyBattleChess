using Coherence.Toolkit;
using UnityEngine;

public class RagdollObject : MonoBehaviour, ICleanable
{
    public void CleanObject()
    {
        if(TryGetComponent<CoherenceSync>(out CoherenceSync sync))
        {
            if (sync.HasStateAuthority)
            {
                Destroy(gameObject);
            }
            else
            {
                sync.RequestAuthority(Coherence.AuthorityType.Full);
                sync.OnStateAuthority.AddListener(OnStateAuthority);
            }
        }

    }


    public void OnStateAuthority()
    {

        if (GetComponent<CoherenceSync>().HasStateAuthority)
        {
            Debug.Log("onstateauth destroy ragdoll");
            GetComponent<CoherenceSync>().OnAuthTransferComplete.RemoveAllListeners();
            Destroy(gameObject) ;
        }
        else
        {
            Debug.Log("ragdoll state auth destroy doesnt work ");
        }
    }

}
