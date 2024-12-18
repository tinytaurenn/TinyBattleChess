using Coherence.Toolkit;
using UnityEngine;

public abstract class EntityFX : MonoBehaviour
{

    protected CoherenceSync m_Sync;
    protected AudioSource m_AudioSource;

    private void Awake()
    {
        m_Sync = GetComponent<CoherenceSync>();
        m_AudioSource = GetComponent<AudioSource>();

    }
    public virtual void PlayHurtFX()
    {
        Debug.Log("PlayHurtFX");
    }

    public virtual void PlayParryFX()
    {
        Debug.Log("PlayParryFX");

    }
}
