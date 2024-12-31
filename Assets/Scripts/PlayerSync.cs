using Coherence;
using Coherence.Toolkit;
using UnityEngine;

public class PlayerSync : MonoBehaviour
{
    [SerializeField] TinyPlayer m_TinyPlayer;
    [SerializeField] PlayerClothes m_PlayerClothes;
    CoherenceSync m_Sync;

    private void Awake()
    {
        m_Sync = GetComponent<CoherenceSync>();
    

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Sync()
    {
        m_Sync.SendCommand<PlayerClothes>(nameof(PlayerClothes.ChangeBody), MessageTarget.All, 0, m_PlayerClothes.m_BodyTextureIndex);
        m_Sync.SendCommand<TinyPlayer>(nameof(TinyPlayer.SyncElements), MessageTarget.All);
    }
}
