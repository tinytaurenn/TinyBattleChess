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
using Coherence.Cloud;
using Unity.VisualScripting;



public class MainSimulator : MonoBehaviour
{


    [SerializeField] GameObject m_PlayerGameObject; 
    [SerializeField] GameObject m_DummyGameObject;

    CoherenceBridge m_CoherenceBridge;
    CoherenceSync m_Sync;

    [SerializeField] float m_SyncUpdateTimer = 0f;
    [SerializeField] float m_SyncUpdateTime = 0.5f;

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
    [Sync] int m_IntArenaChoice = 0; 

    

    


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
        m_CoherenceBridge.onConnected.AddListener(OnConnected);

        SceneManager.sceneLoaded += OnSceneLoaded;






    }

    

    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        m_CoherenceBridge.SceneManager.SetClientScene(scene.buildIndex); 
        m_CoherenceBridge.InstantiationScene = scene;
        

        
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

    private void OnConnected(CoherenceBridge arg0)
    {
        //if (!SimulatorUtility.IsSimulator) return; 
    }

    private void OnDestroyed(CoherenceClientConnection connection)
    {
        RefreshPlayerList();

        //StartCoroutine(UpdateHost());
    }

    private void OnCreated(CoherenceClientConnection connection)
    {
        Debug.Log(" a player is connected");

        //StartCoroutine(UpdateHost());

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
        CheckPlayerList(); 
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
        SyncingNetworkElements(); 

        //Debug.Log("simulator in scene :" + SceneManager.GetActiveScene().name);

        
    }
    void SyncingNetworkElements()
    {
        m_SyncUpdateTimer += Time.deltaTime;
        if (m_SyncUpdateTimer < m_SyncUpdateTime)
        {
            return;
        }
        else
        {
            m_SyncUpdateTimer = 0f;
        }


        CheckPlayerList(); 

    }

    void CheckPlayerList()
    {
        CleanPlayerList();
        int clientCount = m_CoherenceBridge.ClientConnections.GetAllClients().Count();
        

        if (clientCount > m_PlayerSyncs.Count)
        {
            Debug.Log("client count : " + clientCount);
            Debug.Log("not right client connection count, refreshing ");
            RefreshPlayerList();
        }
        else
        {
            //Debug.Log("good connection count  ");
        }

    }

    IEnumerator UpdateHost()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("updating  host ");
        //RefreshPlayerList();
        Debug.Log("player count : " + m_PlayerSyncs.Count);
        foreach (var player in m_PlayerSyncs)
        {
            if(player.gameObject == null )  continue;
            Debug.Log(player.name);

        }
        if (m_PlayerSyncs.Count == 0)
        {
            Debug.Log("No players player list for update");
            yield break;  
        }

        foreach (var player in m_PlayerSyncs)
        {
            if (player == null) continue; 
            if (player.GetComponent<TinyPlayer>().IsHost)
            {
                Debug.Log("a player is already a host ");
                yield break;  
            }

        }
        Debug.Log("sending host become command ");
        
        m_PlayerSyncs[0].SendOrderedCommand<TinyPlayer>(nameof(TinyPlayer.BecomeHost), Coherence.MessageTarget.AuthorityOnly, true);
    }
    internal void AddPlayerSync(CoherenceSync playerSync)
    {
        Debug.Log("adding player to list");

        CleanPlayerList(); 
        if(m_PlayerSyncs.Contains(playerSync))
        {
            Debug.Log("player already in list");
            return; 
        }

        m_PlayerSyncs.Add(playerSync);

        
    }


    public void RefreshPlayerList()
    {
        Debug.Log("Refreshing player list");
        //m_PlayerSyncs.Clear();
        foreach (var player in FindObjectsByType<TinyPlayer>(FindObjectsSortMode.None))
        {
            if(player.TryGetComponent<CoherenceSync>(out CoherenceSync sync)) 
            {
                AddPlayerSync(sync);
            }
        }

        foreach (var player in m_PlayerSyncs)
        {
            Debug.Log(player.name);

        }
        for (int i = 0; i < m_PlayerSyncs.Count; i++)
        {
            m_PlayerSyncs[i].SendCommand<EntityCommands>(nameof(EntityCommands.ChangeGameIDCommand), Coherence.MessageTarget.AuthorityOnly, i);
        }

        StartCoroutine(UpdateHost());
    }

    void CleanPlayerList()
    {
        if(m_PlayerSyncs.Count == 0)
        {
            //Debug.Log("cleaned player list, count is : " + m_PlayerSyncs.Count);
            return; 
        }
        for (int i = 0; i < m_PlayerSyncs.Count; i++)
        {
            if (m_PlayerSyncs[i]== null || m_PlayerSyncs[i].gameObject == null)
            {
                m_PlayerSyncs.RemoveAt(i);
            }
        }
        //Debug.Log("cleaned player list, count is : " + m_PlayerSyncs.Count);
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

        Debug.Log("Sending players to Lobby : player count is " + m_PlayerSyncs.Count);
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
                    if(m_PlayerSyncs[i] == null || m_PlayerSyncs[i].gameObject == null)
                    {
                        Debug.Log("player sync is null in teleportall players to lobby");
                        continue; 
                    }
                    Debug.Log("Sending player to lobby deathmatch : " + m_PlayerSyncs[i].gameObject.name );
                    m_PlayerSyncs[i].SendCommand<TinyPlayer>(nameof(TinyPlayer.LoadToLobby), Coherence.MessageTarget.AuthorityOnly);
                    
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
        CheckPlayerList();
        if (m_PlayState != EPlayState.Lobby)
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
                //RefreshPlayerList();


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



    //#endif




}


#endregion
