using Coherence;
using Coherence.Connection;
using Coherence.Toolkit;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyHUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_LobbyText;
    [SerializeField] Button m_StartButton;
    [SerializeField] Canvas m_GlobalCanvas;
    [SerializeField] Canvas m_HostCanvas;
    [SerializeField] Canvas m_RootCanvas; 

    [SerializeField] Button m_ResetGameButton; 




    private void Awake()
    {
        m_StartButton.onClick.AddListener(OnStartButtonClicked);
        m_ResetGameButton.onClick.AddListener(OnResetGameButtonClicked);
    }


    private void OnStartButtonClicked()
    {
        Debug.Log("start buttonclick");
        if (!ConnectionsHandler.Instance.LocalTinyPlayer.IsHost) return;

        if(ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerState!= TinyPlayer.EPlayerState.Player) return;


        if(Utils.GetSimulatorLocal(out MainSimulator simulator))
        {
            Debug.Log("start game");
            simulator.Sync.SendCommand<MainSimulatorCommands>(nameof(MainSimulatorCommands.StartGame), MessageTarget.AuthorityOnly);
        }


        HideLobbyHUD(); 
    }

    private void OnResetGameButtonClicked()
    {
        if (!ConnectionsHandler.Instance.LocalTinyPlayer.IsHost) return;

        if(Utils.GetSimulatorLocal(out MainSimulator simulator))
        {
            simulator.Sync.SendCommand<MainSimulatorCommands>(nameof(MainSimulatorCommands.ResetGame), MessageTarget.AuthorityOnly);
        }
        else
        {
            return; 
        }


        ShowLobbyHUD();

    }

    public void HideLobbyHUD()
    {
        m_GlobalCanvas.enabled = false;
        m_GlobalCanvas.gameObject.SetActive(false);
    }
    public void ShowLobbyHUD()
    {
        m_GlobalCanvas.enabled = true;
        m_GlobalCanvas.gameObject.SetActive(true);
        
    }

    public void ShowPause(bool show)
    {
        LocalUI.Instance.m_LobbyHUD.UpdateLobbyHud();
        m_RootCanvas.enabled = show;
        m_RootCanvas.gameObject.SetActive(show); 
    }
    


    public void UpdateLobbyHud()
    {
        if (ConnectionsHandler.Instance.LocalTinyPlayer.IsHost)
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

}
