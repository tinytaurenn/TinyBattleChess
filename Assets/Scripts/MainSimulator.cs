using Coherence.Connection;
using Coherence.Toolkit;
using PlayerControls;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Coherence;



public class MainSimulator : MonoBehaviour
{


    [SerializeField] GameObject m_PlayerGameObject; 
    [SerializeField] GameObject m_DummyGameObject;

    CoherenceBridge m_CoherenceBridge;
    CoherenceSync m_Sync; 

    public CoherenceSync Sync => m_Sync;

    [SerializeField] private List<CoherenceSync> m_PlayerSyncs = new List<CoherenceSync>();

    [Space(10)]
    [Header("Timers")]

    [SerializeField] TextMeshProUGUI m_RoundTime;
    [SerializeField] float m_ShopRoundTime = 90f; 
    float m_RoundTimer = 0f;

    [Space(10)]
    [Header("Transforms")]

    [SerializeField] Transform m_DummiesSpawnPositions;
    //test 
    [Space(10)]
    [Header("Testings")]
    [SerializeField] GameObject SwordGameObject; 
    GameObject MySword = null;


    [Space(10)]
    [Header("BattleRound")]
    BattleRoundManager m_BattleRoundManager;

    [Space(10)]
    [Header("DeathMatch Options ")]
    [Sync]
    public float m_RespawnTime = 5f;


    public enum EGameMode
    {
        AutoChess, 
        DeathMatch
    }

    public enum EPlayState
    {
        Lobby,
        Shop, 
        Fighting,
        End
    }

    public enum EGameState
    {
        Lobby,
        InGame
    }
    [Space(10)]
    [Header("GameMode")]
    [SerializeField] EGameMode m_GameMode = EGameMode.AutoChess;
    [Sync] public int m_IntGameMode = 0;


    [Space(10)]
    [Header("Turns and States")] 


    [SerializeField] internal EPlayState m_PlayState = EPlayState.Lobby;
    [SerializeField] internal EGameState m_GameState = EGameState.Lobby;

    [Sync][OnValueSynced(nameof(PlayStateValueSync))] public int m_IntPlayState = 0;
    [Sync][OnValueSynced(nameof(GameStateValueSync))] public int m_IntGameState = 0;
    [Sync]public int m_TurnNumber = 1;


