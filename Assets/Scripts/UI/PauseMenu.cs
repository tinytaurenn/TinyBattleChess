using Coherence.Samples.WorldDialog;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PauseMenu
{

    [SerializeField] WorldDialogUI m_WorldDialogUI;
    [SerializeField] Canvas m_PauseCanvas; 

    [SerializeField] internal bool m_InPauseMenu = false;
    public LobbyHUD m_LobbyHUD;

    [Space(10)]
    [Header("Buttons")]

    [SerializeField] GameObject m_MenuButtons;
    [SerializeField] GameObject m_LobbyButtons;
    [SerializeField] GameObject m_SettingsButtons;

    public Button m_LobbyButton;
    public Button m_SettingsButton;
    public Button[] m_ReturnButton;
    public Slider m_SensivitySlider;
    public TextMeshProUGUI m_SliderText; 

    
    public void TogglePause()
    {
        m_InPauseMenu = !m_InPauseMenu;

        m_PauseCanvas.enabled = m_InPauseMenu;
        m_MenuButtons.SetActive(m_InPauseMenu);
        m_LobbyButtons.SetActive(!m_InPauseMenu);
        m_SettingsButtons.SetActive(!m_InPauseMenu);

        m_LobbyHUD.ShowPause(m_InPauseMenu);

        Cursor.visible = m_InPauseMenu;
        Cursor.lockState = CursorLockMode.Confined;

        m_WorldDialogUI.ShowDiconnectDialog(m_InPauseMenu);
    }

    public void Disconnect()
    {
        m_WorldDialogUI.Disconnect();
        TogglePause();
        Cursor.visible = true;
    }

    public void OnReturnButton()
    {
        ReturnToMenu(); 
    }

    public void OnLobbyButton()
    {
        m_MenuButtons.SetActive(false);
        m_LobbyButtons.SetActive(true);
        m_SettingsButtons.SetActive(false);
    }
    public void OnSettingsButton()
    {
        m_MenuButtons.SetActive(false);
        m_LobbyButtons.SetActive(false);
        m_SettingsButtons.SetActive(true);
    }

    void ReturnToMenu()
    {
        m_MenuButtons.SetActive(true);
        m_LobbyButtons.SetActive(false);
        m_SettingsButtons.SetActive(false);


        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    internal void OnSensivitySliderChange(float value)
    {
        if(ConnectionsHandler.Instance.LocalTinyPlayer != null)
        {
            ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerMovement.m_MouseSensivity = value;
            m_SliderText.text = "Sensivity : " + value.ToString("F2");
        }
    }
}
