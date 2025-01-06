using Coherence.Toolkit;
using PlayerControls;
using UnityEngine;


public class TinyPlayer : MonoBehaviour, IDamageable
{
    public enum EPlayerState
    {
        Player = 0 ,
        Spectator = 1,
        Disqualified = 2

    }

    internal EPlayerState m_PlayerState = EPlayerState.Player;
    [OnValueSynced(nameof(SyncOnChangePlayerState))] public int m_IntPlayerState = 0;

    CoherenceSync m_Sync;
    PlayerMovement m_PlayerMovement;
    Collider m_Collider; 
    PlayerControls.PlayerControls m_PlayerControls;
    PlayerUse m_PlayerUse;
    PlayerWeapons m_PlayerWeapons;
    Ragdoll m_RagDoll;
    PlayerFX m_PlayerFX; 

    [SerializeField] GameObject m_PlayerModel;
    [SerializeField] PlayerAnimEvents m_PlayerAnimEvents; 

    [Space(10)]
    [Header("Player Stats")]
    [Sync] public  int m_Global_Health = 100;
    [Sync] public int m_Player_Health = 100; 
    public bool m_IsBeaten = false;
    public bool m_IsStunned = false;
    public float m_StunTimer = 0;
    

    [Space(10)]
    [Header("Player sockets")]
    [SerializeField] internal  Transform m_PlayerLeftHandSocket; 
    [SerializeField] internal Transform m_PlayerRightHandSocket;

    [Space(10)]
    [Header("Player Ghost")]
    [SerializeField] GameObject m_PlayerGhost;


    private void Awake()
    {

        m_Sync = GetComponent<CoherenceSync>();
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_Collider = GetComponent<Collider>();
        m_PlayerControls = GetComponent<PlayerControls.PlayerControls>();
        m_PlayerUse = GetComponent<PlayerUse>();
        m_PlayerWeapons = GetComponent<PlayerWeapons>();
        m_RagDoll = GetComponent<Ragdoll>();
        m_PlayerFX = GetComponent<PlayerFX>();



        
    }

    private void Update()
    {
        
        UpdatePlayerState();
    }
    //commands

    MainSimulator GetSimulator()
    {
        MainSimulator simulator = FindFirstObjectByType<MainSimulator>(FindObjectsInactive.Exclude);
        if (simulator == null)
        {
            Debug.Log("simulator not found");
            return null; 
        }

        return simulator; 
    }
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
        m_IntPlayerState = (int)playerState;
        OnEnterPlayerState();

