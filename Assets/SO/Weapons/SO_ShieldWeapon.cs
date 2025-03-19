using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SO_Weapon", menuName = "Scriptable Objects/Items/Weapons/SO_Weapon/ShieldWeapon")]
public class SO_ShieldWeapon : SO_Weapon
{
    public List<AudioResource> ParrySounds;
}
