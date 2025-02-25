using Coherence;
using Coherence.Cloud;
using Coherence.Connection;
using Coherence.Toolkit;
using PlayerControls;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionsHandler : MonoBehaviour
{

    

    [SerializeField] CoherenceBridge m_CoherenceBridge;
    [SerializeField] GameObject m_PlayerPrefab;
    [SerializeField] CoherenceSync m_SimulatorSync; 

    GameObject MyPlayer;
    public TinyPlayer LocalTinyPlayer { get; private set; }

    [SerializeField] MainSimulator m_MainSimulator; 

    public MainSimulator MainSimulator
    {
        get
        {
          if(m_MainSimulator == null || !m_MainSimulator.gameObject.activeInHierarchy)
            {
                Debug.Log("MainSimulator is null in connections handler");
                return null; 
            }
            else
            {
                return m_MainSimulator;
            }
        }
        private set { m_MainSimulator = value;  }
    }
    
    public static ConnectionsHandler Instance;

    [SerializeField] float m_SyncUpdateTimer = 0f; 
    [SerializeField] float m_SyncUpdateTime = 1f;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; 
        }
        else
        {
            Destroy(this);
        }

        CoherenceBridgeStore.TryGetBridge(gameObject.scene, out m_CoherenceBridge);
        m_CoherenceBridge.onLiveQuerySynced.AddListener(OnLiveQuerySynced);

    }

    private void OnLiveQuerySynced(CoherenceBridge arg0)
    {
        //m_SimulatorSync = FindFirstObjectByType<MainSimulator>().GetComponent<CoherenceSync>();
        m_CoherenceBridge.onLiveQuerySynced.RemoveListener(OnLiveQuerySynced);
    }

    private void OnEnable()
    {
        

        m_CoherenceBridge.onConnected.AddListener(OnConnected);
        m_CoherenceBridge.onDisconnected.AddListener(OnDisconnected);
        
    }
    private void OnDisable()
    {
        m_CoherenceBridge.onConnected.RemoveListener(OnConnected);
        m_CoherenceBridge.onDisconnected.RemoveListener(OnDisconnected);
    }

    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SyncingNetworkElements(); 
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

        if (MainSimulator == null)
        {
            if (Utils.GetSimulator(out MainSimulator simulator))
            {
                MainSimulator = simulator;
            }
            else
            {
                Debug.Log("main simulator not found in sync update");
            }
        }

    }

    private void OnDisconnected(CoherenceBridge bridge, ConnectionCloseReason reason)
    {
        if (Coherence.SimulatorUtility.IsSimulator) return;

        

        Destroy(MyPlayer); 
        MyPlayer = null;    
        LocalTinyPlayer = null;
    }

    private void OnConnected(CoherenceBridge bridge)
    {
        Debug.Log("player connected"); 
        
        //m_SimulatorSync.SendCommand<MainSimulator>(nameof(MainSimulator.SimpleMessage), MessageTarget.AuthorityOnly);

        if(Coherence.SimulatorUtility.IsSimulator) return;

 
        PlayerSpawn();
        //SyncAll(); 
       
    }

    void PlayerSpawn()
    {
        MyPlayer = Instantiate(m_PlayerPrefab, Vector3.zero, Quaternion.identity);
        MyPlayer.name = "[local] PLAYER";
        LocalTinyPlayer = MyPlayer.GetComponent<TinyPlayer>();
        CameraManager.Instance.m_PlayerTransform = MyPlayer.transform; 
    }

    void SyncAll()
    {
        foreach (var item in FindObjectsByType<PlayerSync>(FindObjectsSortMode.None))
        {
            item.Sync(); 
        }
    }

 
    public void ChangePlayState(int oldState, int newState)
    {
        Debug.Log("changing play state from connections handler");
      LocalTinyPlayer.OnChangePlayState(oldState, newState);
    }
     
    public void ChangeGameState(int oldState, int newState)
    {
        LocalTinyPlayer.OnChangeGameState(oldState, newState);
    }
}
