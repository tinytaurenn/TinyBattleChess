using UnityEngine;
using Sirenix.OdinInspector.Editor;
using System.Linq;
using Sirenix.Utilities.Editor;
using Sirenix.Serialization;
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

public class ItemOrganizer : OdinMenuEditorWindow
{

    [MenuItem("TinyTools/Item Organizer")]
    private static void OpenWindow()
    {
        var window = GetWindow<ItemOrganizer>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1600, 600);

        
    }

    [SerializeField]
    
    private WeaponVisualizer weaponVisualizer = new WeaponVisualizer();
    [SerializeField] 
    private ArmorVisualizer armorVisualizer = new ArmorVisualizer();
    [SerializeField]
    private InventoryItemVisualizer inventoryItemVisualizer = new InventoryItemVisualizer();

    [SerializeField]

    private Menu menu = new Menu();

    protected override OdinMenuTree BuildMenuTree()
    {


        OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: false)
            {
                { "All Items",                      this.menu,                           EditorIcons.Pen                       }, // Draws the this.someData field in this case.
                { "Weapons",              this.weaponVisualizer                                                           },
                { "Armors",               this.armorVisualizer                                                            },
                {"Inventory Items",       this.inventoryItemVisualizer}
            };

        //tree.MenuItems.Insert(2, new OdinMenuItem(tree, "Menu Style", tree.DefaultMenuStyle));

        //tree.SortMenuItemsByName();

        // As you can see, Odin provides a few ways to quickly add editors / objects to your menu tree.
        // The API also gives you full control over the selection, etc..
        // Make sure to check out the API Documentation for OdinMenuEditorWindow, OdinMenuTree and OdinMenuItem for more information on what you can do!

        menu.FindScriptableObjects();
        weaponVisualizer.CreateVisualizer(menu.FoundScriptableObjects);
        armorVisualizer.CreateVisualizer(menu.FoundScriptableObjects);
        inventoryItemVisualizer.CreateVisualizer(menu.FoundScriptableObjects);
        return tree;

        
    }


}

[HideLabel]
[Serializable]
public class Menu
{
    [ReadOnly]
    [ListDrawerSettings()]
    public List<SO_Item> FoundScriptableObjects = new List<SO_Item>();

    public void FindScriptableObjects()
    {
        FoundScriptableObjects.Clear();
        string[] guids = AssetDatabase.FindAssets("t:SO_Item", new[] { "Assets/SO" });
        foreach (string assetPath in guids)
        {

            string path = AssetDatabase.GUIDToAssetPath(assetPath);
            SO_Item item = AssetDatabase.LoadAssetAtPath<SO_Item>(path);
            if (item != null)
            {
                FoundScriptableObjects.Add(item);


            }
        }
    }
}

[Serializable]
public abstract class ItemsListed
{
    [TableColumnWidth(57, Resizable = false)]
    [ReadOnly]
    public SO_Item SO_Item;
    [ReadOnly,AssetsOnly]
    public GameObject UsableGameObject;

    [PreviewField(Alignment = Sirenix.OdinInspector.ObjectFieldAlignment.Center)]
    public Sprite Icon;

    //[TextArea]
    public string ItemName;

    public EItemRarity ItemRarity;

    [OnInspectorInit]
    private void CreateData()
    {

    }
}

[HideLabel]
[Serializable]
public class WeaponVisualizer
{

     // Take a look at SomeData.cs to see how serialization works in Editor Windows.

    

    [TableList(AlwaysExpanded = false, DrawScrollView = false),Searchable]
    [GUIColor("#f2c2b3")]
    public List<MeleeWeaponListed> MeleeWeaponStats = new List<MeleeWeaponListed>();
    [TableList(AlwaysExpanded = false, DrawScrollView = false), Searchable]
    [GUIColor("#b3dff2")]
    public List<StaffWeaponListed> StaffWeaponStats = new List<StaffWeaponListed>();
    [TableList(AlwaysExpanded = false, DrawScrollView = false), Searchable]
    public List<ShieldWeaponListed> shieldWeaponsStats = new List<ShieldWeaponListed>();




