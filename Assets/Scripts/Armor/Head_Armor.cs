using UnityEngine;

public class Head_Armor : Armor
{

    public override SO_Armor.EArmorPlace ArmorPlace => SO_Armor.EArmorPlace.Helmet;
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
