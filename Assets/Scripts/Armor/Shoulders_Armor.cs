using Coherence.Toolkit;
using UnityEngine;

public class Shoulders_Armor : Armor
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]public  GameObject m_LeftShoulder;
    [SerializeField]public  GameObject m_RightShoulder;

    [SerializeField] GameObject m_InstantiatedLeftShoulder; 
    [SerializeField] GameObject m_InstantiatedRightShoulder;

    [SerializeField]public  Renderer m_LeftShoulderVisual;
    [SerializeField] public Renderer m_RightShoulderVisual; 

    public GameObject InstantiatedLeftShoulder { get { return m_InstantiatedLeftShoulder; } set { m_InstantiatedLeftShoulder = value; } }
    public GameObject InstantiatedRightShoulder { get { return m_InstantiatedRightShoulder; } set { m_InstantiatedRightShoulder = value; } }

    public override FArmorParameters ArmorParameters
    {
        get
        {
            return new FArmorParameters(
            base.ArmorParameters.MagicArmor,
            base.ArmorParameters.Armor,
            base.ArmorParameters.Cost,
            base.ArmorParameters.ArmorType,
            SO_Armor.EArmorPlace.Shoulders);
        }
        set => base.ArmorParameters = value;
    }

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void ShowVisuals(bool show)
    {
        m_LeftShoulderVisual.enabled = show;
        m_RightShoulderVisual.enabled = show;

    }
}
