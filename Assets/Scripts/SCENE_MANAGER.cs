using UnityEngine;
using UnityEngine.SceneManagement;

public class SCENE_MANAGER : MonoBehaviour
{
    //put on every scene 
    public static SCENE_MANAGER Instance { get; private set;  }

    [SerializeField] Light m_MainLight; 

    [SerializeField] Transform m_ShopSpawnPositions;
    [SerializeField] Transform m_BattleSpawnPositions;
    [SerializeField] Transform m_BigArenaBattleSpawnPositions;
    [SerializeField] Transform m_LobbyPos;
    public Transform ShopSpawnPos => m_ShopSpawnPositions;
    public Transform BattleSpawnPos => m_BattleSpawnPositions;
    public Transform BigArenaBattleSpawnPos => m_BigArenaBattleSpawnPositions;
    public Transform LobbyPos => m_LobbyPos;

    [Space(10)]
    [Header("Arenas GameObjects")]
    [SerializeField] GameObject m_LibrairyArena;

    //[SerializeField] MainSimulator.EPlayState m_ScenePlayState = MainSimulator.EPlayState.Fighting; 

    //public MainSimulator.EPlayState ScenePlayState => m_ScenePlayState;

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

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {

        Debug.Log("scene manager scene loaded"); 
        //ConnectionsHandler.Instance.LocalTinyPlayer.TeleportPlayer(m_BattleSpawnPositions.GetChild(0).position);
    }

    private void OnEnable()
    {


    }

    private void OnDisable()
    {

        
    }



    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        
    }

    public void UpdateScene(MainSimulator.EPlayState ePlayState)
    {
        switch (ePlayState)
        {
            case MainSimulator.EPlayState.Lobby:
                m_LibrairyArena.SetActive(false);
                break;
            case MainSimulator.EPlayState.Shop:
                break;
            case MainSimulator.EPlayState.Fighting:
                m_LibrairyArena.SetActive(true);
                m_MainLight.enabled = false;
                break;
            case MainSimulator.EPlayState.End:
                m_LibrairyArena.SetActive(false);
                break;
            default:
                break;
        }
    }




    

}