        m_PlayerControls.SwitchState();

    }

    void OnEnterPlayerState()
    {
        switch (m_PlayerState)
        {
            case EPlayerState.Player:
                break;
            case EPlayerState.Spectator:
                m_PlayerGhost.SetActive(true);
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
                EnablePlayer(false);

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
                StunUpdate(); 
                break;
            case EPlayerState.Spectator:
                break;
            default:
                break;
        }
    }

    #region Hits
    public void TakeMeleeSync(int DirectionNESO, CoherenceSync sync, int damage,Vector3 attackerPos)
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

        if (m_PlayerWeapons.m_Parrying && parry && m_PlayerWeapons.IsInParryAngle(attackerPos))
        {
            ParrySync(damage, sync);

        }
        else
        {
            TakeWeaponDamageSync(damage, sync); 


        }
    }

    public void TakeWeaponDamageSync(int damage, CoherenceSync Damagersync)
    {
        Debug.Log("sync Player took " + damage + " weapon damage!");
        m_Player_Health -= damage;

        m_PlayerFX.PlayHurtFX(0);
        m_Sync.SendCommand<PlayerFX>(nameof(PlayerFX.PlayHurtFX), Coherence.MessageTarget.Other, 0);

        HitStun();

        if (m_Player_Health <= 0)
        {
            m_Player_Health = 0;
            Debug.Log("must die now");
            Damagersync.SendCommand<PlayerWeapons>(nameof(PlayerWeapons.SyncHit), Coherence.MessageTarget.AuthorityOnly);//sound
            PlayerDeath(); 
        }
        else
        {
            Debug.Log("sending weapon synchit comand ");
            Damagersync.SendCommand<PlayerWeapons>(nameof(PlayerWeapons.SyncHit), Coherence.MessageTarget.AuthorityOnly);
        }


    }

    public void TakeDamageSync(int damage, CoherenceSync Damagersync)
    {
        Debug.Log("sync Player took " + damage + " damage!");
        m_Player_Health -= damage;
        m_PlayerFX.PlayHurtFX(0); 
        m_Sync.SendCommand<PlayerFX>(nameof(PlayerFX.PlayHurtFX), Coherence.MessageTarget.Other, 0);

        HitStun(); 
        
        if (m_Player_Health <= 0)
        {
            m_Player_Health = 0;
            Debug.Log("must die now"); 
            
            PlayerDeath();
        }
        else
        {
            Debug.Log("sending synchit comand ");
            
        }


    }

    public void ParrySync(int damage, CoherenceSync DamagerSync)
    {
        Debug.Log("sync Player parried ");
        Debug.Log(DamagerSync.transform.name + " parried!");
        DamagerSync.SendCommand<PlayerWeapons>(nameof(PlayerWeapons.SyncBlocked), Coherence.MessageTarget.AuthorityOnly);

        int soundVariationIndex = UnityEngine.Random.Range(0, 3);
        m_Sync.SendCommand<PlayerFX>(nameof(PlayerFX.PlayParryFX), Coherence.MessageTarget.All, soundVariationIndex); 
    } 

    public void HitStun()
    {
        TimedStun(0.6f); 
        

    }

    void Stun()
    {
        m_IsStunned = true; 
        m_PlayerMovement.Stun();
        if (m_PlayerWeapons.m_MainWeapon != null) m_PlayerWeapons.m_MainWeapon.ActivateDamage(false);
        if (m_PlayerWeapons.m_SecondaryWeapon != null) m_PlayerWeapons.m_SecondaryWeapon.ActivateDamage(false);

    }

    public void TimedStun(float time)
    {
        Stun();
        m_StunTimer = time;
    } 

    void StunUpdate()
    {
        if (m_StunTimer > 0)
        {
            m_StunTimer -= Time.deltaTime;
            if (m_StunTimer <= 0)
            {
                m_IsStunned = false;
                m_PlayerMovement.UnStun();
                m_StunTimer = 0;
            }
        }
    }

    void EnablePlayer(bool Enabled)
    {
        if (Enabled)
        {

        }
        else
        {
            m_PlayerMovement.StopMovement();
            m_PlayerAnimEvents.StopRunParticles();
        }
        m_PlayerModel.SetActive(Enabled);
        m_Collider.enabled = Enabled;
        m_PlayerWeapons.enabled = Enabled;
        //m_PlayerControls.enabled = Enabled;
        m_PlayerMovement.enabled = Enabled;
        m_PlayerUse.enabled = Enabled;

        
    }

    public void EnableSyncElements(bool Enabled)
    {
        if (Enabled)
        {

        }
        else
        {
            m_PlayerMovement.StopMovement();
            m_PlayerAnimEvents.StopRunParticles();
        }
        m_Collider.enabled = Enabled;
        m_PlayerModel.SetActive(Enabled);
    }


    public void SyncOnChangePlayerState(int oldState, int NewState) 
    {

        switch (NewState)
        {
            case 0:
                EnableSyncElements(true);
                break;
            case 1:
                EnableSyncElements(false);
                break;
            case 2:
                EnableSyncElements(false);
                break;
            default:
                break;
        }

    }
    public void PlayerDeath()
    {
        if (!m_Sync.HasStateAuthority) return; 
        Debug.Log("player death");
        m_PlayerWeapons.Drop(); 

        

        m_RagDoll.SpawnRagDoll(); 

        SwitchPlayerState(EPlayerState.Spectator);
        

    }
    public void OnChangePlayState(int oldIntPlayState,int NewIntPlayState)
    {
        switch (NewIntPlayState)
        {
            case 0: // Lobby
                Debug.Log("lobby, stopping PVP ");
                break;
            case 1: //shop 
                Debug.Log("Shop, stopping PVP ");

                break;
            case 2: //fighting
                
                break;
            case 3: //end 
                
                break;
            default:
                break;
        }
    }

    public void OnChangeGameState(int OldIntGamestate,int NewIntGameState)
    {
        switch (NewIntGameState)
        {
            case 0: //lobby
                break;
            case 1: //InGame
                break;

            default:
                break;
        }
    }
    #endregion
}
