using UnityEngine;
using UnityEngine.Audio;

public class HumanoidFX : EntityFX
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void PlayHurtFX(int choiceIndex)
    {

        m_HurtFX.Play();
    }


    public override void PlayParryFX(int choiceIndex)
    {
        if (TryGetComponent<HumanoidNPC>(out HumanoidNPC npc))
        {
            if(npc.GetMainWeapon() != null )
            {
                AudioResource sound = npc.GetMainWeapon().m_ParryAudios[choiceIndex];
                m_AudioSource.resource = sound;
                m_AudioSource.Play();
            }
        }
        else
        {
            Debug.LogError("HumanoidNPC component not found on HumanoidFX");
        }
    }
}
