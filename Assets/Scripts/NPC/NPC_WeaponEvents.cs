using Coherence.Toolkit;
using UnityEngine;

public class NPC_WeaponEvents : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] HumanoidNPC m_NPC;

    public void LockAttackRelease() => m_NPC.LockAttackRelease(true);
    public void UnlockAttackRelease() => m_NPC.LockAttackRelease(false);
    public void LockAttack() => m_NPC.LockAttack();

    public void ActivateWeaponDamage()
    {

        if (m_NPC.GetMainWeapon() == null)
        {

            Debug.Log("No weapon to activate"); 
            return;
        }
        if(m_NPC.GetMainWeapon().TryGetComponent<MeleeWeapon>(out MeleeWeapon meleeWeapon))
        {
           meleeWeapon.ActivateDamage(true);
        }

        
    }
    public void DeactivateWeaponDamage()
    {

        if (m_NPC.GetMainWeapon() == null)
        {
            Debug.Log("No weapon to Deactivate");
            return;
        }

        if(m_NPC.GetMainWeapon().TryGetComponent<MeleeWeapon>(out MeleeWeapon meleeWeapon))
        {
            meleeWeapon.ActivateDamage(false);
        }
    }
}
