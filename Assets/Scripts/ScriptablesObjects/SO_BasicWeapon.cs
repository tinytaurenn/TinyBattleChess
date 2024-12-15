using UnityEngine;

[CreateAssetMenu(fileName = "SO_BasicWeapon", menuName = "Scriptable Objects/SO_BasicWeapon")]
public class SO_BasicWeapon : ScriptableObject
{

    public enum EWeaponType
    {
        Sword,
        Axe,
        Mace,
        Spear,
        Bow,
        Crossbow,
        LongBow,
        Staff,
        Shield,
        Dagger
    }
    public enum EWeaponSize
    {
       One_Handed,
       Two_Handed,
       LeftOnly
       
    }
    public EWeaponType WeaponType;
    public EWeaponSize WeaponSize;


    public int Damage = 10; 
    public float Speed = 1.5f;

    public int Cost = 10;

    Vector3 PositionOffset = Vector3.zero;
    Vector3 RotationOffset = Vector3.zero;
}
