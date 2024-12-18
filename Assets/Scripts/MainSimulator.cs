using Coherence.Connection;
using Coherence.Toolkit;
using PlayerControls;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class MainSimulator : MonoBehaviour
{
    [SerializeField] GameObject m_PlayerGameObject;
    [SerializeField] GameObject m_DummyGameObject;

    CoherenceBridge m_CoherenceBridge;
    CoherenceSync m_Sync; 
    public Dictionary<ClientID, CoherenceSync> m_Players = new();
    [SerializeField] private List<GameObject> m_PlayerObjects = new List<GameObject>();

    [SerializeField] TextMeshProUGUI m_PlayerNumberText;

    [SerializeField] Transform m_ShopSpawnPositions;
    [SerializeField] Transform m_DummiesSpawnPositions;

    //test 

    [SerializeField] GameObject SwordGameObject; 
    GameObject MySword = null;

    

    internal enum EPlayState
    {
        Lobby,
        Shop, 
        Fighting,
        End
    }

    internal enum EGameState
    {
        Lobby,
        InGame
    }

    internal EPlayState m_PlayState = EPlayState.Lobby;
    internal EGameState m_GameState = EGameState.Lobby;



#if COHERENCE_SIMULATOR || UNITY_EDITOR // DONT FORGET ONLY WORKS IN EDITOR 

    private void Awake()
    {
        CoherenceBridgeStore.TryGetBridge(gameObject.scene, out m_CoherenceBridge);
        m_Sync = GetComponent<CoherenceSync>();
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
                GameObject dummy = Instantiate(m_DummyGameObject, m_DummiesSpawnPositions.GetChild(i).position, Quaternion.identity);
                
               
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        //m_PlayerNumberText.text = "number of players : " + m_FPlayers.Count;

        //if (Time.time > timer + mytimer)
        //{
        //    timer = Time.time;
        //    RefreshPlayerList();
        //}
    }


    public void RefreshPlayerList()
    {
        Debug.Log("Refreshing player list");
        m_PlayerObjects.Clear();
        foreach (var player in FindObjectsByType<TinyPlayer>(FindObjectsSortMode.None))
        {
            m_PlayerObjects.Add(player.gameObject);


        }

        foreach (var player in m_PlayerObjects)
        {
            Debug.Log(player.name);
        }
    }

    void TeleportAllPlayersToShop()
    {
        if(m_ShopSpawnPositions.childCount == 0 || m_ShopSpawnPositions == null)
        {
            Debug.Log("No spawn positions for shop");
            return;
        }

        Debug.Log("Sending players to shop");

        for (int i = 0; i < m_PlayerObjects.Count; i++)
        {
            CoherenceSync playerSync = m_PlayerObjects[i].GetComponent<CoherenceSync>();
            playerSync.SendCommand<TinyPlayer>(nameof(TinyPlayer.TeleportPlayer), Coherence.MessageTarget.AuthorityOnly, m_ShopSpawnPositions.GetChild(i).position);
        }

    }

    public void StartGame()
    {
        RefreshPlayerList();
        TeleportAllPlayersToShop(); 
    }

#endif




}



