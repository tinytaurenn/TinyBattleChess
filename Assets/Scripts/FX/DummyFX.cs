using UnityEngine;
using UnityEngine.Audio;

public class DummyFX : EntityFX
{
   [SerializeField] AudioResource m_ParryAudio;
    
    
    public override void PlayHurtFX()
    {
        base.PlayHurtFX();

        
    }

    public override void PlayParryFX()
    {
        base.PlayParryFX();
        m_AudioSource.resource = m_ParryAudio;
        m_AudioSource.Play();
        
    }
}
