using System;
using UnityEngine;

public interface IDamageable
{

    public event Action<bool,IDamageable> OnParryEvent;
    public void TakeDamage(int damage);

    public void TakeMelee(PlayerWeapons playerWeapons,  int damage); 

    public void Parry(EWeaponDirection direction);



}
