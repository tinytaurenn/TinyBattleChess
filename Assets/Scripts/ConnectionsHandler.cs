using Coherence;
using Coherence.Cloud;
using Coherence.Connection;
using Coherence.Toolkit;
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

    private void Awake()
    {
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
        
    }

    private void OnDisconnected(CoherenceBridge bridge, ConnectionCloseReason reason)
    {
        if (Coherence.SimulatorUtility.IsSimulator) return;

        

        Destroy(MyPlayer); 
        MyPlayer = null;    
    }

    private void OnConnected(CoherenceBridge bridge)
    {
        Debug.Log("player connected"); 
        
        //m_SimulatorSync.SendCommand<MainSimulator>(nameof(MainSimulator.SimpleMessage), MessageTarget.AuthorityOnly);

        if(Coherence.SimulatorUtility.IsSimulator) return;

        PlayerSpawn();
       
    }

    void PlayerSpawn()
    {
        MyPlayer = Instantiate(m_PlayerPrefab, Vector3.zero, Quaternion.identity);
        MyPlayer.name = "[local] PLAYER";
    }
}
