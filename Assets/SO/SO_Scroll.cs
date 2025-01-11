using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Scroll", menuName = "Scriptable Objects/Items/Scrolls/SO_Scrolls")]
public class SO_Scroll : SO_Item
{
    [Flags]
    public enum EScrollElem
    {
        Neutral = 1,
        Earth = 2,
        Fire = 4,
        Water = 8,
        Air = 16,
        Holy = 32,
        Shadow = 64,
        Nature = 128,
        Arcane = 256,
        Lightning = 512,
        Ice = 1024,
        Poison = 2048,
    }

    public EScrollElem ScrollElem;

    public int Charges = 1;


}
