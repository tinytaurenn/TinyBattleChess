
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;




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


    





}
