using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameEffectsContainers))]
public class GameEffectContainersEditor : Editor
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameEffectsContainers soGameEffect = (GameEffectsContainers)target;

        if (GUILayout.Button("Retrieve Effect"))
        {

            //search for all So_Item in Assets/
            string[] guids = AssetDatabase.FindAssets("t:SO_GameEffect_Container", new[] { "Assets/SO" });
            //add all SO_Item to a list
            soGameEffect.GameEffectContainers.Clear();
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                SO_GameEffect_Container effect = AssetDatabase.LoadAssetAtPath<SO_GameEffect_Container>(path);
                soGameEffect.GameEffectContainers.Add(effect);
            }
            EditorUtility.SetDirty(soGameEffect);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            


        }
    }
}
