using Coherence.Toolkit;
using UnityEngine;
using UnityEngine.Audio;

public abstract class EntityFX : MonoBehaviour
{

    protected CoherenceSync m_Sync;
    protected AudioSource m_AudioSource;

    [SerializeField] protected ParticleSystem m_HurtFX; 
    private void Awake()
    {
        m_Sync = GetComponent<CoherenceSync>();
        m_AudioSource = GetComponent<AudioSource>();

    }
    [Command]
    public abstract void PlayHurtFX(int choiceIndex);
    [Command]
    public abstract void PlayParryFX(int choiceIndex);

}
