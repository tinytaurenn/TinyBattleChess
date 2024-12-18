using Coherence.Toolkit;
using PlayerControls;
using UnityEngine;

public class TinyPlayer : MonoBehaviour, IDamageable
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
    PlayerWeapons m_PlayerWeapons;

    [Space(10)]
    [Header("Player sockets")]
    [SerializeField] internal  Transform m_PlayerLeftHandSocket; 
    [SerializeField] internal Transform m_PlayerRightHandSocket;


    private void Awake()
    {
        m_Sync = GetComponent<CoherenceSync>();
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_PlayerControls = GetComponent<PlayerControls.PlayerControls>();
        m_PlayerUse = GetComponent<PlayerUse>();
        m_PlayerWeapons = GetComponent<PlayerWeapons>();



        
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

    #region Hits
    public void TakeMeleeSync(int DirectionNESO, CoherenceSync sync, int damage)
    {
        EWeaponDirection direction = (EWeaponDirection)DirectionNESO;
        EWeaponDirection weaponDirection = m_PlayerWeapons.m_WeaponDirection;
        Debug.Log(" player take melee sync");
        Debug.Log(" strike " + direction.ToString() + " direction!");

        bool parry = false;

        switch (direction)
        {
            case EWeaponDirection.Right:
                parry = weaponDirection == EWeaponDirection.Left;
                break;
            case EWeaponDirection.Left:
                parry = weaponDirection == EWeaponDirection.Right;
                break;
            case EWeaponDirection.Up:
                parry = weaponDirection == EWeaponDirection.Up;
                break;
            case EWeaponDirection.Down:
                parry = weaponDirection == EWeaponDirection.Down;
                break;
        }

        if (m_PlayerWeapons.m_Parrying && parry)
        {
            ParrySync(damage, sync);

        }
        else
        {
            TakeDamageSync(damage, sync);


        }
    }

    public void TakeDamageSync(int damage, CoherenceSync Damagersync)
    {
        Debug.Log("sync Player took " + damage + " damage!");
    }

    public void ParrySync(int damage, CoherenceSync DamagerSync)
    {
        Debug.Log("sync Player parried ");
        Debug.Log(DamagerSync.transform.name + " parried!");
        DamagerSync.SendCommand<PlayerWeapons>(nameof(PlayerWeapons.SyncBlocked), Coherence.MessageTarget.AuthorityOnly);
    }
    #endregion
}
