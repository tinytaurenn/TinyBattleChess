using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Armor", menuName = "Scriptable Objects/Items/SO_Armor")]
public class SO_Armor : SO_Item
{

    public EArmorType ArmorType; 
    public EArmorPlace ArmorPlace;
    public int MagicArmor;
    public int Armor;

    public List<SO_ArmorEffect> ArmorEffects;

    
}
