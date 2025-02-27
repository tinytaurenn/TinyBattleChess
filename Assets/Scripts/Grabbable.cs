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
        set { m_SO_Item = value; }
    }

    [SerializeField] SO_Item m_SO_Item; 

    [Sync] public bool m_IsHeld;
    [SerializeField] protected bool m_IsNPCHeld = false; 
    [Sync] public bool IsNPCHeld
    {
        get { return m_IsHeld && m_IsNPCHeld; }
        set { m_IsNPCHeld = value; }
    }

    public event Action<bool> OnGrabValidate;

    [SerializeField] bool m_GrabRequested = false; 


    [HideInInspector] public Rigidbody m_Rigidbody;
    [HideInInspector] public Collider m_Collider;

    Vector3 m_ItemScale = Vector3.one;


    protected override void Awake()
    {
        base.Awake();
        
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();

        if(m_Sync && m_Sync.CoherenceBridge) m_Sync.CoherenceBridge.onDisconnected.AddListener(OnDisconnected);


        m_ItemScale = transform.localScale;

    }

    protected virtual void OnDisconnected(CoherenceBridge arg0, ConnectionCloseReason arg1)
    {
        //Debug.Log("Grabbable disconnected");
        if(m_Sync.CoherenceBridge != null) m_Sync.CoherenceBridge.onDisconnected.RemoveListener(OnDisconnected);
        if(this.gameObject != null) Destroy(gameObject);

    }

    protected override void Start()
    {
        base.Start();
        OnStateAuthority();
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
        Debug.Log("grabbable OnStateAuthority");
        if (IsNPCHeld) return; 
        if(m_GrabRequested)
        {
            Debug.Log("grabbable grab requested");
            m_GrabRequested = false;
            DoGrab();

        }
        else
        {
            //Debug.Log("grabbable OnStateAuthority");
            m_Collider.enabled = true;
            m_Rigidbody.isKinematic = false;
            m_IsHeld = false;

        }
    }

    private bool OnAuthorityRequested(ClientID requesterID, AuthorityType authorityType, CoherenceSync sync)
    {
        Debug.Log("auth requested, sending : " + !m_IsHeld);
        return !m_IsHeld; 
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
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
            Debug.Log("Requesting auth");


        }
    }

    void DoGrab()
    {
        m_IsHeld = true;
        OnGrabValidate?.Invoke(true);
        m_Sync.SendCommand<Grabbable>(nameof(Grabbable.EnableComponent), Coherence.MessageTarget.Other,false);

    }

    [Command]
    public void EnableComponent(bool enable)
    {
        this.enabled = enable;

    }

    public void Grab()
    {
        //supposed to
        
    }

    public virtual void Release()
    {
        m_IsHeld = false;
        m_Sync.SendCommand<Grabbable>(nameof(Grabbable.EnableComponent), Coherence.MessageTarget.Other, true);
        transform.localScale = m_ItemScale;
    }
}
