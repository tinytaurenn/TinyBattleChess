using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    Collider m_Collider; 

    [SerializeField] SO_Chest m_ChestSO;

    [SerializeField] List<SO_Item> m_ChosenItems = new List<SO_Item>();

    public int Cost = 10; 
    

    void Start()
    {
        

       
    }
    private void Awake()
    {
        m_Collider = GetComponent<Collider>();
    }


    void Update()
    {
        
    }

    public void LoadChest(EItemRarity rarity)
    {
        m_ChosenItems = m_ChestSO.GetItemsList(rarity);

        
    }

    public SO_Item GetItem()
    {
        return m_ChosenItems.RandomInList();
    }
}
