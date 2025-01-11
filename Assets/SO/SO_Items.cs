
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Flags]
public enum EItemType
{
    Weapon = 1,
    Armor = 2,  // passive without consomption
    Potion = 4, // passive with consomption
    Scroll = 8, // ability with consomption
    Rune = 16 // Ability without consomption
}

public enum EItemRarity
{
    Common = 0 ,
    Uncommon = 1,
    Rare = 2,
    Epic= 3,
    Legendary = 4
}

[CreateAssetMenu(fileName = "SO_Items", menuName = "Scriptable Objects/Items/SO_Items")]


public class SO_Items : ScriptableObject
{
    public List<SO_Item> Items;

    

    const float CommonChance = 100f;

    
    [Range(1f, 100f)]
    public float UnCommonChance = 75f;


    [Range(1f, 100f)]
    public float RareChance = 55f;


    [Range(1f, 100f)]
    public float EpicChance = 35f;

    [Range(1f, 100f)]
    public float LegendaryChance = 15f;

    public SO_Item FindItemByName(string name)
    {
        return Items.Find(item => item.ItemName == name);
        
    }





}
