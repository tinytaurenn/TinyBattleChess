using Coherence.Toolkit;
using System;
using Unity.VisualScripting;
using UnityEngine;


public abstract class SO_ArmorEffect : ScriptableObject
{
    public abstract void OnTakeDamage(CoherenceSync damageDealer, int damage, FArmorParameters armorParams);

    public abstract void OnActivate(Transform parent);
    

}
