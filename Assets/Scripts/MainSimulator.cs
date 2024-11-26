using Coherence.Connection;
using Coherence.Toolkit;
using System;
using System.Collections.Generic;
using UnityEngine;


public class MainSimulator : MonoBehaviour
{
    [SerializeField] GameObject m_PlayerGameObject;

    CoherenceBridge m_CoherenceBridge;
    public Dictionary<ClientID, CoherenceSync> m_Players = new();
    [SerializeField] private List<FPlayerStruct> m_FPlayers = new List<FPlayerStruct>();

    [Serializable]
    public struct FPlayerStruct
    {
        public CoherenceSync Sync;
        public GameObject Player;
        public ClientID ClientID; 

        public FPlayerStruct(CoherenceSync sync, ClientID clientID)
        {
            Sync = sync;
            Player = null;
            ClientID = clientID;
        }
    }


#if COHERENCE_SIMULATOR || UNITY_EDITOR

    private void Awake()
    {
        CoherenceBridgeStore.TryGetBridge(gameObject.scene, out m_CoherenceBridge);
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

    private void OnDestroyed(CoherenceClientConnection connection)
    {
        if (m_Players.TryGetValue(connection.ClientId, out var sync))
        {
            m_Players.Remove(connection.ClientId);
        }
    }

    private void OnCreated(CoherenceClientConnection connection)
    {
        if(m_Players.ContainsKey(connection.ClientId))
        {
            return;
        }

        m_Players.Add(connection.ClientId, connection.Sync);
        Debug.Log("added in dictionary on created ");
        m_FPlayers.Add(new FPlayerStruct(connection.Sync, connection.ClientId)); 
    }

    private void OnSynced(CoherenceClientConnectionManager manager)
    {
        foreach (var client in manager.GetAllClients())
        {
            if (m_Players.ContainsKey(client.ClientId))
            {
                continue;
            }
            m_Players.Add(client.ClientId, client.Sync);
            Debug.Log("added in dictionary"); 
            m_FPlayers.Add(new FPlayerStruct(client.Sync, client.ClientId));
            Debug.Log("added " + client.ClientId + " to the dictionary");
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

#endif


    public void SimpleMessage()
    {
        Debug.Log("Simple Message");
    }
}
