using System.Collections.Generic;
using UnityEngine;

public class LootSpot :  Usable
{
    [SerializeField] public List<SO_Armor> m_ArmorItems; 

    public override void TryUse()
    {

        Debug.Log("using lootspot");
        foreach (SO_Armor item in m_ArmorItems)
        {
            ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerLoadout.EquipArmorInLoadout(item);
        }

        ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerLoadout.EquipLoadout(); 



    }
}