    public void CreateVisualizer(List<SO_Item> list)
    {
        MeleeWeaponStats.Clear();
        StaffWeaponStats.Clear();
        shieldWeaponsStats.Clear();

        foreach (SO_Item item in list)
        {
            if(item is SO_Weapon weapon)
            {
                switch (weapon.WeaponParameters.WeaponType)
                {
                    case EWeaponType.Hands:
                        break;
                    case EWeaponType.Melee:
                        MeleeWeaponListed meleeWeaponListed = new MeleeWeaponListed(weapon as SO_MeleeWeapon);
                        MeleeWeaponStats.Add(meleeWeaponListed);
                        break;
                    case EWeaponType.Staff:
                        StaffWeaponListed staffWeaponListed = new StaffWeaponListed(weapon as SO_StaffWeapon);
                        StaffWeaponStats.Add(staffWeaponListed);
                        break;
                    case EWeaponType.Ranged:
                        //RangedWeaponListed rangedWeaponListed = new RangedWeaponListed(weapon as );

                        break;
                    case EWeaponType.Shield:
                        ShieldWeaponListed shieldWeaponListed = new ShieldWeaponListed(weapon as SO_ShieldWeapon );
                        shieldWeaponsStats.Add(shieldWeaponListed);
                        break;
                    default:
                        break;
                }
            }
           


            
        }
        
    }

