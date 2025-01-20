using Coherence;
using Coherence.Connection;
using Coherence.Toolkit;
using System;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LobbyHUD : MonoBehaviour
{
    CoherenceSync m_Sync;
    [SerializeField] TextMeshProUGUI m_LobbyText;
    [SerializeField] Button m_StartButton;
    [SerializeField] Canvas m_GlobalCanvas;
    [SerializeField] Canvas m_HostCanvas;
    [SerializeField] Canvas m_RootCanvas; 

    [SerializeField] Button m_ResetGameButton; 



    private void Awake()
    {
        m_Sync = GetComponent<CoherenceSync>();
        m_Sync.CoherenceBridge.onLiveQuerySynced.AddListener(OnLiveQuerySynced);

        m_Sync.OnStateAuthority.AddListener(OnStateAuth);
        m_Sync.OnAuthTransferComplete.AddListener(OnAuthTransfer);
        m_Sync.OnAuthorityRequested += OnAuthRequested;
        m_Sync.OnAuthorityRequestRejected.AddListener(OnAuthRequestRejected);

        m_StartButton.onClick.AddListener(OnStartButtonClicked);

        m_ResetGameButton.onClick.AddListener(OnResetGameButtonClicked);

    }

  

    private void OnStartButtonClicked()
    {
        if(!m_Sync.HasStateAuthority) return;

        if(ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerState!= TinyPlayer.EPlayerState.Player) return;

        CoherenceSync mainSimulatorSync = Utils.GetSimulatorSync();
        if (mainSimulatorSync != null)
        {
            mainSimulatorSync.SendCommand<MainSimulator>(nameof(MainSimulator.StartGame), MessageTarget.AuthorityOnly);
        }

        HideLobbyHUD(); 
    }

    private void OnResetGameButtonClicked()
    {
        if (!m_Sync.HasStateAuthority) return;

        MainSimulator mainSimulator = Utils.GetSimulator();

        if (mainSimulator == null) return; 

        if (mainSimulator.m_IntGameState == (int)MainSimulator.EGameState.Lobby)
        {
            Debug.Log("Game is in lobby state, cant reset");
            return;
        }
        mainSimulator.GetComponent<CoherenceSync>().SendCommand<MainSimulator>(nameof(MainSimulator.ResetGame), MessageTarget.AuthorityOnly);

        ShowLobbyHUD();

    }

    public void HideLobbyHUD()
    {
        m_GlobalCanvas.enabled = false;
    }
    public void ShowLobbyHUD()
    {
        m_GlobalCanvas.enabled = true;
        
    }

    public void ShowPause(bool show)
    {
        m_RootCanvas.enabled = show;
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
            ShowLobbyHUD();
            m_LobbyText.text = "Start Game";
            m_HostCanvas.enabled = true;
        }
        else
        {
            Debug.Log("no auth, set to waiting");
            m_LobbyText.text = "Waiting for host";
            m_HostCanvas.enabled = false;
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
