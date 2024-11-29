using Coherence;
using Coherence.Connection;
using Coherence.Toolkit;
using System;
using TMPro;
using UnityEngine;

public class LobbyHUD : MonoBehaviour
{
    CoherenceSync m_Sync;
    [SerializeField] TextMeshProUGUI m_LobbyText; 

    private void Awake()
    {
        m_Sync = GetComponent<CoherenceSync>();
        m_Sync.CoherenceBridge.onLiveQuerySynced.AddListener(OnLiveQuerySynced);


        m_Sync.OnStateAuthority.AddListener(OnStateAuth);
        m_Sync.OnAuthTransferComplete.AddListener(OnAuthTransfer);
        m_Sync.OnAuthorityRequested+= OnAuthRequested;
        
        
        
        
    }
    //give authority to the first player that joins the lobby
    private bool OnAuthRequested(ClientID requesterID, AuthorityType authorityType, CoherenceSync sync)
    {
       if(Coherence.SimulatorUtility.IsSimulator) return true;

        return false; 
    }

    private void OnAuthTransfer()
    {
        OnStateAuth(); 
    }

    private void OnStateAuth()
    {
        if (m_Sync.HasStateAuthority)
        {
            m_LobbyText.text = "Host PlayButton";
        }
        else
        {
            m_LobbyText.text = "not host PlayButton";
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnLiveQuerySynced(CoherenceBridge arg0)
    {
        if (Coherence.SimulatorUtility.IsSimulator) return;

        if (m_Sync.IsOrphaned)
        {
            m_Sync.Adopt();
        }
        else if (m_Sync.HasStateAuthority)
        {
            OnStateAuth(); 
        }
        else
        {
            m_Sync.RequestAuthority(AuthorityType.Full);
        }




        m_Sync.CoherenceBridge.onLiveQuerySynced.RemoveListener(OnLiveQuerySynced);
    }
}