    [Button("Apply Changes")]
    private void ApplyChanges()
    {
        ApplyWeaponChanges();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void ApplyWeaponChanges()
    {
        foreach (MeleeWeaponListed item in MeleeWeaponStats)
        {
            SO_MeleeWeapon weapon = item.SO_Item as SO_MeleeWeapon;
            MeleeWeapon weaponGameObject = weapon.Usable_GameObject.GetComponent<MeleeWeapon>();

            EditorUtility.SetDirty(weapon);
            EditorUtility.SetDirty(weaponGameObject.gameObject);

            weapon.WeaponParameters = new FWeaponParameters(item.EffectType, item.WeaponType, item.WeaponSize);
            weapon.MeleeWeaponParameters = new FMeleeWeaponParameters(item.Damage, item.Speed, item.CanPierce);

            ApplySoMeleeWeaponToPrefab(weapon, weaponGameObject);

            //save scriptable object 
        }

        //

        foreach (StaffWeaponListed item in StaffWeaponStats)
        {
            SO_StaffWeapon staff = item.SO_Item as SO_StaffWeapon;
            StaffWeapon staffGameObject = staff.Usable_GameObject.GetComponent<StaffWeapon>();
            EditorUtility.SetDirty(staff);
            EditorUtility.SetDirty(staffGameObject.gameObject);
            staff.WeaponParameters = new FWeaponParameters(item.EffectType, item.WeaponType, item.WeaponSize);
            ApplySoStaffWeaponToPrefab(staff, staffGameObject);


        }

        foreach (ShieldWeaponListed item in shieldWeaponsStats)
        {
            SO_ShieldWeapon shield = item.SO_Item as SO_ShieldWeapon;
            ShieldWeapon shieldGameObject = shield.Usable_GameObject.GetComponent<ShieldWeapon>();
            EditorUtility.SetDirty(shield);
            EditorUtility.SetDirty(shieldGameObject.gameObject);
            shield.WeaponParameters = new FWeaponParameters(item.EffectType, item.WeaponType, item.WeaponSize);
            ApplyShieldWeaponToPrefab(shield, shieldGameObject);

        }
    }
    void ApplySoMeleeWeaponToPrefab(SO_MeleeWeapon so_Weapon, MeleeWeapon weaponGameObject)
    {
        weaponGameObject.WeaponParameters = so_Weapon.WeaponParameters;
        weaponGameObject.MeleeWeaponParameters = so_Weapon.MeleeWeaponParameters;
        weaponGameObject.WeaponEffects = so_Weapon.WeaponEffects;
        weaponGameObject.HitSounds = so_Weapon.HitSounds;
        weaponGameObject.m_ParryAudios = so_Weapon.ParrySounds;
    }

    void ApplySoStaffWeaponToPrefab(SO_StaffWeapon so_Weapon, StaffWeapon weaponGameObject)
    {
        weaponGameObject.WeaponParameters = so_Weapon.WeaponParameters;
        weaponGameObject.WeaponEffects = so_Weapon.WeaponEffects;

    }

    void ApplyShieldWeaponToPrefab(SO_ShieldWeapon so_Shield, ShieldWeapon shieldGameObject)
    {
        shieldGameObject.WeaponParameters = so_Shield.WeaponParameters;
        shieldGameObject.WeaponEffects = so_Shield.WeaponEffects;
        shieldGameObject.m_ParryAudios = so_Shield.ParrySounds;
    }

    

    [Serializable]
    public class WeaponListed : ItemsListed
    {
        [HideInTables]
        public EWeaponType WeaponType;

        public EWeaponSize WeaponSize;
        public EEffectType EffectType;


        [HideInTables]
        [TableList(AlwaysExpanded = false, DrawScrollView = false)]
        public List<SO_WeaponEffect> WeaponEffects = new List<SO_WeaponEffect>();


        [Button(ButtonSizes.Small), HorizontalGroup("WeaponEffects"), HideIf("@WeaponEffects.Count <= 0")]
        private void See()
        {
            var window = OdinEditorWindow.InspectObject(WeaponEffects);
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(270, 200);

        }


        public WeaponListed(SO_Weapon so_item)
        {
            SO_Item = so_item;
            UsableGameObject = so_item.Usable_GameObject;
            Icon = so_item.ItemIcon;
            ItemName = so_item.ItemName;
            ItemRarity = so_item.EItemRarity;
            WeaponType = so_item.WeaponParameters.WeaponType;
            WeaponSize = so_item.WeaponParameters.WeaponSize;
            EffectType = so_item.WeaponParameters.DamageType;
            WeaponEffects = so_item.WeaponEffects;
        }


    }

    [Serializable]
    public class MeleeWeaponListed : WeaponListed
    {
        public float Speed;
        public int Damage;
        public bool CanPierce;

        public MeleeWeaponListed(SO_MeleeWeapon so_item) : base(so_item)
        {

            Speed = so_item.MeleeWeaponParameters.Speed;
            Damage = so_item.MeleeWeaponParameters.Damage;
            CanPierce = so_item.MeleeWeaponParameters.CanPierce;

        }
    }

    [Serializable]
    public class StaffWeaponListed : WeaponListed
    {


        public StaffWeaponListed(SO_StaffWeapon so_item) : base(so_item)
        {



        }
    }

    [Serializable]
    public class ShieldWeaponListed : WeaponListed
    {


        public ShieldWeaponListed(SO_ShieldWeapon so_item) : base(so_item)
        {



        }
    }

    [Serializable]
    public class RangedWeaponListed : WeaponListed
    {


        public RangedWeaponListed(SO_ShieldWeapon so_item) : base(so_item)
        {



        }
    }



}

[HideLabel]
[Serializable]
public class ArmorVisualizer
{
    [TableList(AlwaysExpanded = false, DrawScrollView = false), Searchable]
    public List<ArmorListed> ArmorStats = new List<ArmorListed>();

    public void CreateVisualizer(List<SO_Item> list)
    {
        ArmorStats.Clear();
        foreach (SO_Item item in list)
        {
            if (item is SO_Armor armor)
            {
                ArmorListed armorListed = new ArmorListed(armor);
                ArmorStats.Add(armorListed);
            }

        }

    }

