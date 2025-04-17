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

        visualizer.FindScriptableObjects(); 
        return tree;

        
    }
}

[HideLabel]
[Serializable]
public class ItemVisualizer
{


    [ReadOnly]
    [ListDrawerSettings()]
    public List<ScriptableObject> FoundScriptableObjects = new List<ScriptableObject>();


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
    

    //example
    private string Test3
    {
        get
        {
            return EditorPrefs.GetString("OdinDemo.PersistentString",
                "This value is persistent forever, even cross Unity projects. But it's not saved together " +
                "with your project. That's where ScriptableObejcts and OdinEditorWindows come in handy.");
        }
        set
        {
            EditorPrefs.SetString("OdinDemo.PersistentString", value);
        }
    }
}
