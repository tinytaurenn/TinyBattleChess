using Coherence;
using Coherence.Toolkit;
using UnityEngine;

public class PlayerClothes : MonoBehaviour
{
    [SerializeField] CoherenceSync m_Sync;
    [SerializeField] SkinnedMeshRenderer m_BodyRenderer;
    [SerializeField] Texture2D[] m_BodyTextures;

    MaterialPropertyBlock m_BodyMPB;
    [OnValueSynced(nameof(ChangeBody))] public int m_BodyTextureIndex = 0;



    private void Awake()
    {
        m_BodyMPB = new MaterialPropertyBlock();

    }

    void Start()
    {
        if (!m_Sync.HasStateAuthority) return; 
        m_BodyTextureIndex = Random.Range(0, m_BodyTextures.Length);

        //SyncClothes();  
        ChangeBody(0,m_BodyTextureIndex); 
           
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        if(Coherence.SimulatorUtility.IsSimulator) return;
        if(m_BodyMPB.GetTexture("_BaseMap") != m_BodyTextures[m_BodyTextureIndex])
        {
            ChangeBody(0,m_BodyTextureIndex);
        }
    }

    public void ChangeBody(int oldIndex ,int Newindex)
    {
        m_BodyMPB.SetTexture("_BaseMap", m_BodyTextures[Newindex]);
        m_BodyRenderer.SetPropertyBlock(m_BodyMPB);
        Debug.Log("Body Changed, old index was : " + oldIndex + " and new is : " + Newindex);
    }


    //not used
    public void SyncClothes()
    {
        m_Sync.SendCommand<PlayerClothes>(nameof(PlayerClothes.ChangeBody), MessageTarget.All, 0, m_BodyTextureIndex);
    }
}
