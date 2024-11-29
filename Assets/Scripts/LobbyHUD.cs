using Coherence;
using Coherence.Connection;
using Coherence.Toolkit;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyHUD : MonoBehaviour
{
    CoherenceSync m_Sync;
    [SerializeField] TextMeshProUGUI m_LobbyText;
    [SerializeField] Button m_StartButton;



    private void Awake()
    {
        m_Sync = GetComponent<CoherenceSync>();
        m_Sync.CoherenceBridge.onLiveQuerySynced.AddListener(OnLiveQuerySynced);

        m_Sync.OnStateAuthority.AddListener(OnStateAuth);
        m_Sync.OnAuthTransferComplete.AddListener(OnAuthTransfer);
        m_Sync.OnAuthorityRequested += OnAuthRequested;
        m_Sync.OnAuthorityRequestRejected.AddListener(OnAuthRequestRejected);

        m_StartButton.onClick.AddListener(OnStartButtonClicked);

    }

    private void OnStartButtonClicked()
    {
        MainSimulator mainSimulator = FindFirstObjectByType<MainSimulator>(); 
        if (mainSimulator != null)
        {
            mainSimulator.GetComponent<CoherenceSync>().SendCommand<MainSimulator>(nameof(MainSimulator.RefreshPlayerList), MessageTarget.AuthorityOnly);
        }
    }

    private void OnAuthRequestRejected(AuthorityType arg0)
    {
        Debug.Log("Auth request rejected");
        OnStateAuth(); 
    }

    //give authority to the first player that joins the lobby
    private bool OnAuthRequested(ClientID requesterID, AuthorityType authorityType, CoherenceSync sync)
    {
        Debug.Log("Auth requested");
       if(Coherence.SimulatorUtility.IsSimulator) return true;

        return false; 
    }

    private void OnAuthTransfer()
    {
        Debug.Log("Auth transfered");
        OnStateAuth(); 
    }

    private void OnStateAuth()
    {
        Debug.Log("OnStateAuth");

        if(Coherence.SimulatorUtility.IsSimulator)
        {
            if(m_Sync.CoherenceBridge.ClientConnections.GetAllClients().Count() > 0)
            {
                ClientID client =  m_Sync.CoherenceBridge.ClientConnections.GetAllClients().First().ClientId;
                m_Sync.TransferAuthority(client, AuthorityType.Full);
            }
            
        }
        else
        {
            m_LobbyText.text = "Waiting for host";
        }

        if (m_Sync.HasStateAuthority)
        {
            Debug.Log("auth, set to start game");
            m_LobbyText.text = "Start Game";
        }
        else
        {
            Debug.Log("no auth, set to waiting");
            m_LobbyText.text = "Waiting for host";
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
        Debug.Log("Live query synced");
        if (Coherence.SimulatorUtility.IsSimulator) return;

        if (m_Sync.IsOrphaned)
        {
            Debug.Log("Adopting"); 
            m_Sync.Adopt();
        }
        else if (m_Sync.HasStateAuthority)
        {
            Debug.Log("has state auth");
            OnStateAuth(); 
        }
        else
        {
            Debug.Log("requesting from querysync");
            m_Sync.RequestAuthority(AuthorityType.Full);
        }

    }
}