    [Button("Apply Changes")]
    private void ApplyChanges()
    {
        ApplyArmorChanges();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void ApplyArmorChanges()
    {
        foreach (ArmorListed item in ArmorStats)
        {
            SO_Armor armor = item.SO_Item as SO_Armor;
            Armor armorGameObject = armor.Usable_GameObject.GetComponent<Armor>();

            EditorUtility.SetDirty(armor);
            EditorUtility.SetDirty(armorGameObject.gameObject);
            armor.MagicArmor = item.MagicArmorValue;
            armor.Armor = item.ArmorValue;
            armor.ArmorType = item.ArmorType;
            armor.ArmorPlace = item.ArmorPlace;
            armor.ArmorEffects = item.ArmorEffects;
            

            ApplySoArmorToPrefab(armor, armorGameObject);

            //save scriptable object 
        }
    }

    void ApplySoArmorToPrefab(SO_Armor so_Armor, Armor armorGameObject)
    {
        armorGameObject.ArmorEffects = so_Armor.ArmorEffects;
        armorGameObject.ArmorParameters = new FArmorParameters(so_Armor.MagicArmor, so_Armor.Armor, so_Armor.ArmorType, so_Armor.ArmorPlace); 


    }

    [Serializable]
    public class ArmorListed : ItemsListed
    {

        public EArmorType ArmorType;
        public EArmorPlace ArmorPlace; 
        public int ArmorValue;
        public int MagicArmorValue; 

        [HideInTables]
        [TableList(AlwaysExpanded = false, DrawScrollView = false)]
        public List<SO_ArmorEffect> ArmorEffects = new List<SO_ArmorEffect>();


        [Button(ButtonSizes.Small), HorizontalGroup("ArmorEffects"), HideIf("@ArmorEffects.Count <= 0")]
        private void See()
        {
            var window = OdinEditorWindow.InspectObject(ArmorEffects);
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(270, 200);

        }


        public ArmorListed(SO_Armor so_item)
        {
            SO_Item = so_item;
            UsableGameObject = so_item.Usable_GameObject;
            ArmorPlace = so_item.ArmorPlace;
            Icon = so_item.ItemIcon;
            ItemName = so_item.ItemName;
            ItemRarity = so_item.EItemRarity;
            ArmorType = so_item.ArmorType;
            ArmorValue = so_item.Armor;
            MagicArmorValue = so_item.MagicArmor;
            ArmorEffects = so_item.ArmorEffects;

        }


    }
}

[HideLabel]
[Serializable]
public class InventoryItemVisualizer
{
    [TableList(AlwaysExpanded = false, DrawScrollView = false), Searchable]
    [GUIColor("#f2b3db")]
    public List<PotionItemListed> PotionStats = new List<PotionItemListed>();
    [TableList(AlwaysExpanded = false, DrawScrollView = false), Searchable]
    [GUIColor("#f2e4b3")]
    public List<ScrollItemListed> ScrollStats = new List<ScrollItemListed>();

    public void CreateVisualizer(List<SO_Item> list)
    {
        PotionStats.Clear();
        ScrollStats.Clear();

        foreach (SO_Item item in list)
        {
            if (item is SO_Potion potion)
            {
                PotionItemListed potionListed = new PotionItemListed(potion);
                PotionStats.Add(potionListed);
            }
            if(item is SO_Scroll scroll)
            {
                ScrollItemListed scrollListed = new ScrollItemListed(scroll);
                ScrollStats.Add(scrollListed);
            }

        }

    }

