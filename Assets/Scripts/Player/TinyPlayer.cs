using Coherence;
using Coherence.Cloud;
using Coherence.Toolkit;
using PlayerControls;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;


public class TinyPlayer : Entity, IDamageable
{
    public enum EPlayerState
    {
        Player = 0 ,
        Spectator = 1,
        Disqualified = 2

    }

    [SerializeField] internal EPlayerState m_PlayerState = EPlayerState.Player;
    [OnValueSynced(nameof(SyncOnChangePlayerState))] public int m_IntPlayerState = 0;

    MainSimulator.EGameMode m_Gamemode = MainSimulator.EGameMode.AutoChess;

    [SerializeField] bool m_IsHost = false;
    [Sync]
    public bool IsHost
    {
        get { return m_IsHost; } 
        set { 
            m_IsHost = value;
            
        }
    }

    CoherenceSync m_Sync;
    

    public CoherenceSync Sync => m_Sync;
    public PlayerMovement m_PlayerMovement;
    PlayerGhostMovement m_GhostMovement;
    Collider m_Collider; 
    internal PlayerControls.PlayerControls m_PlayerControls;
    PlayerUse m_PlayerUse;
    public PlayerWeapons m_PlayerWeapons;
    Ragdoll m_RagDoll;
    PlayerFX m_PlayerFX; 
    public PlayerLoadout m_PlayerLoadout;
    public Animator m_Animator; 

    [SerializeField] GameObject m_PlayerModel;
    [SerializeField] PlayerAnimEvents m_PlayerAnimEvents;

    [Space(10)]
    [Header("Player Stats")]
    [Sync] [SerializeField] int m_Global_Health = 100;


    [Space(10)]
    [Header("DeathMatch Options ")]

    [SerializeField] float m_RespawnTimer = 0f; 
    float m_RespawnTime = 10f;

   
    public int GlobalHealth { 
        get { return m_Global_Health;  }
        set {
            OnChangeGlobalPlayerHealth(m_Global_Health, value);
            m_Global_Health = value;
        }
    }
    public override int EntityHealth { 
        get { return m_EntityHealth;  }
        set { 
            OnChangePlayerHealth(m_EntityHealth, value);
            m_EntityHealth = value; 
        }
    }


    public bool m_IsBeaten = false;
    public bool m_IsStunned = false;
    public float m_StunTimer = 0;
    

    [Space(10)]
    [Header("Player Ghost")]
    [SerializeField] GameObject m_PlayerGhost;

    [Space(10)]
    [Header("Player Gold")]

    [SerializeField] int m_PlayerGold = 10;
    [SerializeField] bool m_CanUseGold = false;

    public bool CanUseGold { get { return m_CanUseGold; } set { m_CanUseGold = value; } }

    public int PlayerGold
    {
        get { return m_PlayerGold; }
        set {
            value = Mathf.Clamp(value, 0, 9999);
            m_PlayerGold = value; 
            LocalUI.Instance.UpdateGoldAmount(m_PlayerGold); 
            }
    }

    [Space(10)]
    [Header("Player Battle")]
    [SerializeField] int m_BattleIndex = -1; 
    public int BattleIndex { get { return m_BattleIndex; } set { m_BattleIndex = value; } }

    private void Awake()
    {

        m_Sync = GetComponent<CoherenceSync>();
        m_PlayerMovement = GetComponent<PlayerMovement>();
        m_GhostMovement = GetComponent<PlayerGhostMovement>();
        m_Collider = GetComponent<Collider>();
        m_PlayerControls = GetComponent<PlayerControls.PlayerControls>();
        m_PlayerUse = GetComponent<PlayerUse>();
        m_PlayerWeapons = GetComponent<PlayerWeapons>();
        m_RagDoll = GetComponent<Ragdoll>();
        m_PlayerFX = GetComponent<PlayerFX>();
        m_PlayerLoadout = GetComponent<PlayerLoadout>();




        
    }

