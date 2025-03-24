using Coherence.Toolkit;
using UnityEngine;

public class WeaponEvents : MonoBehaviour
{
    [SerializeField] PlayerWeapons m_PlayerWeapons; 

    public void LockAttackRelease() => m_PlayerWeapons.LockAttackRelease(true);
    public void UnlockAttackRelease() => m_PlayerWeapons.LockAttackRelease(false);
    public void LockAttack() => m_PlayerWeapons.LockAttack();

    public void ActivateWeaponDamage()
    {
        if(transform.parent.GetComponent<CoherenceSync>().HasStateAuthority == false)
        {
            //Debug.Log("No authority to activate weapon");
            return;
        }
        
        if (m_PlayerWeapons.GetMainWeapon() == null)
        {
            
            return;
        }

        if (m_PlayerWeapons.GetMainWeapon().TryGetComponent<MeleeWeapon>(out MeleeWeapon meleeWeapon))
        {
            meleeWeapon.ActivateDamage(true);
        }
    }
    public void DeactivateWeaponDamage()
    {
        if (transform.parent.GetComponent<CoherenceSync>().HasStateAuthority == false)
        {
            //Debug.Log("No authority to Deactivate weapon");
            return;
        }
        if (m_PlayerWeapons.GetMainWeapon() == null)
        {
            //Debug.Log("No weapon to Deactivate");
            return;
        }

        if (m_PlayerWeapons.GetMainWeapon().TryGetComponent<MeleeWeapon>(out MeleeWeapon meleeWeapon))
        {
            meleeWeapon.ActivateDamage(false);
        }
    }

    public void LeftPunchDamage() => m_PlayerWeapons.DoBareHandedDamage(true);
    public void RightPunchDamage() => m_PlayerWeapons.DoBareHandedDamage(false);

}
