using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Potion", menuName = "Scriptable Objects/Items/Potions/SO_Potions")]
public class SO_Potion : SO_Item
{
    public bool Throwable = false;
  
    public List<FPotionEffect> Effects;

    public int Charges = 1;
    


}
