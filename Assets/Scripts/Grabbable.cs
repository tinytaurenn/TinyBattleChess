using Coherence;
using Coherence.Connection;
using Coherence.Toolkit;
using System;
using UnityEngine;

public abstract class Grabbable : Usable
{
    public SO_Item SO_Item
    {
        get { return m_SO_Item;}
        private set { m_SO_Item = value; }
    }

    [SerializeField] SO_Item m_SO_Item; 

    [Sync] public bool m_IsHeld;

    public event Action<bool> OnGrabValidate;

    [SerializeField] bool m_GrabRequested = false; 


    [HideInInspector] public Rigidbody m_Rigidbody;
    [HideInInspector] public Collider m_Collider;


    protected override void Awake()
    {
        base.Awake();
        
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();

        m_Sync.CoherenceBridge.onDisconnected.AddListener(OnDisconnected);
        
        

    }

    private void OnDisconnected(CoherenceBridge arg0, ConnectionCloseReason arg1)
    {
        Debug.Log("disconnected");
        if(m_Sync.CoherenceBridge != null) m_Sync.CoherenceBridge.onDisconnected.RemoveListener(OnDisconnected);
        if(this.gameObject != null) Destroy(gameObject);

    }

    void Start()
    {
        
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        m_Sync.OnAuthorityRequested += OnAuthorityRequested;
        m_Sync.OnStateAuthority.AddListener(OnStateAuthority);
        m_Sync.OnStateRemote.AddListener(OnStateRemote);
        m_Sync.OnAuthorityRequestRejected.AddListener(OnAuthorityRequestRejected);

    }

    

    private void OnDisable()
    {
        m_Sync.OnAuthorityRequested -= OnAuthorityRequested;
        m_Sync.OnStateAuthority.RemoveListener(OnStateAuthority);
        m_Sync.OnStateRemote.RemoveListener(OnStateRemote);
        m_Sync.OnAuthorityRequestRejected.RemoveListener(OnAuthorityRequestRejected);


        
    }
    private void OnAuthorityRequestRejected(AuthorityType arg0)
    {
        OnGrabValidate?.Invoke(false);
    }
    private void OnStateRemote()
    {
        Debug.Log("grabbable OnStateRemote");
    }

    private void OnStateAuthority()
    {
        if(m_GrabRequested)
        {
            m_GrabRequested = false;
            DoGrab();

        }
        else
        {
            m_Rigidbody.isKinematic = false;
            m_Collider.enabled = true;
            m_IsHeld = false;

        }
    }

    private bool OnAuthorityRequested(ClientID requesterID, AuthorityType authorityType, CoherenceSync sync)
    {
        return !m_IsHeld; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TryUse()
    {
        Debug.Log("TryGrab");

        if (m_Sync.HasStateAuthority)
        {
            DoGrab();
        }
        else
        {
           m_GrabRequested = true;
           m_Sync.RequestAuthority(AuthorityType.Full); 


        }
    }

    void DoGrab()
    {
        m_IsHeld = true;
        OnGrabValidate?.Invoke(true);

    }

    public void Grab()
    {
        //supposed to
        
    }

    public virtual void Release()
    {
        m_IsHeld = false; 
    }
}
