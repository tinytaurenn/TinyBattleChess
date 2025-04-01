using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BasicWeapon),true)]
public class WeaponEditor : Editor
{
    SerializedProperty So_ItemProperty;

    private void OnEnable()
    {
        So_ItemProperty = serializedObject.FindProperty("m_SO_Item");
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update(); 
        if(So_ItemProperty.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("Please assign a SO_Item to this weapon", MessageType.Warning);
        }
        else
        {
            if (GUILayout.Button("Setup Weapon Prefabs"))
            {
                SetupWeaponPrefabs();
            }
            if(GUILayout.Button("Update Weapon from SO_Item"))
            {
                
            }
        }
        serializedObject.ApplyModifiedProperties();

       

        
    }

    void SetupWeaponPrefabs()
    {
        Debug.Log("Setting up weapon prefabs");

        GameObject prefabObj = ((BasicWeapon)target).gameObject;

        GameObject instancedPrefabObj = PrefabUtility.InstantiatePrefab(prefabObj) as GameObject;


        if(instancedPrefabObj ==null)
        {
            Debug.LogError("Failed to instantiate prefab");
            return;
        }


        if(PrefabUtility.IsPartOfPrefabAsset(prefabObj))
        {
            Debug.Log("part of prefab");

        }

        //main 
        if(instancedPrefabObj.GetComponent<Collider>() == null)
        {
            Debug.Log("no collider");

            BoxCollider boxColl =  instancedPrefabObj.AddComponent<BoxCollider>();
            boxColl.enabled = false; 


        }
        else
        {
            Debug.Log("got main collider");
        }

        if(instancedPrefabObj.GetComponent<Rigidbody>() == null)
        {
            Debug.Log("no rigidbody");

            Rigidbody rb = instancedPrefabObj.AddComponent<Rigidbody>();
            rb.isKinematic = true; 
        }
        else
        {
            Debug.Log("got main rigidbody");
        }

        if(instancedPrefabObj.GetComponent<AudioSource>() == null)
        {

            Debug.Log("no audio source");

            AudioSource audioSource = instancedPrefabObj.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1;
            audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
        }
        else
        {
            Debug.Log("got main audio source");
        }

        //

        

        string prefabPath = AssetDatabase.GetAssetPath(prefabObj);

        PrefabUtility.SaveAsPrefabAssetAndConnect(instancedPrefabObj, prefabPath, InteractionMode.AutomatedAction);

        DestroyImmediate(instancedPrefabObj);

        AssetDatabase.Refresh();
    }

}
