using UnityEngine;

public class Chest_Armor : Armor
{
    public override FArmorParameters ArmorParameters
    {
        get
        {
            return new FArmorParameters(
            base.ArmorParameters.MagicArmor,
            base.ArmorParameters.Armor,
            base.ArmorParameters.ArmorType,
            EArmorPlace.Chest);
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
}