    private void Update()
    {
        
        UpdatePlayerState();
    }

    
    private void Start()
    {
        RefreshPlayerUI();

        

    }
    //commands


    public bool CanPlayerUseInventoryItem(bool inAttackReady = false, bool inParry = false)
    {
        if(m_Animator.GetBool("Seated")) return false;
        if(m_PlayerState != EPlayerState.Player) return false;
        if(m_IsStunned) return false;
        if(m_PlayerWeapons.InAttackRelease) return false;
        
        if (m_PlayerWeapons.UsingMagic) return false;
        if (m_PlayerWeapons.Throwing) return false;

        if (m_PlayerWeapons.InAttackRelease) return false;




        if (inAttackReady == false && m_PlayerWeapons.InAttackReady) return false;
        if(inParry == false && m_PlayerWeapons.InParry) return false;

        return true; 
    }
    [Command]
    public void BecomeHost(bool isHost)
    {
        IsHost = isHost;
        
    }

    [Command]
    public void TeleportPlayer(Vector3 worldPos)
    {
        Debug.Log("teleporting to : " + worldPos);
        transform.position = worldPos;
    }
    [Command]
    public void LoadToArena()
    {
        ConnectionsHandler.Instance.LoadArena(); 
    }
    [Command]
    public void LoadToLobby()
    {
        ResetPlayerStats();
        SwitchPlayerState(0); 
        ConnectionsHandler.Instance.LoadLobby();
    }
    [Command]
    public void SyncSimulatorScene(int sceneIndex)
    {
        Debug.Log("sync simulator in tinyplayer ");
        if (SceneManager.GetActiveScene().buildIndex == sceneIndex)
        {
            Debug.Log("same scene then the simulator"); 

        }

        switch (sceneIndex)
        {
            case 0:
                LoadToLobby();
                break;
            case 1:
                LoadToArena();
                break; 

            default:
                break;
        }
    }



    public void SwitchPlayerState(EPlayerState playerState)
    {
        if(m_PlayerState == playerState) return;
        OnExitPlayerState();
        m_PlayerState = playerState;
        m_IntPlayerState = (int)playerState;
        OnEnterPlayerState();


    }

    public void SwitchPlayerState(int intPlayerState) 
    {
        //Player = 0 ,
        //Spectator = 1,
        //Disqualified = 2

        if (m_PlayerState ==(EPlayerState) intPlayerState) return;
        OnExitPlayerState();
        m_PlayerState = (EPlayerState)intPlayerState;
        m_IntPlayerState = intPlayerState;
        OnEnterPlayerState();

    }

    void OnEnterPlayerState()
    {
        if(Utils.GetSimulatorLocal(out MainSimulator simulator))
        {
            m_Gamemode = (MainSimulator.EGameMode) simulator.m_IntGameMode;
        }
        switch (m_Gamemode)
        {
            case MainSimulator.EGameMode.AutoChess:
                OnEnterStateAutoChess();
                break;
            case MainSimulator.EGameMode.DeathMatch:
                OnEnterStateDeathMatch();
                break;
            default:
                break;
        }

       
    }
    void OnEnterStateDeathMatch()
    {
        bool simulatorIsOn = (Utils.GetSimulatorLocal(out MainSimulator simulator)); 
        
        switch (m_PlayerState)
        {
            case EPlayerState.Player:
                if (simulatorIsOn)
                {
                    simulator.Sync.SendCommand<MainSimulatorCommands>(nameof(MainSimulatorCommands.AskForTeleport), Coherence.MessageTarget.AuthorityOnly, m_Sync);
                }
                EnablePlayer(true);
                m_PlayerControls.SwitchState(PlayerControls.PlayerControls.EControlState.Player);
                m_PlayerGhost.SetActive(false);
                SeeGhosts(false);
                EntityHealth = 100;
                break;
            case EPlayerState.Spectator:
                m_PlayerControls.SwitchState(PlayerControls.PlayerControls.EControlState.Ghost);
                m_PlayerGhost.SetActive(true);
                SeeGhosts(true);
                m_PlayerLoadout.UnloadEquippedStuff();
                if(simulatorIsOn)
                {
                    m_RespawnTime = simulator.m_RespawnTime; 
                }

                
                break;
            case EPlayerState.Disqualified:
                m_PlayerControls.SwitchState(PlayerControls.PlayerControls.EControlState.Ghost);
                m_PlayerGhost.SetActive(true);
                SeeGhosts(true);
                break;
            default:
                break;
        }
    }

