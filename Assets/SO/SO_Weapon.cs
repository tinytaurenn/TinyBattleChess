using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SO_Weapon", menuName = "Scriptable Objects/Items/Weapons/SO_Weapon")]
public class SO_Weapon : SO_Item
{
    
    
     
    public EWeaponType WeaponType;
    public EWeaponSize WeaponSize;

    public EDamageType DamageType;


    public int Damage = 10; 
    public float Speed = 1.5f;

    

    Vector3 PositionOffset = Vector3.zero;
    Vector3 RotationOffset = Vector3.zero;

    public List<AudioResource> HitSounds;
    public List<AudioResource> ParrySounds;
}
