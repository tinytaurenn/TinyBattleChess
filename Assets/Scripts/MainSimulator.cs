using Coherence.Connection;
using Coherence.Toolkit;
using PlayerControls;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



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

    [SerializeField] Transform m_ShopSpawnPositions;
    [SerializeField] Transform m_BattleSpawnPositions;
    [SerializeField] Transform m_DummiesSpawnPositions;
    [SerializeField] Transform m_LobbyPos; 
    //test 
    [Space(10)]
    [Header("Testings")]
    [SerializeField] GameObject SwordGameObject; 
    GameObject MySword = null;


    [Space(10)]
    [Header("BattleRound")]
    BattleRoundManager m_BattleRoundManager;

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
    [Header("Turns and States")]

    internal EPlayState m_PlayState = EPlayState.Lobby;
    internal EGameState m_GameState = EGameState.Lobby;

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
        if(m_ShopSpawnPositions.childCount == 0 || m_ShopSpawnPositions == null) 
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
            m_PlayerSyncs[i].SendCommand<TinyPlayer>(nameof(TinyPlayer.TeleportPlayer), Coherence.MessageTarget.AuthorityOnly, m_ShopSpawnPositions.GetChild(i).position);
        }

    }

    void TeleportAllPlayersToLobby()
    {
        for (int i = 0; i < m_PlayerSyncs.Count; i++)
        {
           
            //CoherenceSync playerSync = .GetComponent<CoherenceSync>();
            m_PlayerSyncs[i].SendCommand<TinyPlayer>(nameof(TinyPlayer.TeleportPlayer), Coherence.MessageTarget.AuthorityOnly, m_LobbyPos.position);
            m_PlayerSyncs[i].SendCommand<TinyPlayer>(nameof(TinyPlayer.ResetPlayerStats), Coherence.MessageTarget.AuthorityOnly);
        }
    }

    void TeleportPlayersToBattle()
    {
        if (m_ShopSpawnPositions.childCount == 0 || m_ShopSpawnPositions == null)
        {
            Debug.Log("No spawn positions for Battle");
            return;
        }

        Debug.Log("Sending players to Battle");

        for (int i = 0; i < m_PlayerSyncs.Count; i++)
        {
            
            m_PlayerSyncs[i].SendCommand<TinyPlayer>(nameof(TinyPlayer.TeleportPlayer), Coherence.MessageTarget.AuthorityOnly, m_BattleSpawnPositions.GetChild(i).position);
        }
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
 
        m_BattleRoundManager.PlayerDeath(playerSync);
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

    #endregion

//#endif




}



