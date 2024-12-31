using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] GameObject m_PlayerModel;
    [SerializeField] GameObject m_RagDollObject;
     GameObject m_MyRagDoll;
    
    public void SpawnRagDoll()
    {
        m_MyRagDoll = Instantiate(m_RagDollObject, m_PlayerModel.transform.position, m_PlayerModel.transform.rotation);
    }
}
