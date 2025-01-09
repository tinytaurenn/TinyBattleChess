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

    protected virtual void OnEnable()
    {
        if (TryGetComponent<CoherenceSync>(out CoherenceSync sync))
        {
            m_Sync = sync;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TryUse()
    {

    }
    
}
