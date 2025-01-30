using System.Collections.Generic;
using UnityEngine;

public class LootSpot :  Usable
{
    [SerializeField] public List<SO_Armor> m_ArmorItems; 
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
        foreach (SO_Armor item in m_ArmorItems)
        {
            ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerLoadout.EquipArmorInLoadout(item);
        }

        ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerLoadout.EquipLoadout(); 



    }
}
