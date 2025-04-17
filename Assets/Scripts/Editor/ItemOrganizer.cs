using UnityEngine;
using Sirenix.OdinInspector.Editor;
using System.Linq;
using Sirenix.Utilities.Editor;
using Sirenix.Serialization;
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Demos;
using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector.Editor.Examples;
public class ItemOrganizer : OdinMenuEditorWindow
{

    [MenuItem("TinyTools/Item Organizer")]
    private static void OpenWindow()
    {
        var window = GetWindow<ItemOrganizer>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);

        
    }

    [SerializeField]
    private ItemVisualizer visualizer = new ItemVisualizer();

    protected override OdinMenuTree BuildMenuTree()
    {


        OdinMenuTree tree = new OdinMenuTree(supportsMultiSelect: true)
            {
                { "Home",                           this,                           EditorIcons.Pen                       }, // Draws the this.someData field in this case.
                { "Some Class",                     null                                                           }
            };

        tree.MenuItems.Insert(2, new OdinMenuItem(tree, "Menu Style", tree.DefaultMenuStyle));

        tree.SortMenuItemsByName();

        // As you can see, Odin provides a few ways to quickly add editors / objects to your menu tree.
        // The API also gives you full control over the selection, etc..
        // Make sure to check out the API Documentation for OdinMenuEditorWindow, OdinMenuTree and OdinMenuItem for more information on what you can do!

        //visualizer.FindScriptableObjects();
        return tree;

        
    }
    [OnInspectorInit]
    void OnOpenWindow()
    {
        visualizer.FindScriptableObjects();
        visualizer.CreateVisualizer(); 
    }
}

[HideLabel]
[Serializable]
public class ItemVisualizer
{

     // Take a look at SomeData.cs to see how serialization works in Editor Windows.

    [ReadOnly]
    [ListDrawerSettings()]
    public List<SO_Item> FoundScriptableObjects = new List<SO_Item>();

    [TableList(AlwaysExpanded = true, DrawScrollView = false)]
    public List<WeaponListed> WeaponStats = new List<WeaponListed>();


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

    public void CreateVisualizer()
    {
        foreach (SO_Item item in FoundScriptableObjects)
        {
            switch (item)
            {
                case SO_Weapon weapon:
                    WeaponListed weaponListed = new WeaponListed(item.ItemIcon, item.ItemName, item.EItemRarity, EWeaponType.Melee);
                    WeaponStats.Add(weaponListed);
                    break;
                case SO_Armor armor:
                    break;
                case SO_Potion potion:
                    break;
                case SO_Scroll scroll:
                    break;
                default:
                    break;
            }


            
        }
        
    }
    


}

[Serializable]
public class ItemsListed
{
    [TableColumnWidth(57, Resizable = false)]
    [PreviewField(Alignment = Sirenix.OdinInspector.ObjectFieldAlignment.Center)]
    public Sprite Icon;

    //[TextArea]
    public string ItemName;

    public EItemType ItemType;

    public EItemRarity ItemRarity;

    [ShowIf("ItemType", EItemType.Weapon)]
    public EWeaponType WeaponType;


    public ItemsListed(Sprite icon, string itemName, EItemType itemType, EItemRarity itemRarity, EWeaponType weaponType)
    {
        Icon = icon;
        ItemName = itemName;
        ItemType = itemType;
        ItemRarity = itemRarity;
        WeaponType = weaponType;
    }

    [OnInspectorInit]
    private void CreateData()
    {

    }
}

[Serializable]
public class WeaponListed
{
    [TableColumnWidth(57, Resizable = false)]
    [PreviewField(Alignment = Sirenix.OdinInspector.ObjectFieldAlignment.Center)]
    public Sprite Icon;

    //[TextArea]
    public string ItemName;

    public EItemRarity ItemRarity;

    public EWeaponType WeaponType;


    public WeaponListed(Sprite icon, string itemName, EItemRarity itemRarity, EWeaponType weaponType)
    {
        Icon = icon;
        ItemName = itemName;
        ItemRarity = itemRarity;
        WeaponType = weaponType;
    }

    [OnInspectorInit]
    private void CreateData()
    {

    }
}

