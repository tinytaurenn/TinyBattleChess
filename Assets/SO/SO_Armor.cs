using UnityEngine;

[CreateAssetMenu(fileName = "SO_Armor", menuName = "Scriptable Objects/Items/SO_Armor")]
public class SO_Armor : SO_Item
{
    public enum EArmorType
    {
        Cloth = 0,
        Leather = 1,
        Mail = 2,
        Plate = 3,
    }

    public enum EArmorPlace
    {
        Helmet = 0,
        Chest = 1,
        Shoulders = 2,
    }

    public EArmorType ArmorType; 
    public EArmorPlace ArmorPLace;

    
}
