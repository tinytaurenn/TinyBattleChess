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
        
        if (m_PlayerWeapons.m_MainWeapon == null)
        {
            
            //Debug.Log("No weapon to activate"); 
            return;
        }

        m_PlayerWeapons.m_MainWeapon.ActivateDamage(true);
    }
    public void DeactivateWeaponDamage()
    {
        if (transform.parent.GetComponent<CoherenceSync>().HasStateAuthority == false)
        {
            //Debug.Log("No authority to Deactivate weapon");
            return;
        }
        if (m_PlayerWeapons.m_MainWeapon == null)
        {
            //Debug.Log("No weapon to Deactivate");
            return;
        }

        m_PlayerWeapons.m_MainWeapon.ActivateDamage(false);
    }

}
