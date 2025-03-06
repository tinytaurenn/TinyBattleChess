using Coherence;
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
        if (!m_Sync.HasStateAuthority)
        {
            m_Sync.RequestAuthority(AuthorityType.Full);
            
        }

        DoUse();

    }

    protected override void DoUse()
    {
        base.DoUse();
        Debug.Log("Sitting on seat");
        IsOccupied = true;
    }
}
