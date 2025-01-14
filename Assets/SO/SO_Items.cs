
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Flags]
public enum EItemType
{
    Weapon = 1, //rouge brun
    Armor = 2,  // passive without consomption // gris bleu 
    Potion = 4, // passive with consomption //vert pomme
    Scroll = 8, // ability with consomption // orange colruyt 
    Rune = 16 // Ability without consomption // bleu electrique
}

public enum EItemRarity
{
    Common = 0 , // white
    Uncommon = 1, // green 
    Rare = 2, //blue
    Epic= 3, // purple
    Legendary = 4 // orange
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
