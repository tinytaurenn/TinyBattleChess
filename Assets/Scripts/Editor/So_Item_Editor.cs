using UnityEditor;

using UnityEngine;
[CustomEditor(typeof(SO_Item),true)]

public class So_Item_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Create Item Prefabs"))
        {
            ItemEditor.OpenWithSOItem((SO_Item)target);
        }
    }
}
