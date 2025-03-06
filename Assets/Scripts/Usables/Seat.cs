using Coherence;
using Coherence.Toolkit;
using UnityEngine;

public class Seat : Usable
{

    [Sync] public bool IsOccupied;
    public Transform m_SeatPosition;
    BoxCollider m_Collider; 
    protected override void Awake()
    {
        base.Awake(); 
        m_Collider = GetComponent<BoxCollider>();
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
        m_Collider.enabled = false;
        IsOccupied = true;
    }

    public void ReleaseSeat()
    {
        IsOccupied = false;
        m_Collider.enabled = true;
    }
}
