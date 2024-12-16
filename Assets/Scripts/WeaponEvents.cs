using UnityEngine;

public class WeaponEvents : MonoBehaviour
{
    [SerializeField] PlayerWeapons m_PlayerWeapons; 

    public void LockAttackRelease() => m_PlayerWeapons.LockAttackRelease(true);
    public void UnlockAttackRelease() => m_PlayerWeapons.LockAttackRelease(false);
    public void LockAttack() => m_PlayerWeapons.LockAttack();

}
