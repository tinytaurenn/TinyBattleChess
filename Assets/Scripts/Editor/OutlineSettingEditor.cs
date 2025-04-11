using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(OutlineSetting), true)]
public class OutlineSettingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        OutlineSetting setting = (OutlineSetting)target;
        
        if (GUILayout.Button("Apply on every Usables"))
        {
            string[] guids = AssetDatabase.FindAssets("t:Object", new[] { "Assets/Prefabs" });
   
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                
                GameObject usable = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                
                if (usable != null)
                {
                    if(usable.TryGetComponent<Usable>(out Usable usableItem))
                    {
                        Debug.Log("found Usable : " + usable.name);
                        if(usableItem.m_OutLineSetting == null || usableItem.m_OutLineSetting != setting)
                        {
                            EditorUtility.SetDirty(usable);
                            usableItem.m_OutLineSetting = setting;
                        }
                        

                    }
                  
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
