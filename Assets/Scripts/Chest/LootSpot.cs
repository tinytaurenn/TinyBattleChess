using UnityEngine;

public class LootSpot :  Usable
{
    [SerializeField] SO_Armor m_ArmorItem; 
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

        Debug.Log("using lootspot"); 

        ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerLoadout.EquipArmorInLoadout(m_ArmorItem);
        ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerLoadout.EquipLoadout(); 



    }
}
