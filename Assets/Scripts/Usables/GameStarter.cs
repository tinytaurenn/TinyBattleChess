using UnityEngine;

public class GameStarter : Usable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void TryUse()
    {

        Debug.Log("Loading another scene ");

        ConnectionsHandler.Instance.LoadArena(); 
        
     }




   
}
