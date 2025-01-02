using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum EItemType
{
    Weapon = 1,
    Armor = 2,
    Potion = 4,
    Scroll = 8,
    Gold = 16
}

[CreateAssetMenu(fileName = "SO_Items", menuName = "Scriptable Objects/Items/SO_Items")]


public class SO_Items : ScriptableObject
{
    

    public List<SO_Item> Items;   


}
