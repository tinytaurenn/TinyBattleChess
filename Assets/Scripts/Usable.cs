using Coherence;
using Coherence.Connection;
using Coherence.Toolkit;
using System;
using UnityEngine;

public abstract class Usable : MonoBehaviour
{
    protected CoherenceSync m_Sync;

    [SerializeField] protected bool m_UseRequested = false;

    public event Action<bool> OnUseValidate;


    [Space(10)]
    [Header("Use Outline properties")]
    Outline m_UsableOutline;
    public OutlineSetting m_OutLineSetting; 

    protected virtual void Awake()
    {
        if(TryGetComponent<CoherenceSync>(out CoherenceSync sync))
        {
            m_Sync = sync;
        }
        if(m_OutLineSetting == null)
        {
            Debug.LogWarning("no outline setting in this prefab");
            return; 
        }
        m_UsableOutline = gameObject.AddComponent<Outline>();
        m_UsableOutline.OutlineMode = m_OutLineSetting.OutlineMode ;
        m_UsableOutline.OutlineColor = m_OutLineSetting.OutlineColor;
        m_UsableOutline.OutlineWidth = m_OutLineSetting.OutlineWidth;
        m_UsableOutline.enabled = false;
  
        
    }
    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        
    }
    public void EnableOutline(bool enable)
    {
        if (m_UsableOutline == null) return; 
        m_UsableOutline.enabled = enable;
    }

    protected virtual void OnEnable()
    {
        if (TryGetComponent<CoherenceSync>(out CoherenceSync sync))
        {
            m_Sync = sync;
        }
        if (m_Sync == null) return;

        m_Sync.OnStateAuthority.AddListener(OnStateAuthority);
        m_Sync.OnAuthorityRequestRejected.AddListener(OnAuthorityRequestRejected);
        m_Sync.OnAuthorityRequested += OnAuthorityRequested;
    }

    protected virtual void OnDisable()
    {
        if (m_Sync == null) return;
        m_Sync.OnStateAuthority.RemoveListener(OnStateAuthority);
        m_Sync.OnAuthorityRequestRejected.RemoveListener(OnAuthorityRequestRejected);
        m_Sync.OnAuthorityRequested -= OnAuthorityRequested;
    }



    public virtual void TryUse()
    {
        if (m_Sync.HasStateAuthority)
        {
            DoUse();
        }
        else
        {
            m_UseRequested = true;
            m_Sync.RequestAuthority(AuthorityType.Full);
            Debug.Log("Requesting auth");


        }
    }

    protected virtual void DoUse()
    {
        OnUseValidate?.Invoke(true);
    }

    protected virtual void OnStateAuthority()
    {
        Debug.Log("usable main OnStateAuthority");
        if (m_UseRequested)
        {
            Debug.Log("grabbable grab requested");
            m_UseRequested = false;
            DoUse();

        }
       
    }

    protected virtual void OnAuthorityRequestRejected(AuthorityType arg0)
    {

        OnUseValidate?.Invoke(false);
    }

    protected virtual bool OnAuthorityRequested(ClientID requesterID, AuthorityType authorityType, CoherenceSync sync)
    {
        return true;
    }


}
