using Coherence.Toolkit;
using UnityEngine;

public class Shoulders_Armor : Armor
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject m_LeftShoulder;
    [SerializeField] GameObject m_RightShoulder;

    [SerializeField] GameObject m_InstantiatedLeftShoulder; 
    [SerializeField] GameObject m_InstantiatedRightShoulder;

    public GameObject LeftShoulder => m_LeftShoulder;
    public GameObject RightShoulder => m_RightShoulder;

    public GameObject InstantiatedLeftShoulder { get { return m_InstantiatedLeftShoulder; } set { m_InstantiatedLeftShoulder = value; } }
    public GameObject InstantiatedRightShoulder { get { return m_InstantiatedRightShoulder; } set { m_InstantiatedRightShoulder = value; } }

    public override SO_Armor.EArmorPlace ArmorPlace => SO_Armor.EArmorPlace.Shoulders;
    
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
