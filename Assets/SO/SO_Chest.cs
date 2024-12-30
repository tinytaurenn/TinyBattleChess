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

    

    public List<SO_Item> GetItemList()
    {
        List<SO_Item> itemList = new List<SO_Item>();   
        Debug.Log("doing something chest");
        foreach (var item in Items.Items)
        {
           
            if ((item.GetType() == typeof(SO_BasicWeapon)) &&( (ItemType & EItemType.Weapon) != 0 ))
            {
                Debug.Log("adding weapon in chest list ");
                itemList.Add(item);

            }
            //next 


        }
        
        //Utils.Shuffle(itemList);

        return itemList;

    }

    public List<SO_Item> GetItemList(int size)
    {
        List<SO_Item> itemList = new List<SO_Item>();
        Debug.Log("doing something chest");
        foreach (var item in Items.Items)
        {

            if ((item.GetType() == typeof(SO_BasicWeapon)) && ((ItemType & EItemType.Weapon) != 0))
            {
                Debug.Log("adding weapon in chest list ");
                itemList.Add(item);

            }
            //next 


        }

        //Utils.Shuffle(itemList);

        itemList.Shuffle();

        return itemList;

    }


}
