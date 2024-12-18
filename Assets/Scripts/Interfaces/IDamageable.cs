using Coherence.Toolkit;
using System;
using UnityEngine;

public interface IDamageable
{


    public void TakeMeleeSync(int DirectionNESO, CoherenceSync sync, int damage,Vector3 attackerPos);

    public void TakeDamageSync(int damage, CoherenceSync Damagersync);


    public void ParrySync(int damage, CoherenceSync DamagerSync);



}
