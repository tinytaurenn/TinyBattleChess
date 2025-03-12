using UnityEngine;

public class StuffUnloader : Usable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public override void TryUse()
    {

        Debug.Log("using unloader");


        ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerLoadout.UnloadEquippedStuff();



    }
}
