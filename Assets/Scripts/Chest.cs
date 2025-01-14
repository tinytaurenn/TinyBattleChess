using PlayerControls;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Usable
{
    Collider m_Collider; 

    [SerializeField] SO_Chest m_ChestSO;

    [SerializeField] List<SO_Item> m_ChosenItems = new List<SO_Item>();

    public int Cost = 10; 
    

    
    protected override void Awake()
    {
        base.Awake();
        m_Collider = GetComponent<Collider>();
    }


    void Update()
    {
        
    }

    public override  void  TryUse()
    {
        base.TryUse();

        LoadChest(3,EItemRarity.Common);
        ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerControls.SwitchState(PlayerControls.PlayerControls.ECrontrolState.Selecting); 

        //SO_Item newItem = GetItemFast();
        //Debug.Log("Chest opened : " + newItem.ItemName);
        //ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerLoadout.EquipItemInLoadout(newItem); 
        
    }

    public void LoadChest(int number,EItemRarity rarity)
    {
        m_ChosenItems = m_ChestSO.GetItemsList(number, rarity);

        
    }

    public SO_Item GetItem()
    {
        return m_ChosenItems.RandomInList();
    }
    public SO_Item GetItemFast()
    {
        return m_ChestSO.GetItemFast(EItemRarity.Common);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