    void OnEnterStateAutoChess()
    {
        switch (m_PlayerState)
        {
            case EPlayerState.Player:
                EnablePlayer(true);
                m_PlayerControls.SwitchState(PlayerControls.PlayerControls.EControlState.Player);
                m_PlayerGhost.SetActive(false);
                SeeGhosts(false);
                EntityHealth = 100;
                break;
            case EPlayerState.Spectator:
                m_PlayerControls.SwitchState(PlayerControls.PlayerControls.EControlState.Ghost);
                m_PlayerGhost.SetActive(true);
                SeeGhosts(true);
                m_PlayerLoadout.UnloadEquippedStuff();
                break;
            case EPlayerState.Disqualified:
                m_PlayerControls.SwitchState(PlayerControls.PlayerControls.EControlState.Ghost);
                m_PlayerGhost.SetActive(true);
                SeeGhosts(true);
                break;
            default:
                break;
        }
    }

    void OnExitPlayerState()
    {
        switch (m_Gamemode)
        {
            case MainSimulator.EGameMode.AutoChess:
                OnExitPlayerStateAutoChess();
                break;
            case MainSimulator.EGameMode.DeathMatch:
                OnExitPlayerStateDeathMatch();
                break;
            default:
                break;
        }


    }
    void OnExitPlayerStateDeathMatch()
    {
        switch (m_PlayerState)
        {
            case EPlayerState.Player:
                EnablePlayer(false);

                break;
            case EPlayerState.Spectator:
                break;
            case EPlayerState.Disqualified:
                break;
            default:
                break;
        }


    }
    void OnExitPlayerStateAutoChess()
    {
        switch (m_PlayerState)
        {
            case EPlayerState.Player:
                EnablePlayer(false);

                break;
            case EPlayerState.Spectator:
                break;
            case EPlayerState.Disqualified:
                break;
            default:
                break;
        }


    }

    void UpdatePlayerState()
    {
        switch (m_Gamemode)
        {
            case MainSimulator.EGameMode.AutoChess:
                UpdatePlayerStateAutoChess();
                break;
            case MainSimulator.EGameMode.DeathMatch:
                UpdatePlayerStateDeathMatch();
                break;
            default:
                break;
        }
    }
    void UpdatePlayerStateDeathMatch()
    {
        switch (m_PlayerState)
        {
            case EPlayerState.Player:
                StunUpdate();
                break;
            case EPlayerState.Spectator:
                m_RespawnTimer += Time.deltaTime;
                if (m_RespawnTimer >= m_RespawnTime)
                {
                    m_RespawnTimer = 0f;
                    SwitchPlayerState(EPlayerState.Player); 
                }
                break;
            case EPlayerState.Disqualified:
                break;
            default:
                break;
        }
    }
    void UpdatePlayerStateAutoChess()
    {
        switch (m_PlayerState)
        {
            case EPlayerState.Player:
                StunUpdate();
                break;
            case EPlayerState.Spectator:
                break;
            case EPlayerState.Disqualified:
                break;
            default:
                break;
        }
    }

    #region Hits
    public override void TakeMeleeSync(int DirectionNESO, CoherenceSync sync, int damage,EEffectType damageType, Vector3 attackerPos)
    {

        if (m_PlayerState != EPlayerState.Player)
        {
            Debug.Log("player is not alive");
            return;
        }

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

        if (m_PlayerWeapons.InShieldParry) parry = true; 

        if (m_PlayerWeapons.InParry && parry && m_PlayerWeapons.IsInParryAngle(attackerPos))
        {
            ParrySync(damage, sync);

        }
        else
        {
            TakeWeaponDamageSync(damage,damageType, sync); 


        }
    }

