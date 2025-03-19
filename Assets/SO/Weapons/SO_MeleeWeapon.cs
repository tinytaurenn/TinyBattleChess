using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SO_Weapon", menuName = "Scriptable Objects/Items/Weapons/SO_Weapon/MeleeWeapon")]
public class SO_MeleeWeapon : SO_Weapon
{
    public FMeleeWeaponParameters MeleeWeaponParameters; 

    public List<AudioResource> HitSounds;
    public List<AudioResource> ParrySounds;
}
