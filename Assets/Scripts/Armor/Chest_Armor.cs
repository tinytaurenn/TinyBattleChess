using UnityEngine;

public class Chest_Armor : Armor
{
    public override SO_Armor.EArmorPlace ArmorPlace => SO_Armor.EArmorPlace.Chest;
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