    public override void TakeWeaponDamageSync(int damage,EEffectType damageType, CoherenceSync Damagersync)
    {

        Debug.Log("sync Player took " + damage + " weapon damage!");

        m_PlayerFX.PlayHurtFX(0);
        Sync.SendCommand<PlayerFX>(nameof(PlayerFX.PlayHurtFX), Coherence.MessageTarget.Other, 0);
        Damagersync.SendCommand<EntityCommands>(nameof(EntityCommands.SyncHitCommand), Coherence.MessageTarget.AuthorityOnly); //sound 
        
        TakeDamageSync(damage, damageType, Damagersync);


    }

    public override void TakeDamageSync(int damage,EEffectType damageType, CoherenceSync Damagersync)
    {


        Debug.Log("sync Player took " + damage + " damage!");
        m_PlayerFX.PlayHurtFX(0); 
        Sync.SendCommand<PlayerFX>(nameof(PlayerFX.PlayHurtFX), Coherence.MessageTarget.Other, 0);
        //
        //armors calculations
        //
        damage =  m_PlayerLoadout.DamageReduction(damage, Damagersync);
        Debug.Log("damage type is " + damageType.ToString()); 

        Debug.Log("sync Player took " + damage + " reduced damage!");


        //
        HitStun();

        if (Utils.GetSimulatorLocal(out MainSimulator simulator))
        {
            if (simulator.m_IntPlayState != (int)MainSimulator.EPlayState.Fighting)
            {
                Debug.Log("in lobby, no damage taken");
                return;
            }
        }



        if (m_PlayerState != EPlayerState.Player)
        {
            Debug.Log("player is not alive");
            return;
        }

        Debug.Log("simulator not found, game is not hosted");

        EntityHealth -= damage;

        if (EntityHealth <= 0)
        {
            EntityHealth = 0;
            Debug.Log("must die now");

            EntityDeath();
        }
        else
        {
            Debug.Log("sending weapon synchit comand ");

        }


    }

    public void TakeGlobalDamage(int damage)
    {
        Debug.Log("Taking global damage");
        GlobalHealth -= damage;
        if (GlobalHealth <= 0)
        {
            GlobalHealth = 0;
            Debug.Log("global health is 0, player disqualified");
            SwitchPlayerState(EPlayerState.Disqualified);
        }
    }

    public override void ParrySync(int damage, CoherenceSync DamagerSync)
    {
        Debug.Log("sync Player parried ");
        Debug.Log(DamagerSync.transform.name + " parried!");
        DamagerSync.SendCommand<EntityCommands>(nameof(EntityCommands.SyncBlockedCommand), Coherence.MessageTarget.AuthorityOnly);

        int soundVariationIndex = UnityEngine.Random.Range(0, 3);
       
        BasicWeapon basicWeapon = m_PlayerWeapons.InShieldParry ? (BasicWeapon)m_PlayerLoadout.m_EquippedItems[EStuffSlot.SecondaryWeapon] : (BasicWeapon)m_PlayerLoadout.m_EquippedItems[EStuffSlot.MainWeapon]; 
        if(basicWeapon != null)
        {
            basicWeapon.PlayParryFX(soundVariationIndex);
            if(basicWeapon.TryGetComponent<CoherenceSync>(out CoherenceSync sync))
            {
                sync.SendCommand<BasicWeapon>(nameof(BasicWeapon.PlayParryFX), Coherence.MessageTarget.Other, soundVariationIndex);
            }
        }
    }

    public override void SyncBlocked()
    {
        m_PlayerWeapons.SyncBlocked();
    }

    public override void SyncHit()
    {
        m_PlayerWeapons.SyncHit();
    }

    public void HitStun()
    {
        TimedStun(0.6f); 
        

    }

