using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected int m_GameID = -1; 
    public int GameID
    {
        get { return m_GameID; }
        set { m_GameID = value; }
    }

}
