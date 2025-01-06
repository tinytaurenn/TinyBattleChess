using Coherence.Toolkit;
using UnityEngine;
using UnityEngine.Audio;

public abstract class EntityFX : MonoBehaviour
{

    protected CoherenceSync m_Sync;
    protected AudioSource m_AudioSource;

    [SerializeField] protected ParticleSystem m_HurtFX; 

    [SerializeField] protected AudioResource[] m_ParryAudios;

    private void Awake()
    {
        m_Sync = GetComponent<CoherenceSync>();
        m_AudioSource = GetComponent<AudioSource>();

    }
    public virtual void PlayHurtFX(int choiceIndex)
    {
        Debug.Log("PlayHurtFX");
    }

    public virtual void PlayParryFX(int choiceIndex)
    {
        Debug.Log("PlayParryFX");

    }
}
