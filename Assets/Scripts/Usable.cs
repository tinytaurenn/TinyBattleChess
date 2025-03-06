using Coherence;
using Coherence.Toolkit;
using System;
using UnityEngine;

public abstract class Usable : MonoBehaviour
{
    protected CoherenceSync m_Sync;

    [SerializeField] protected bool m_UseRequested = false;

    public event Action<bool> OnUseValidate;
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
        if (m_Sync == null) return;

        m_Sync.OnAuthorityRequestRejected.AddListener(OnAuthorityRequestRejected);
    }

    protected virtual void OnDisable()
    {
        if (m_Sync == null) return; 
        m_Sync.OnAuthorityRequestRejected.RemoveListener(OnAuthorityRequestRejected);
    }



    public abstract void TryUse();

    protected virtual void DoUse()
    {
        OnUseValidate?.Invoke(true);
    }

    protected virtual void OnAuthorityRequestRejected(AuthorityType arg0)
    {

        OnUseValidate?.Invoke(false);
    }


}
