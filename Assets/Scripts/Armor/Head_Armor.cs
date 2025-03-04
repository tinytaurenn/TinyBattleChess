using UnityEngine;

public class Head_Armor : Armor
{

    public override FArmorParameters ArmorParameters { 
        get {  return new FArmorParameters(
            base.ArmorParameters.MagicArmor,
            base.ArmorParameters.Armor,
            base.ArmorParameters.Cost,
            base.ArmorParameters.ArmorType,
            SO_Armor.EArmorPlace.Helmet);}
        set => base.ArmorParameters = value; }
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