    public override void Stun()
    {
        m_IsStunned = true; 
        m_PlayerMovement.Stun();
        if (m_PlayerWeapons.GetMainWeapon() != null) m_PlayerWeapons.GetMainWeapon().ActivateDamage(false);
        if (m_PlayerWeapons.GetSecondaryWeapon() != null) m_PlayerWeapons.GetSecondaryWeapon().ActivateDamage(false);

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
    /// <summary>
    /// local function to see or not see other ghosts when you die in the game
    /// </summary>
    void SeeGhosts(bool See)
    {
        TinyPlayer[] players = FindObjectsByType<TinyPlayer>(FindObjectsSortMode.None);

        foreach (var player in players)
        {
            if (player.m_IntPlayerState > 0) player.m_PlayerGhost.SetActive(See); 
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
        m_GhostMovement.enabled = !Enabled;
        
        m_PlayerUse.enabled = Enabled;
        m_PlayerLoadout.enabled = Enabled;

        
    }

    public void PlayerVisible(bool Visible)
    {
        if (Visible)
        {

        }
        else
        {
            //m_PlayerMovement.StopMovement();
            m_PlayerAnimEvents.StopRunParticles();
        }
        m_Collider.enabled = Visible;
        m_PlayerModel.SetActive(Visible);
    }

    public void ResetPlayerStats()
    {
        GlobalHealth = 100;
        EntityHealth = 100;
        PlayerGold = 10;
    }

    void OnChangePlayerHealth(int oldHealh, int newHealth)
    {
        Debug.Log("player health changed from " + oldHealh + " to " + newHealth);
        if (Sync==null || !Sync.HasStateAuthority) return; // event triggered from all
        LocalUI.Instance.UpdatePlayerHealthSlider(newHealth);
    }

    void OnChangeGlobalPlayerHealth(int oldGlobal, int newGlobal)
    {
        Debug.Log("global player health changed from " + oldGlobal + " to " + newGlobal);
        if (Sync == null || !Sync.HasStateAuthority) return; // event triggered from all
        LocalUI.Instance.UpdateGlobalHealthSlider(newGlobal);
    }

    void RefreshPlayerUI()
    {
        LocalUI.Instance.UpdatePlayerHealthSlider(EntityHealth);
        LocalUI.Instance.UpdateGlobalHealthSlider(GlobalHealth);
        LocalUI.Instance.UpdateGoldAmount(PlayerGold);
    }

    /// <summary>
    /// Important function, called when another player change its state
    /// </summary>
    /// <param name="oldState"></param>
    /// <param name="NewState"></param>
    public void SyncOnChangePlayerState(int oldState, int NewState) 
    {
        //REMINDER
        // 0 = player
        // 1 = spectator
        // 2 = disqualified
        if(ConnectionsHandler.Instance.LocalTinyPlayer == null) return;


        //exit 
        switch (oldState)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                break;
        }
        //enter 
        switch (NewState)
        {
            case 0:
                PlayerVisible(true);
                m_PlayerGhost.SetActive(false);
                break;
            case 1:
                PlayerVisible(false);
                if(ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerState == EPlayerState.Spectator || ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerState == EPlayerState.Disqualified)
                {
                    Debug.Log("local player mode is spectator, enabling ghost"); 
                    m_PlayerGhost.SetActive(true);
                }
                break;
            case 2:
                PlayerVisible(false);
                if (ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerState == EPlayerState.Spectator || ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerState == EPlayerState.Disqualified)
                {
                    Debug.Log("local player mode is spectator, enabling ghost");
                    m_PlayerGhost.SetActive(true);
                }
                break;
            default:
                break;
        }

    }
    public override void EntityDeath()
    {
        if (!m_Sync.HasStateAuthority) return; 
        Debug.Log("player death");
        m_PlayerWeapons.Drop(); 

        

        m_RagDoll.SpawnRagDoll(); 

        SwitchPlayerState(EPlayerState.Spectator);

        if (Utils.GetSimulatorLocal(out MainSimulator simulator))
        {
            simulator.Sync.SendCommand<MainSimulatorCommands>(nameof(MainSimulatorCommands.PlayerDeath), Coherence.MessageTarget.AuthorityOnly, m_Sync);
        }
        else
        {
            return;
        }


    }

    public override bool GetAttackState(out EWeaponDirection attackDir)
    {
        
        attackDir = m_PlayerWeapons.m_WeaponDirection;

        return m_PlayerWeapons.InAttackReady;

    }

    public override void OnReceiveAttackState(bool isAttacking, EWeaponDirection attackDir)
    {
        
        throw new System.NotImplementedException();
    }

    //battle manager related 

    public void ChangeBattleIndex(int index)
    {
        BattleIndex = index;    
    }

    public void OnChangePlayState(int oldIntPlayState,int NewIntPlayState)
    {

        switch (oldIntPlayState)
        {
            case 0: // Lobby
                Debug.Log("exit lobby  ");
                break;
            case 1: //shop 
                Debug.Log("exit Shop ");
                LocalUI.Instance.ShowShopRelated(false);
                if(m_PlayerControls.m_ControlState == PlayerControls.PlayerControls.EControlState.Selecting)
                {
                    LocalUI.Instance.CloseSelection(); 
                }
                CanUseGold = false;

                break;
            case 2: //fighting
                Debug.Log("exit Fighting ");
                EntityHealth = 100;
                

                break;
            case 3: //end 

                break;
            default:
                break;
        }


        switch (NewIntPlayState)
        {
            case 0: // Lobby
                Debug.Log("lobby, stopping PVP ");
                m_PlayerLoadout.UnloadEquippedStuff();
                m_PlayerLoadout.ClearLoadout(); 
                FindFirstObjectByType<LobbyHUD>(FindObjectsInactive.Exclude).ShowLobbyHUD();
                break;
            case 1: //shop 
                Debug.Log("Shop, stopping PVP ");
                PlayerGold = 10; 
                m_PlayerLoadout.UnloadEquippedStuff();
                LocalUI.Instance.ShowShopRelated(true); 
                CanUseGold = true;

                break;
            case 2: //fighting
                m_PlayerLoadout.EquipLoadout(); 


                
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

    #region Effects
    public override void HealingEffect(float value)
    {

        EntityHealth += (int)value;
    }

    public override void RegenerationEffect(float value, float duration)
    {
        Debug.Log("Regeneration");
    }

    public override void StrengthEffect(float value, float duration)
    {
        base.StrengthEffect(value, duration);
    }

    public override void SpeedEffect(float value, float duration)
    {
        base.SpeedEffect(value, duration);
    }

    public override void AttackSpeedEffect(float value, float duration)
    {
        base.AttackSpeedEffect(value, duration);
    }

    public override void JumpHeightEffect(float value, float duration)
    {
        base.JumpHeightEffect(value, duration);
    }

    public override void FlyEffect(float value, float duration)
    {
        base.FlyEffect(value, duration);
    }

    public override void ParryEffect(float value, float duration)
    {
        base.ParryEffect(value, duration);
    }

    public override void InvisibilityEffect(float value, float duration)
    {
        base.InvisibilityEffect(value, duration);
    }

    public override void DamageEffect(float value,EEffectType damageType)
    {
        Debug.Log("player Damage from effect");
        TakeDamageSync((int)value, damageType, m_Sync);
    }

    public override void PoisonEffect(float value, float duration)
    {
        base.PoisonEffect(value, duration);
    }

    public override void FireEffect(float value, float duration)
    {
        base.FireEffect(value, duration);
    }

    public override void SlowEffect(float value, float duration)
    {
        base.SlowEffect(value, duration);
    }

    public override void BlindEffect(float value, float duration)
    {
        base.BlindEffect(value, duration);
    }

    public override void GroundedEffect(float value, float duration)
    {
        base.GroundedEffect(value, duration);
    }

    public override void WeaknessEffect(float value, float duration)
    {
        base.WeaknessEffect(value, duration);
    }



    #endregion
}
