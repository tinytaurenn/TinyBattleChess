using Coherence.Toolkit;
using UnityEngine;

public class Seat : Usable
{

    [Sync] public bool IsOccupied;
    protected override void Awake()
    {
        base.Awake(); 
    }
    public override void TryUse()
    {
        if(IsOccupied)
        {
            Debug.Log("Seat is occupied");
            return;
        }
        IsOccupied = true; 


    }
}
