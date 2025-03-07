using Coherence.Toolkit;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Armor", menuName = "Scriptable Objects/Items/SO_Armor/ArmorEffect/ThornEffect")]
public class SO_Armor_ThornEffect : SO_ArmorEffect
{
    public override void OnActivate(Transform parent)
    {
        //
    }

    public override void OnTakeDamage(CoherenceSync damageDealer, int damage, FArmorParameters armorParams)
    {
        Debug.Log("taking damage with thorn effect");
    }
}
