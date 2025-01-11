using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Chest", menuName = "Scriptable Objects/SO_Chest")]
public class SO_Chest : ScriptableObject
{
    

    [SerializeField] EItemType ItemType;

    [SerializeField] SO_Items Items;

    Dictionary<Type, EItemType> m_ItemTypesDico = new Dictionary<Type, EItemType>{
    { typeof(SO_Weapon), EItemType.Weapon },
    { typeof(SO_Potion), EItemType.Potion },
    { typeof(SO_Scroll), EItemType.Scroll }//add armor and rune later

};


    public SO_Item GetItemFast(EItemRarity maxRarity)
    {
        List<SO_Item> ShuffleitemList = Items.Items;
        ShuffleitemList.Shuffle();
        Debug.Log("Getting Item chest");

        foreach (var item in ShuffleitemList)
        {

            if (m_ItemTypesDico.TryGetValue(item.GetType(), out EItemType itemType))
            {
                if ((ItemType & itemType) != 0 && (int)item.EItemRarity <= (int)maxRarity)
                {
                    Debug.Log($"Adding {item.GetType().Name} to chest list");
                    return item;
                }
            }


        }
        Debug.Log("No item found");
        return null; 

    }

    public List<SO_Item> GetItemsList(EItemRarity maxRarity)
    {
        List<SO_Item> ShuffleitemList = Items.Items;
        ShuffleitemList.Shuffle();
        List<SO_Item> itemList = new List<SO_Item>();   
        Debug.Log("Getting Items chest"); 

       
        foreach (var item in ShuffleitemList)
        {

            if (m_ItemTypesDico.TryGetValue(item.GetType(), out EItemType itemType))
            {
                if ((ItemType & itemType) != 0 && (int)item.EItemRarity <= (int)maxRarity)
                {
                    Debug.Log($"Adding {item.GetType().Name} to chest list");
                    itemList.Add(item);
                }
            }


        }
        
        

        return itemList;

    }

    public List<SO_Item> GetItemsList(int size, EItemRarity maxRarity)
    {
        List<SO_Item> ShuffleitemList = Items.Items;
        ShuffleitemList.Shuffle();
        List<SO_Item> itemList = new List<SO_Item>();
        Debug.Log("doing something chest");
        int i = 0;
        foreach (var item in ShuffleitemList)
        {

            if (m_ItemTypesDico.TryGetValue(item.GetType(), out EItemType itemType))
            {
                if ((ItemType & itemType) != 0 && (int)item.EItemRarity <= (int)maxRarity)
                {
                    Debug.Log($"Adding {item.GetType().Name} to chest list");
                    itemList.Add(item);
                    i++;
                }
            }


            if (i >= size)
            {
                break;
            }

        }
        

        return itemList;

        

    }
    //local random 
    public void GetRandomItem()
    {
        EItemRarity rarity = GetItemRarity();
        List<SO_Item> items = GetItemsList(rarity);
        Debug.Log("Item rarity: " + rarity);

        SO_Item item = items.RandomInList(); 

    }

    public EItemRarity GetItemRarity()
    {
        //chose rarity 
        float randomRarity = UnityEngine.Random.Range(0.01f, 99.99f);

        if (randomRarity > Items.UnCommonChance) return EItemRarity.Common;

        if (randomRarity > Items.RareChance) return EItemRarity.Uncommon;

        if (randomRarity > Items.EpicChance) return EItemRarity.Rare;

        if(randomRarity > Items.LegendaryChance) return EItemRarity.Epic;

        return EItemRarity.Legendary;


    }

    


}
