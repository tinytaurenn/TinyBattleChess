using Coherence;
using Coherence.Connection;
using Coherence.Toolkit;
using System;
using UnityEngine;

public abstract class Grabbable : Usable
{
    public SO_Item So_Item
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
        //if(this.gameObject != null) Destroy(gameObject);

    }

    protected override void Start()
    {
        base.Start();
        OnStateAuthority();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        m_Sync.OnStateRemote.AddListener(OnStateRemote);
        

    }



    protected override void OnDisable()
    {
        base.OnDisable(); 
        m_Sync.OnStateRemote.RemoveListener(OnStateRemote);


        
    }
    protected override void OnAuthorityRequestRejected(AuthorityType arg0)
    {

        base.OnAuthorityRequestRejected(arg0);
    }
    private void OnStateRemote()
    {
        Debug.Log("grabbable OnStateRemote");
    }

    protected override void OnStateAuthority()
    {
        Debug.Log("grabbable OnStateAuthority");
        if (IsNPCHeld) return;

        if (m_IsHeld)
        {
            if (transform.root != transform)
            {
                if (transform.root.GetComponentInChildren<CoherenceSync>(true) != null)
                {
                    return;
                }
            }
        }

        if(m_UseRequested)
        {
            m_UseRequested = false; 
            DoUse();
        }
        else
        {
            Release(); 

        }
    }

    protected override bool OnAuthorityRequested(ClientID requesterID, AuthorityType authorityType, CoherenceSync sync)
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

        base.TryUse();
    }

    protected override void DoUse()
    {
        m_IsHeld = true;
        base.DoUse();
        //m_Sync.SendCommand<Grabbable>(nameof(Grabbable.EnableComponent), Coherence.MessageTarget.Other,false);

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
        m_Collider.enabled = true;
        m_Rigidbody.isKinematic = false;
        m_IsHeld = false;
        //m_Sync.SendCommand<Grabbable>(nameof(Grabbable.EnableComponent), Coherence.MessageTarget.Other, true); 
        transform.localScale = m_ItemScale;
    }
}
