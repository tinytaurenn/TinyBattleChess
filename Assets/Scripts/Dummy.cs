using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Dummy : MonoBehaviour, IDamageable
{

    [SerializeField]bool  m_InParry = false;
    [SerializeField] EWeaponDirection m_WeaponDirection = EWeaponDirection.Right;

    public event Action<bool,IDamageable> OnParryEvent;
   

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Dummy took " + damage + " damage!");
    }

    public void Parry(EWeaponDirection direction)
    {
        Debug.Log("Dummy parried from " + direction.ToString() + " direction!");
    }

    public void TakeMelee(PlayerWeapons playerWeapons, int damage)
    {
        EWeaponDirection direction = playerWeapons.m_WeaponDirection;
        Debug.Log(" strike " + direction.ToString() + " direction!"); 

        bool parry = false;
        switch(direction)
        {
            case EWeaponDirection.Right:
                parry = m_WeaponDirection == EWeaponDirection.Left;
                break;
            case EWeaponDirection.Left:
                parry = m_WeaponDirection == EWeaponDirection.Right;
                break;
            case EWeaponDirection.Up:
                parry = m_WeaponDirection == EWeaponDirection.Up;
                break;
            case EWeaponDirection.Down:
                parry = m_WeaponDirection == EWeaponDirection.Down;
                break;
        }

        if(m_InParry && parry)
        {
            Parry(direction);
            OnParryEvent?.Invoke(true,this);
        }
        else
        {
            TakeDamage(damage);
            OnParryEvent?.Invoke(false,this);

        } 



    }
}
