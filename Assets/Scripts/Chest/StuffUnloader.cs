using UnityEngine;

public class StuffUnloader : Usable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TryUse()
    {
        base.TryUse();

        Debug.Log("using unloader");


        ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerLoadout.UnloadEquippedStuff();



    }
}