    //run on every synced players
    public void PlayStateValueSync(int oldValue, int newValue)
    {
        Debug.Log("new value is " + (EPlayState)newValue);
        ConnectionsHandler.Instance.ChangePlayState(oldValue, newValue); 
    }
    //run on every synced players
    public void GameStateValueSync(int oldValue, int newValue)
    {
        Debug.Log("new value is " + (EGameState)newValue);
        ConnectionsHandler.Instance.ChangeGameState(oldValue, newValue);
    }


//#if COHERENCE_SIMULATOR || UNITY_EDITOR // DONT FORGET ONLY WORKS IN EDITOR 

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        CoherenceBridgeStore.TryGetBridge(gameObject.scene, out m_CoherenceBridge);
        m_Sync = GetComponent<CoherenceSync>();
        m_BattleRoundManager = GetComponent<BattleRoundManager>();
        m_CoherenceBridge.onLiveQuerySynced.AddListener(OnLiveQuerySynced);
        m_CoherenceBridge.onDisconnected.AddListener(OnDisconnected);




    }


    private void OnEnable()
    {
        m_CoherenceBridge.ClientConnections.OnSynced += OnSynced;
        m_CoherenceBridge.ClientConnections.OnCreated += OnCreated;
        m_CoherenceBridge.ClientConnections.OnDestroyed += OnDestroyed;

    }


    private void OnDisable()
    {
        m_CoherenceBridge.ClientConnections.OnSynced -= OnSynced; 
        m_CoherenceBridge.ClientConnections.OnCreated -= OnCreated;
        m_CoherenceBridge.ClientConnections.OnDestroyed -= OnDestroyed;

        Debug.Log("main simulator OnDisable");
    }

    private void OnLiveQuerySynced(CoherenceBridge arg0)
    {
        
        
    }

    private void OnDisconnected(CoherenceBridge arg0, ConnectionCloseReason arg1)
    {


        if(MySword != null)
        {
            Debug.Log("destroying sword"); 
            Destroy(MySword);
        }
    }

    private void OnDestroyed(CoherenceClientConnection connection)
    {
        //RefreshPlayerList();
    }

    private void OnCreated(CoherenceClientConnection connection)
    {
        //RefreshPlayerList();

    }

    private void OnSynced(CoherenceClientConnectionManager manager)
    {
        
        //RefreshPlayerList();
    }

    IEnumerator DelayedReset(float time)
    {
        yield return new WaitForSeconds(time);
        ResetGame();
    }


    public void ResetGame()
    {
        m_TurnNumber = 1;
        SwitchGameState(EGameState.Lobby);


    }

    
    void Start()
    {
        if (!m_Sync.HasStateAuthority) return;
        if (!Coherence.SimulatorUtility.IsSimulator) return;

        //StartCoroutine(SpamSomething()); 
        
        m_IntGameMode = (int)m_GameMode;


        if (MySword == null)
        {
            MySword =  Instantiate(SwordGameObject, transform.position, Quaternion.identity);
        }

        if(m_DummiesSpawnPositions.childCount >0)
        {
            for (int i = 0; i < m_DummiesSpawnPositions.childCount; i++)
            {
                Quaternion angle = Quaternion.Euler(0, 180, 0); 
                GameObject dummy = Instantiate(m_DummyGameObject, m_DummiesSpawnPositions.GetChild(i).position, angle);
                
               
            }
        }


    }

    // Update is called once per frame
    void Update()
    {

        if (!m_Sync.HasStateAuthority) return;
        if (!Coherence.SimulatorUtility.IsSimulator) return;

        UpdateGameState();

        Debug.Log("simulator in scene :" + SceneManager.GetActiveScene().name);

        
    }


    public void RefreshPlayerList()
    {
        Debug.Log("Refreshing player list");
        m_PlayerSyncs.Clear();
        foreach (var player in FindObjectsByType<TinyPlayer>(FindObjectsSortMode.None))
        {
            m_PlayerSyncs.Add(player.GetComponent<CoherenceSync>());

            


        }

        foreach (var player in m_PlayerSyncs)
        {
            Debug.Log(player.name);

        }
        for (int i = 0; i < m_PlayerSyncs.Count; i++)
        {
            m_PlayerSyncs[i].SendCommand<EntityCommands>(nameof(EntityCommands.ChangeGameIDCommand), Coherence.MessageTarget.AuthorityOnly, i);
        }
    }

    public List<CoherenceSync> GetAllPlayersSyncByState(TinyPlayer.EPlayerState playerState)
    {
        List<CoherenceSync> players = new List<CoherenceSync>();
        foreach (CoherenceSync playerSync in m_PlayerSyncs)
        {
            if(playerSync.GetComponent<TinyPlayer>().m_IntPlayerState == (int)playerState)
            {
                players.Add(playerSync);
            }
        }

        return players;
    }

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param> player is 0, ghost is 1, disqualified is 2 
    /// <returns></returns>
    public List<CoherenceSync> GetAllStateSync(int state)
    {
        List<CoherenceSync> players = new List<CoherenceSync>();

        Debug.Log("getAllStateSync player count = " + m_PlayerSyncs.Count); 

        foreach (CoherenceSync playerSync in m_PlayerSyncs)
        {
            Debug.Log(playerSync.GetComponent<TinyPlayer>().m_IntPlayerState); 

            if (playerSync.GetComponent<TinyPlayer>().m_IntPlayerState == state)
            {
                players.Add(playerSync);
            }
        }
        Debug.Log("getAllStateSync "+ state + "  count = " + players.Count);

        return players;
    }



    void TeleportAllPlayersToShop()
    {
        if(SCENE_MANAGER.Instance.ShopSpawnPos.childCount == 0 || SCENE_MANAGER.Instance.ShopSpawnPos == null) 
        {
            Debug.Log("No spawn positions for shop");
            return;
        }

        Debug.Log("Sending players to shop");

        for (int i = 0; i < m_PlayerSyncs.Count; i++)
        {
            //if ((m_PlayerObjects[i].GetComponent<TinyPlayer>().m_IntPlayerState) != ((int)TinyPlayer.EPlayerState.Player))
            //{
            //    continue; 
            //}
            //CoherenceSync playerSync = .GetComponent<CoherenceSync>();
            m_PlayerSyncs[i].SendCommand<TinyPlayer>(nameof(TinyPlayer.TeleportPlayer), Coherence.MessageTarget.AuthorityOnly, SCENE_MANAGER.Instance.ShopSpawnPos.GetChild(i).position);
        }

    }

    void TeleportAllPlayersToLobby()
    {
        for (int i = 0; i < m_PlayerSyncs.Count; i++)
        {
            switch (m_GameMode)
            {
                case EGameMode.AutoChess:
                    //CoherenceSync playerSync = .GetComponent<CoherenceSync>();
                    m_PlayerSyncs[i].SendCommand<TinyPlayer>(nameof(TinyPlayer.TeleportPlayer), Coherence.MessageTarget.AuthorityOnly, SCENE_MANAGER.Instance.LobbyPos.position);
                    m_PlayerSyncs[i].SendCommand<TinyPlayer>(nameof(TinyPlayer.ResetPlayerStats), Coherence.MessageTarget.AuthorityOnly);
                    break;
                case EGameMode.DeathMatch:
                    //CoherenceSync playerSync = .GetComponent<CoherenceSync>();

                    m_PlayerSyncs[i].SendCommand<TinyPlayer>(nameof(TinyPlayer.LoadToLobby), Coherence.MessageTarget.AuthorityOnly);
                    //m_PlayerSyncs[i].SendOrderedCommand<TinyPlayer>(nameof(TinyPlayer.ResetPlayerStats), Coherence.MessageTarget.AuthorityOnly);
                    //m_PlayerSyncs[i].SendOrderedCommand<TinyPlayer>(nameof(TinyPlayer.TeleportPlayer), Coherence.MessageTarget.AuthorityOnly, SCENE_MANAGER.Instance.LobbyPos.position);
                    break;
                default:
                    break;
            }

            
        }
    }

    void TeleportPlayersToBattle()
    {

        Debug.Log("Sending players to Battle");

        for (int i = 0; i < m_PlayerSyncs.Count; i++)
        {
            switch (m_GameMode)
            {
                case EGameMode.AutoChess:
                    m_PlayerSyncs[i].SendCommand<TinyPlayer>(nameof(TinyPlayer.TeleportPlayer), Coherence.MessageTarget.AuthorityOnly, SCENE_MANAGER.Instance.BattleSpawnPos.GetChild(i).position);
                    break;
                case EGameMode.DeathMatch:
                    m_PlayerSyncs[i].SendCommand<TinyPlayer>(nameof(TinyPlayer.LoadToArena),Coherence.MessageTarget.AuthorityOnly);
                    
                    break; 
                default:
                    break; 
            }
            
        }
    }
    public Vector3 GetTeleportPoint()
    {
        Vector3 pos = SCENE_MANAGER.Instance.LobbyPos.transform.position; 
        switch (m_GameMode)
        {
            case EGameMode.AutoChess:
               
                break;
            case EGameMode.DeathMatch:
                Vector3 deathMatchPos = SCENE_MANAGER.Instance.BigArenaBattleSpawnPos.GetChild(UnityEngine.Random.Range(0, SCENE_MANAGER.Instance.BigArenaBattleSpawnPos.childCount)).position;
                return deathMatchPos; 
               
            default:
                break;
        }
        return pos; 
    }

    public void ReviveGhostPlayers()
    {
        foreach (var player in m_PlayerSyncs)
        {
            if(player.GetComponent<TinyPlayer>().m_IntPlayerState == 1  )
            {
                player.SendCommand<TinyPlayer>(nameof(TinyPlayer.SwitchPlayerState), Coherence.MessageTarget.AuthorityOnly,0);
            }
           
        }
    }

    public void ReviveAllPlayers()
    {
        foreach (var player in m_PlayerSyncs)
        {
            if (player.GetComponent<TinyPlayer>().m_IntPlayerState != 0)
            {
                player.SendCommand<TinyPlayer>(nameof(TinyPlayer.SwitchPlayerState), Coherence.MessageTarget.AuthorityOnly, 0);
            }

        }
    }

    public void StartGame()
    {
        if(m_PlayState != EPlayState.Lobby)
        {
            Debug.Log("Game already started");
            return;
        }

        if(m_GameState != EGameState.Lobby)
        {
            Debug.Log("Game already started");
            return;
        }

        SwitchGameState(EGameState.InGame);

        
    }

    public void PlayerDeath(CoherenceSync playerSync)
    {
        switch (m_GameMode)
        {
            case EGameMode.AutoChess:
                m_BattleRoundManager.PlayerDeath(playerSync);
                break;
            case EGameMode.DeathMatch:
                break;
            default:
                break;
        }

       
    }

    void ShopRoundTimeUpdate()
    {
        if (m_RoundTimer > 0)
        {
            m_RoundTimer -= Time.deltaTime;
        }
        else
        {
            m_RoundTimer = 0;
            SwitchPlayState(EPlayState.Fighting);
        }
        m_RoundTime.text = m_PlayState.ToString() + " : " + (int)m_RoundTimer;

    }


    #region StateMachine
    #region GameState StateMachine
    public void SwitchGameState(EGameState gameState)
    {
        OnExitGameState(); 
        m_GameState = gameState;
        m_IntGameState = (int)gameState;
        OnEnterGameState(); 
    }
    void OnEnterGameState()
    {
        Debug.Log("Entering game state" + m_GameState.ToString());
        switch (m_GameMode)
        {
            case EGameMode.AutoChess:
                OnEnterAutoChessGameState();
                break;
            case EGameMode.DeathMatch:
                OnEnterDeathMatchGameState();

                break;
            default:
                break;
        }
    }

    void OnEnterAutoChessGameState()
    {
        switch (m_GameState)
        {
            case EGameState.Lobby:
                SwitchPlayState(EPlayState.Lobby);

                break;
            case EGameState.InGame:
                RefreshPlayerList();

                TeleportAllPlayersToShop();

                SwitchPlayState(EPlayState.Shop);
                break;
            default:
                break;
        }
    }

    void OnEnterDeathMatchGameState()
    {
        switch (m_GameState)
        {
            case EGameState.Lobby:
                SwitchPlayState(EPlayState.Lobby);

                break;
            case EGameState.InGame:
                RefreshPlayerList();


                SwitchPlayState(EPlayState.Fighting);
                break;
            default:
                break;
        }
    }
    public void OnExitGameState()
    {
        switch (m_GameState)
        {
            case EGameState.Lobby:
                break;
            case EGameState.InGame:
                break;
            default:
                break;
        }
    }

    void UpdateGameState()
    {
        switch (m_GameState)
        {
            case EGameState.Lobby:
                break;
            case EGameState.InGame:
                UpdatePlayState();
                break;
            default:
                break;
        }
    }
    #endregion

    #region PlayState StateMachine

    public void SwitchPlayState(EPlayState playState)
    {
        OnExitPlayState();
        m_PlayState = playState;
        m_IntPlayState = (int)playState;
        OnEnterPlayState();
    }

    void OnEnterPlayState()
    {
        Debug.Log("Entering play state" + m_PlayState.ToString());
        switch (m_GameMode)
        {
            case EGameMode.AutoChess:
                OnEnterAutoChessPlayState(); 
                break;
            case EGameMode.DeathMatch:
                OnEnterDeathMatchPlayState();
                break;
            default:
                break;
        }
    }
    void OnEnterDeathMatchPlayState()
    {
        Debug.Log("Entering play state" + m_PlayState.ToString());
        switch (m_PlayState)
        {
            case EPlayState.Lobby:
                RefreshPlayerList();
                
                TeleportAllPlayersToLobby();
                StartCoroutine(LoadSceneRoutine(0));
                break;
            case EPlayState.Shop:
                TeleportAllPlayersToShop();
                CleanAllCleanables();
                m_RoundTime.enabled = true;
                m_RoundTimer = m_ShopRoundTime;

                break;
            case EPlayState.Fighting:
                TeleportPlayersToBattle();
                StartCoroutine(LoadSceneRoutine(1)); 

                break;
            case EPlayState.End:
                Debug.Log("end of the game");
                StartCoroutine(DelayedReset(5));
                break;
            default:
                break;
        }
    }

    void OnEnterAutoChessPlayState()
    {
        Debug.Log("Entering play state" + m_PlayState.ToString());
        switch (m_PlayState)
        {
            case EPlayState.Lobby:
                ReviveAllPlayers();
                TeleportAllPlayersToLobby();
                break;
            case EPlayState.Shop:
                TeleportAllPlayersToShop();
                CleanAllCleanables();
                m_RoundTime.enabled = true;
                m_RoundTimer = m_ShopRoundTime;

                break;
            case EPlayState.Fighting:
                TeleportPlayersToBattle();
                m_BattleRoundManager.StartBattleRound(GetAllStateSync(0));

                break;
            case EPlayState.End:
                Debug.Log("end of the game");
                StartCoroutine(DelayedReset(5));
                break;
            default:
                break;
        }
    }

    void OnExitPlayState()
    {
        switch (m_GameMode)
        {
            case EGameMode.AutoChess:
                OnExitAutoChessPlayState();
                break;
            case EGameMode.DeathMatch:
                OnExitDeathMatchPlayState();
                break;
            default:
                break;
        }
    }
    void OnExitDeathMatchPlayState()
    {
        switch (m_PlayState)
        {
            case EPlayState.Lobby:
                break;
            case EPlayState.Shop:
                break;
            case EPlayState.Fighting:


                break;
            case EPlayState.End:
                break;
            default:
                break;
        }
    }

    void OnExitAutoChessPlayState()
    {
        switch (m_PlayState)
        {
            case EPlayState.Lobby:
                break;
            case EPlayState.Shop:
                m_RoundTime.enabled = false;
                break;
            case EPlayState.Fighting:


                break;
            case EPlayState.End:
                break;
            default:
                break;
        }
    }

    void UpdatePlayState()
    {
       

        switch (m_PlayState)
        {
            case EPlayState.Lobby:
                break;
            case EPlayState.Shop:
                ShopRoundTimeUpdate();
                break;
            case EPlayState.Fighting:
                break;
            case EPlayState.End:
                break;
            default:
                break;

        }
    }

    public void EndTurn()
    {

        //40 hp flat for now 

        List<CoherenceSync> playerSync = GetAllStateSync(1); //ghosts 
        Debug.Log("ghost in the game : " + playerSync.Count);
        foreach (var player in playerSync)
        {
            player.SendCommand<TinyPlayer>(nameof(TinyPlayer.TakeGlobalDamage), Coherence.MessageTarget.AuthorityOnly, 40);
        }

        StartCoroutine(DelayedNextTurn());  
    }

    IEnumerator DelayedNextTurn()
    {
        yield return new WaitForSeconds(2);
        NextTurn(); 
    }

    public void NextTurn()
    {
        List<CoherenceSync> disqualifiedSyncs = GetAllStateSync(2); //ghosts
        Debug.Log("get all disqualified players : " + disqualifiedSyncs.Count);
        Debug.Log("get all players for turn check : " + m_PlayerSyncs.Count);

        if(disqualifiedSyncs.Count >= ( m_PlayerSyncs.Count -1))
        {
            SwitchPlayState(EPlayState.End);
            return;
        }
        ReviveGhostPlayers(); 

        m_TurnNumber++;

        SwitchPlayState(EPlayState.Shop);
    }

    void CleanAllCleanables()
    {
        foreach (ICleanable cleanable in FindObjectsByType(typeof(MonoBehaviour), FindObjectsSortMode.None).OfType<ICleanable>())
        {
            cleanable.CleanObject(); 
        }
    }




    #endregion

    private IEnumerator LoadSceneRoutine(int sceneIndex)
    {
        Debug.Log("loadscene in simulator"); 
        CoherenceSync[] bringAlong = new CoherenceSync[] { Sync };
        yield return CoherenceSceneManager.LoadScene(m_CoherenceBridge, sceneIndex, bringAlong);

    }

    #endregion

    //#endif




}



