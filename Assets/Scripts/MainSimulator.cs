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

    CoherenceBridge m_CoherenceBridge;
    public Dictionary<ClientID, CoherenceSync> m_Players = new();
    [SerializeField] private List<GameObject> m_FPlayers = new List<GameObject>();

    [SerializeField] TextMeshProUGUI m_PlayerNumberText;

    float timer = 0;
    float mytimer = 3f; 



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

    }

    // Update is called once per frame
    void Update()
    {
        m_PlayerNumberText.text = "number of players : " + m_FPlayers.Count;

        if(Time.time > timer + mytimer)
        {
            timer = Time.time;
            RefreshPlayerList();
        }
    }


    public void RefreshPlayerList()
    {
        m_FPlayers.Clear();
        foreach (var player in FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None))
        {
            m_FPlayers.Add(player.gameObject);


        }

        foreach (var player in m_FPlayers)
        {
            Debug.Log(player.name);
        }
    }

#endif


}



