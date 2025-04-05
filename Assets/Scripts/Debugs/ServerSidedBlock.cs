using Coherence.Toolkit;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ServerSidedBlock : Usable
{


    protected override void Awake()
    {
        m_Sync = GetComponent<CoherenceSync>(); 
    }
    protected override void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {

    }
    private void FixedUpdate()
    {

    }

    public override void TryUse()
    {
        m_Sync.SendCommand<ServerSidedBlock>(nameof(MultiAction), Coherence.MessageTarget.All);
    }
    [Command]

    public void MultiAction()
    {
        int random =  Random.Range(0, 100);
        Debug.Log("server sided block action: " + random);
    }




}
