using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SO_Weapon", menuName = "Scriptable Objects/Items/Weapons/SO_Weapon")]
public class SO_Weapon : SO_Item
{
    
    public enum EWeaponType 
    {
        Sword = 1,
        Axe = 2,
        Mace = 4,
        Spear = 8,
        Bow = 16,
        Crossbow = 32,
        LongBow = 64,
        Staff = 128,
        Shield = 256,
        Dagger = 512
    }
    public enum EWeaponSize
    {
       Right_Handed,
       Left_Handed, 
       Two_Handed,
       
    }
     
    public EWeaponType WeaponType;
    public EWeaponSize WeaponSize;

     


    public int Damage = 10; 
    public float Speed = 1.5f;

    

    Vector3 PositionOffset = Vector3.zero;
    Vector3 RotationOffset = Vector3.zero;

    public List<AudioResource> HitSounds;
}
