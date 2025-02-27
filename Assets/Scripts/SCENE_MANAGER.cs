using Coherence.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SCENE_MANAGER : MonoBehaviour
{
    //put on every scene 
    public static SCENE_MANAGER Instance { get; private set;  }



    [SerializeField] Transform m_ShopSpawnPositions;
    [SerializeField] Transform m_BattleSpawnPositions;
    [SerializeField] Transform m_BigArenaBattleSpawnPositions;
    [SerializeField] Transform m_LobbyPos;
    public Transform ShopSpawnPos => m_ShopSpawnPositions;
    public Transform BattleSpawnPos => m_BattleSpawnPositions;
    public Transform BigArenaBattleSpawnPos => m_BigArenaBattleSpawnPositions;
    public Transform LobbyPos => m_LobbyPos;



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




    

}
