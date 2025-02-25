using Coherence.Toolkit;
using UnityEngine;

public abstract class Usable : MonoBehaviour
{
    protected CoherenceSync m_Sync;
    protected virtual void Awake()
    {
        if(TryGetComponent<CoherenceSync>(out CoherenceSync sync))
        {
            m_Sync = sync;
        }
        
    }
    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        
    }

    protected virtual void OnEnable()
    {
        if (TryGetComponent<CoherenceSync>(out CoherenceSync sync))
        {
            m_Sync = sync;
        }
    }


    public abstract void TryUse();

    
}
