using Coherence.Toolkit;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ServerSidedBlock : MonoBehaviour
{
    CoherenceSync m_Sync;
    [SerializeField] float timer = 0f; 
    [SerializeField] float time = 5f; 



    private void Awake()
    {
        m_Sync = GetComponent<CoherenceSync>(); 
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= time)
        {
            ChangeScene(); 
            timer = 0f;
        }
    }
    private void FixedUpdate()
    {
        Debug.Log("block active scene is :"+  SceneManager.GetActiveScene().name);
    }

    void ChangeScene()
    {
        //StartCoroutine(LoadSceneRoutine(1));
    }

    private IEnumerator LoadSceneRoutine(int sceneIndex)

    {
        int sceneInt = SceneManager.GetActiveScene().buildIndex;
        if(sceneInt == 1)
        {
            sceneIndex = 0;
        }
        else
        {
            sceneIndex = 1;
        }
        Debug.Log("loadscene in simulator");
        CoherenceSync[] bringAlong = new CoherenceSync[] { m_Sync };
        yield return CoherenceSceneManager.LoadScene(m_Sync.CoherenceBridge, sceneIndex, bringAlong);

    }
}
