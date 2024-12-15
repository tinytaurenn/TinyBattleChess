using Coherence.Toolkit;
using UnityEngine;

public abstract class Usable : MonoBehaviour
{
    protected CoherenceSync m_Sync;
    protected virtual void Awake()
    {
 
        m_Sync = GetComponent<CoherenceSync>();
    }

    protected virtual void OnEnable()
    {
        m_Sync = GetComponent<CoherenceSync>();
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
