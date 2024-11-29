using Coherence.Toolkit;
using PlayerControls;
using UnityEngine;

public class TinyPlayer : MonoBehaviour
{
    public enum EPlayerState
    {
        Player,
        Spectator
    }

    internal EPlayerState m_PlayerState = EPlayerState.Player;

    CoherenceSync m_Sync;
    PlayerMovement m_PlayerMovement;
    PlayerControls.PlayerControls m_PlayerControls;
    PlayerUse m_PlayerUse; 

    private void Awake()
    {
        m_Sync = GetComponent<CoherenceSync>();
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PlayerControls = GetComponent<PlayerControls.PlayerControls>();
        m_PlayerUse = GetComponent<PlayerUse>();


        
    }
    //commands
    public void TeleportPlayer(Vector3 worldPos)
    {
        Debug.Log("teleporting to : " + worldPos);
        transform.position = worldPos;
    }

    public void SwitchPlayerState(EPlayerState playerState)
    {
        if(m_PlayerState == playerState) return;
        OnExitPlayerState();
        m_PlayerState = playerState;
        OnEnterPlayerState();

    }

    void OnEnterPlayerState()
    {
        switch (m_PlayerState)
        {
            case EPlayerState.Player:
                break;
            case EPlayerState.Spectator:
                break;
            default:
                break;
        }
    }

    void OnExitPlayerState()
    {
        switch (m_PlayerState)
        {
            case EPlayerState.Player:
                break;
            case EPlayerState.Spectator:
                break;
            default:
                break;
        }


    }

    void UpdatePlayerState()
    {
        switch (m_PlayerState)
        {
            case EPlayerState.Player:
                break;
            case EPlayerState.Spectator:
                break;
            default:
                break;
        }
    }
}
