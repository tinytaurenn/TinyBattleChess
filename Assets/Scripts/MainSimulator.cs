using Coherence.Connection;
using Coherence.Toolkit;
using PlayerControls;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;


public class MainSimulator : MonoBehaviour
{


    [SerializeField] GameObject m_PlayerGameObject;
    [SerializeField] GameObject m_DummyGameObject;

    CoherenceBridge m_CoherenceBridge;
    CoherenceSync m_Sync; 

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


#if COHERENCE_SIMULATOR || UNITY_EDITOR // DONT FORGET ONLY WORKS IN EDITOR 

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

    public void ResetGame()
    {
        m_TurnNumber = 1;
        SwitchGameState(EGameState.Lobby);


    }

    
    void Start()
    {
        if (!m_Sync.HasStateAuthority) return;
        if (!Coherence.SimulatorUtility.IsSimulator) return; 

        


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

    public List<CoherenceSync> GetAllAlivePlayersSync(bool alive)
    {

        List<CoherenceSync> players = new List<CoherenceSync>();

        foreach (CoherenceSync playerSync in m_PlayerSyncs)
        {
            bool isAlive = playerSync.GetComponent<TinyPlayer>().m_IntPlayerState == 0; 

            if(isAlive == alive)
            {
                players.Add(playerSync);
            }
        }

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
                TeleportAllPlayersToLobby(); 
                break;
            case EPlayState.Shop:
                TeleportAllPlayersToShop(); 
                m_RoundTime.enabled = true;
                m_RoundTimer = m_ShopRoundTime;
                
                break;
            case EPlayState.Fighting:
                TeleportPlayersToBattle();
                m_BattleRoundManager.StartBattleRound(GetAllAlivePlayersSync(true)); 
                
                break;
            case EPlayState.End:
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

    public void NextTurn()
    {
        m_TurnNumber++; 
        SwitchPlayState(EPlayState.Shop);
    }
    #endregion

    #endregion

#endif




}



