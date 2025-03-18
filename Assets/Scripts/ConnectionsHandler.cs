using Coherence;
using Coherence.Cloud;
using Coherence.Connection;
using Coherence.Toolkit;
using PlayerControls;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionsHandler : MonoBehaviour
{



    [SerializeField] CoherenceBridge m_CoherenceBridge ; 
    [SerializeField] CoherenceLiveQuery m_CoherenceQuery; 
    [SerializeField] GameObject m_PlayerPrefab;
    [SerializeField] CoherenceSync m_SimulatorSync; 


    [SerializeField] TinyPlayer m_TinyPlayer;
    public TinyPlayer LocalTinyPlayer => m_TinyPlayer;  

    [SerializeField] MainSimulator m_MainSimulator; 


    public MainSimulator Main_Simulator
    {
        
        get
        {
          if(m_MainSimulator == null || !m_MainSimulator.gameObject.activeInHierarchy)
            {
                //Debug.Log("MainSimulator is null in connections handler");
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

    [Header("Data Storage")]

    public CloudStorage m_CloudStorage => m_CoherenceBridge.CloudService.GameServices.CloudStorage;
    public StorageObjectId SceneToLoad = (0, 0); //value , objectID



    private void Awake()
    {


        if (Instance == null)
        {
            Instance = this; 
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);



        CoherenceBridgeStore.TryGetBridge(gameObject.scene, out m_CoherenceBridge);
        

        var scene = m_CoherenceBridge.gameObject.scene;
        DontDestroyOnLoad(m_CoherenceBridge);
        m_CoherenceBridge.InstantiationScene = scene;
        m_CoherenceQuery.BridgeResolve += _ => m_CoherenceBridge;
        CoherenceSync.BridgeResolve += _ => m_CoherenceBridge;


        m_CoherenceBridge.onLiveQuerySynced.AddListener(OnLiveQuerySynced);
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    

    private void OnLiveQuerySynced(CoherenceBridge arg0)
    {
        //m_SimulatorSync = FindFirstObjectByType<MainSimulator>().GetComponent<CoherenceSync>();
        //int sceneInt = await m_CoherenceBridge.CloudService.GameServices.CloudStorage.LoadObjectAsync(m_MainSimulator.SceneToLoad);

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

        if (Main_Simulator == null)
        {
            if (Utils.GetSimulator(out MainSimulator simulator))
            {
                Main_Simulator = simulator;

            }
            else
            {
                //Debug.Log("main simulator not found in sync update");
            }
        }

    }

    private void OnDisconnected(CoherenceBridge bridge, ConnectionCloseReason reason)
    {
        if (Coherence.SimulatorUtility.IsSimulator) return;

        Destroy(LocalTinyPlayer.gameObject);     
        m_TinyPlayer = null;

        SceneManager.LoadScene(0);
    }

    private void OnConnected(CoherenceBridge bridge)
    {
        Debug.Log("player connected"); 
        
        //m_SimulatorSync.SendCommand<MainSimulator>(nameof(MainSimulator.SimpleMessage), MessageTarget.AuthorityOnly);

        if(Coherence.SimulatorUtility.IsSimulator) return;

        StartCoroutine(SpawnSync(m_CloudStorage.LoadObjectAsync<int>(SceneToLoad)));

        
        //SyncAll(); 
       
    }

    IEnumerator SpawnSync(StorageOperation<int> operation)
    {
        yield return operation;
        Debug.Log("operation result : " + operation.Result.ToString());
        PlayerSpawn();
        yield return new WaitUntil(() => LocalTinyPlayer != null);

        //if (Main_Simulator == null) yield break; 

        if (operation.Result != SceneManager.GetActiveScene().buildIndex)
        {
            StartCoroutine(LoadSceneRoutine(operation.Result));
        }

       



    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("scene has loaded"); 
        m_CoherenceBridge.SceneManager.SetClientScene(scene.buildIndex);
        m_CoherenceBridge.InstantiationScene = scene;
        //LocalUI.Instance.m_LobbyHUD = FindFirstObjectByType<LobbyHUD>(FindObjectsInactive.Exclude); 
    }

    public void LoadArena()
    {
        StartCoroutine(LoadSceneRoutine(1));
    }
    public void LoadLobby()
    {
        Debug.Log("connections handler: loading lobby");
        StartCoroutine(LoadSceneRoutine(0));
    }
    private IEnumerator LoadSceneRoutine(int sceneIndex)
    {

        Debug.Log("ConnectionHandler: Loading Scene " + sceneIndex.ToString());

        CoherenceSync[] bringAlong = LocalTinyPlayer == null ? new CoherenceSync[0] : new CoherenceSync[] { LocalTinyPlayer.Sync };

        yield return CoherenceSceneManager.LoadScene(m_CoherenceBridge, sceneIndex, bringAlong);
        if(LocalTinyPlayer == null ||SimulatorUtility.IsSimulator) yield break;

        yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == sceneIndex);
        switch (SCENE_MANAGER.Instance.ScenePlayState)
        {
            case MainSimulator.EPlayState.Lobby:
                LocalTinyPlayer.TeleportPlayer(SCENE_MANAGER.Instance.LobbyPos.position);
                break;
            case MainSimulator.EPlayState.Shop:
                LocalTinyPlayer.TeleportPlayer(SCENE_MANAGER.Instance.ShopSpawnPos.GetChild(0).position);
                break;
            case MainSimulator.EPlayState.Fighting:
                LocalTinyPlayer.TeleportPlayer(SCENE_MANAGER.Instance.BigArenaBattleSpawnPos.GetChild(0).position);
                break;
            case MainSimulator.EPlayState.End:
                break;
            default:
                break;
        }

    }

    void PlayerSpawn()
    {
        GameObject MyPlayer = null;
        switch (SCENE_MANAGER.Instance.ScenePlayState)
        {

            case MainSimulator.EPlayState.Lobby:
                 MyPlayer = Instantiate(m_PlayerPrefab, SCENE_MANAGER.Instance.LobbyPos.position, Quaternion.identity);
                break;
            case MainSimulator.EPlayState.Shop:
                 MyPlayer = Instantiate(m_PlayerPrefab, SCENE_MANAGER.Instance.ShopSpawnPos.GetChild(0).position, Quaternion.identity);
                break;
            case MainSimulator.EPlayState.Fighting:
                 MyPlayer = Instantiate(m_PlayerPrefab, SCENE_MANAGER.Instance.BigArenaBattleSpawnPos.GetChild(0).position, Quaternion.identity);
                break;
            case MainSimulator.EPlayState.End:
                 MyPlayer = Instantiate(m_PlayerPrefab, SCENE_MANAGER.Instance.LobbyPos.position, Quaternion.identity);
                break;
            default:
                break;
        }
        
        MyPlayer.name = "[local] PLAYER";
        m_TinyPlayer = MyPlayer.GetComponent<TinyPlayer>();
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
