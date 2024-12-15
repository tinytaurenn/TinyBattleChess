using UnityEngine;

[CreateAssetMenu(fileName = "SO_BasicWeapon", menuName = "Scriptable Objects/SO_BasicWeapon")]
public class SO_BasicWeapon : ScriptableObject
{

    public int Damage = 10; 
    public float Speed = 1.5f;

    public int Cost = 10;
}
