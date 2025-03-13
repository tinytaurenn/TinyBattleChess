using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_Potion", menuName = "Scriptable Objects/Items/Potions/SO_Potions")]
public class SO_Potion : SO_Item
{
    public bool Throwable = false;
    public float ThrowForce = 10f;
    public float ExplosionRadius = 3f;

    public GameObject ThrowableGameObject; 
    public GameObject ExplosionEffect;
    public GameObject DrinkEffect;

    //public List<FGameEffect> Effects;

    public SO_GameEffect_Container GameEffectContainer;

    public int Charges = 1;
    


}
