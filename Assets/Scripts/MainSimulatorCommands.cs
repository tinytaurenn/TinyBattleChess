using Coherence.Toolkit;
using UnityEngine;

public class MainSimulatorCommands : MonoBehaviour
{

    MainSimulator m_MainSimulator;

    private void Awake()
    {
        m_MainSimulator = GetComponent<MainSimulator>();
    }

    [Command]
    public void StartGame()
    {
        m_MainSimulator.StartGame();
    }
    [Command]
    public void ResetGame()
    {
        m_MainSimulator.ResetGame();
    }
    [Command]
    public void PlayerDeath(CoherenceSync playerSync)
    {
        m_MainSimulator.PlayerDeath(playerSync);
    }
    [Command]
    public void AskForTeleport(CoherenceSync askerSync)
    {
        Vector3 pos =  m_MainSimulator.GetTeleportPoint();
        askerSync.SendCommand<TinyPlayer>(nameof(TinyPlayer.TeleportPlayer), Coherence.MessageTarget.AuthorityOnly, pos);
    }




}
