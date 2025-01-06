using UnityEngine;

public class PlayerFX : EntityFX
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void PlayHurtFX(int choiceIndex)
    {
        base.PlayHurtFX(choiceIndex);


        m_HurtFX.Play();
    }
    

    public override void PlayParryFX(int choiceIndex)
    {
        base.PlayParryFX(choiceIndex);
        m_AudioSource.resource = m_ParryAudios[choiceIndex];
        m_AudioSource.Play();

    }
}
