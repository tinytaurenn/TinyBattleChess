using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Scroll", menuName = "Scriptable Objects/Items/Scrolls/SO_Scrolls")]
public class SO_Scroll : SO_Item
{
    public float UseTime = 0.7f;
    public int Charges = 1;

    public List<SO_ScrollEffect> ScrollEffects;

}