    [Button("Apply Changes")]
    private void ApplyChanges()
    {
        ApplyInventoryItemsChanges();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void ApplyInventoryItemsChanges()
    {
        foreach (PotionItemListed item in PotionStats)
        {
            SO_Potion potion = item.SO_Item as SO_Potion;
            Potion potionGameObject = potion.Usable_GameObject.GetComponent<Potion>();

            EditorUtility.SetDirty(potion);
            EditorUtility.SetDirty(potionGameObject.gameObject);
            potion.Throwable = item.Throwable;
            potion.ThrowForce = item.ThrowForce;
            potion.UseTime = item.UseTime;
            potion.ExplosionRadius = item.ExplosionRadius;
            potion.HitMask = item.HitMask;
            potion.GameEffectContainer = item.GameEffectContainer;
            potion.Charges = item.Charges;
            potion.ThrowableGameObject = item.ThrowableGameObject;
            potion.ExplosionEffect = item.ExplosionEffect;
            potion.DrinkEffect = item.DrinkEffect;

            ApplySoPotionToPrefab(potion, potionGameObject);

            //save scriptable object 
        }

        foreach (ScrollItemListed item in ScrollStats)
        {
            SO_Scroll scroll = item.SO_Item as SO_Scroll;
            Scroll scrollGameObject = scroll.Usable_GameObject.GetComponent<Scroll>();
            EditorUtility.SetDirty(scroll);
            EditorUtility.SetDirty(scrollGameObject.gameObject);
            scroll.UseTime = item.UseTime;
            scroll.Charges = item.Charges;

            ApplySoScrollToPrefab(scroll, scrollGameObject);
        }
    }

    void ApplySoPotionToPrefab(SO_Potion so_Potion, Potion potionGameObject)
    {
        
        potionGameObject.Throwable = so_Potion.Throwable;
        potionGameObject.UseAmount = so_Potion.Charges;



    }

    void ApplySoScrollToPrefab(SO_Scroll so_Scroll, Scroll scrollGameObject)
    {

        scrollGameObject.UseAmount = so_Scroll.Charges;


    }   


    [Serializable]
    public class PotionItemListed : ItemsListed
    {

        public bool Throwable; 
        public float ThrowForce;
        public float UseTime; 
        public float ExplosionRadius;
        public LayerMask HitMask;
        public SO_GameEffect_Container GameEffectContainer;
        public int Charges; 
        public GameObject ThrowableGameObject;
        public GameObject ExplosionEffect;
        public GameObject DrinkEffect;


        public PotionItemListed( SO_Potion so_Potion)
        {
            SO_Item = so_Potion;
            UsableGameObject = so_Potion.Usable_GameObject;
            Throwable = so_Potion.Throwable;
            ThrowForce = so_Potion.ThrowForce;
            UseTime = so_Potion.UseTime;
            ExplosionRadius = so_Potion.ExplosionRadius;
            HitMask = so_Potion.HitMask;
            GameEffectContainer = so_Potion.GameEffectContainer;
            Charges = so_Potion.Charges;
            ThrowableGameObject = so_Potion.ThrowableGameObject;
            ExplosionEffect = so_Potion.ExplosionEffect;
            DrinkEffect = so_Potion.DrinkEffect;

        }


    }
    [Serializable]
    public class ScrollItemListed : ItemsListed
    {
        

        [HideInTables]
        [TableList(AlwaysExpanded = false, DrawScrollView = false)]
        public List<SO_ScrollEffect> ScrollEffects = new List<SO_ScrollEffect>();
        public float UseTime;
        public int Charges;



        [Button(ButtonSizes.Small), HorizontalGroup("ScrollEffects"), HideIf("@ScrollEffects.Count <= 0")]
        private void See()
        {
            var window = OdinEditorWindow.InspectObject(ScrollEffects);
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(270, 200);

        }

        public ScrollItemListed(SO_Scroll so_item)
        {
            SO_Item = so_item;
            Icon = so_item.ItemIcon;
            ItemName = so_item.ItemName;
            ItemRarity = so_item.EItemRarity;
            UsableGameObject = so_item.Usable_GameObject;
            Icon = so_item.ItemIcon;
            ItemName = so_item.ItemName;
            ItemRarity = so_item.EItemRarity;
            Charges = so_item.Charges;
            UseTime = so_item.UseTime;
            ScrollEffects = so_item.ScrollEffects;

        }

    }
}



