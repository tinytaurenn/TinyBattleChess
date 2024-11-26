using Coherence.Connection;
using Coherence.Entities;
using Coherence.Toolkit;
using PlayerControls;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SceneSimulatorManager : MonoBehaviour
{
    
    public static SceneSimulatorManager Instance { get; private set;  }

    internal CoherenceSync m_CoherenceSync;
    [SerializeField] CoherenceBridge m_CoherenceBridge; 
    bool LiveQuerySynced = false;

    [SerializeField] TextMeshProUGUI m_PlayerNumberText;

    [SerializeField] List<GameObject> m_ConnectedPlayerSyncs = new List<GameObject>();
    [SerializeField] GameObject m_PlayerPrefabTest;


#if COHERENCE_SIMULATOR || UNITY_EDITOR

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        


        m_CoherenceSync = GetComponent<CoherenceSync>();
        CoherenceBridgeStore.TryGetBridge(gameObject.scene, out m_CoherenceBridge);
        m_CoherenceBridge.onLiveQuerySynced.AddListener(OnLiveQuerySynced);

    }

    private void OnEnable()
    {
        if (!Coherence.SimulatorUtility.IsSimulator) return; 
        m_CoherenceSync.CoherenceBridge.ClientConnections.OnCreated += (client) =>
        {
           
            RefreshPlayerList();

            
        };



    }

    private void OnDisable()
    {
        m_CoherenceSync.CoherenceBridge.onLiveQuerySynced.RemoveListener(OnLiveQuerySynced);

        
    }



    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!LiveQuerySynced) return; 


        int clientCount = m_CoherenceBridge.ClientConnections.GetAllClients().Count();

        m_PlayerNumberText.text = "number of players : " + clientCount;
    }
    private void FixedUpdate()
    {
        
    }

    private void OnLiveQuerySynced(CoherenceBridge arg0)
    {
        LiveQuerySynced = true;



    }

#endif

    public void RefreshPlayerList()
    {
        Debug.Log("refreshing player list");



        if(m_CoherenceBridge == null)
        {
            Debug.Log("coherence bridge is null");
            return;
        }

        if(m_CoherenceBridge.ClientConnections.GetAllClients().Count() == 0)
        {
            Debug.Log("no players connected");
            return;
        }

        foreach (var item in m_CoherenceBridge.ClientConnections.GetAllClients())
        {
            Debug.Log(item.Sync.name);
            Debug.Log(item.Sync.gameObject.name);
            Debug.Log(item.ClientId);
            Debug.Log(item.NetworkEntity);
        }
    }

    

}
