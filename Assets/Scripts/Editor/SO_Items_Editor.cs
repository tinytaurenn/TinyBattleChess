using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SO_Items))]
public class SO_Items_Editor : Editor
{
    //draw a button in the inspector that run a function
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SO_Items soItems = (SO_Items)target;

        if (GUILayout.Button("Retrieve Items"))
        {

            //search for all So_Item in Assets/
            string[] guids = AssetDatabase.FindAssets("t:SO_Item", new[] { "Assets/SO" }); 
            //add all SO_Item to a list
            soItems.Items.Clear();
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                SO_Item item = AssetDatabase.LoadAssetAtPath<SO_Item>(path);
                soItems.Items.Add(item);
            }


        }
    }
}
