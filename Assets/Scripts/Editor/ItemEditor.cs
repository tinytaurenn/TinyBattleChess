
using Coherence.Toolkit;
using UnityEditor;
using UnityEngine;

public class ItemEditor : EditorWindow
{
    public SO_Item SO_Item;
    public Mesh itemMesh; 
    

    [MenuItem("TinyTools/Create Item from scriptable object")]
    public static void ShowWindow()
    {
        GetWindow<ItemEditor>("Item Maker");
    }

    public static void OpenWithSOItem(SO_Item soItem)
    {
        ItemEditor window = GetWindow<ItemEditor>("Item Maker");
        window.SO_Item = soItem;
        window.Show();
    }

    void OnGUI()
    {

        GUILayout.Label("Create Item from a scriptable object", EditorStyles.boldLabel);

        SO_Item = (SO_Item)EditorGUILayout.ObjectField("Item", SO_Item, typeof(SO_Item), false);
        itemMesh = (Mesh)EditorGUILayout.ObjectField("Item Mesh", itemMesh, typeof(Mesh), false);

       if(SO_Item == null)
        {
            EditorGUILayout.HelpBox("Please assign a SO_Item ", MessageType.Warning);
            return; 
        }

        if (SO_Item.GetType() == typeof(SO_Weapon))
        {
            if (GUILayout.Button("Create Weapon"))
            {
                CreateWeapon();
            }
        }
        else if (SO_Item.GetType() == typeof(SO_Armor))
        {
            if (GUILayout.Button("Create Armor"))
            {
                CreateArmor();
            }
        }
        else if (SO_Item.GetType() == typeof(SO_Potion))
        {
            if (GUILayout.Button("Create Potion"))
            {
                CreatePotion();
            }
        }
        else if (SO_Item.GetType() == typeof(SO_Scroll))
        {
            if (GUILayout.Button("Create Scroll"))
            {
                CreateScroll();
            }
        }
        //runes ? 
        


    }

    void CreateWeapon()
    {
        Debug.Log("Creating item");

        string itemName = SO_Item.ItemName;


        GameObject UsableItem = new GameObject(itemName);

        //parent.AddComponent(typeof(BasicWeapon));
        BasicWeapon weaponScript =  UsableItem.AddComponent<BasicWeapon>();
        Debug.Log($"Added '{typeof(BasicWeapon)}' script to {itemName}");


        

        // Add Collider
        BoxCollider boxColl =  UsableItem.AddComponent<BoxCollider>();
        boxColl.enabled = false;    

        Rigidbody rb = UsableItem.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        CoherenceSync coherenceSync = UsableItem.AddComponent<CoherenceSync>();
        coherenceSync.simulationType = CoherenceSync.SimulationType.ClientSide;
        coherenceSync.lifetimeType = CoherenceSync.LifetimeType.Persistent;
        coherenceSync.orphanedBehavior = CoherenceSync.OrphanedBehavior.AutoAdopt;
        coherenceSync.uniquenessType = CoherenceSync.UniquenessType.AllowDuplicates;
        coherenceSync.authorityTransferType = CoherenceSync.AuthorityTransferType.Request;

        

        UsableItem.AddComponent<CoherenceNode>(); 

        AudioSource audioSource = UsableItem.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;

        // visuals

        GameObject weaponMesh= new GameObject("WeaponMesh");

        weaponMesh.transform.parent = UsableItem.transform;

        MeshFilter meshFilter = weaponMesh.AddComponent<MeshFilter>();
        if(itemMesh != null)
        {
            meshFilter.mesh = itemMesh;
        }
        

        MeshRenderer meshRenderer = weaponMesh.AddComponent<MeshRenderer>();

        //triggers

        GameObject DamageCollider = new GameObject("DamageCollider");
        DamageCollider.transform.parent = UsableItem.transform;
        BoxCollider boxCollider = DamageCollider.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;


        weaponScript.SO_Item = SO_Item;
        weaponScript.m_IsHeld = false;

        SO_Weapon soWeapon = SO_Item as SO_Weapon;
        BasicWeapon.FWeaponParameters weaponParameters = new BasicWeapon.FWeaponParameters(soWeapon.Damage,soWeapon.Speed,soWeapon.Cost,soWeapon.WeaponType,soWeapon.WeaponSize);

        weaponScript.SetupWeapon(boxCollider, weaponParameters); 




        // Save prefab to Assets folder
        string path = $"Assets/Prefabs/Weapons/Usable/{itemName}.prefab";
        var prefab = PrefabUtility.SaveAsPrefabAsset(UsableItem, path);

        SO_Item.Usable_GameObject = prefab;

        // Clean up the scene by destroying the temporary object
        DestroyImmediate(UsableItem);

        // Refresh the asset database
        

        Debug.Log($"Prefab '{itemName}' created with Collider and '{typeof(BasicWeapon)}' script at {path}");

        //
        //creating store prefab 
        //

        GameObject storeItem = new GameObject("store " + itemName);

        StoreItem storeItemScript = storeItem.AddComponent<StoreItem>();
        storeItemScript.m_Rotating = true;
        storeItemScript.m_RotationSpeed = 1;

        GameObject storeWeaponMesh = new GameObject("WeaponMesh");

        storeWeaponMesh.transform.parent = storeItem.transform;

        MeshFilter storeMeshFilter = storeWeaponMesh.AddComponent<MeshFilter>();
        if (itemMesh != null)
        {
            storeMeshFilter.mesh = itemMesh;
        }


        MeshRenderer storeMeshRenderer = storeWeaponMesh.AddComponent<MeshRenderer>();

        string storeItemname = "store " + itemName;
        string storePath = $"Assets/Prefabs/Weapons/Store/{storeItemname}.prefab";
        var storePrefab = PrefabUtility.SaveAsPrefabAsset(storeItem, storePath);

        SO_Item.Chest_GameObject = storePrefab;

        DestroyImmediate(storeItem);


        //


        AssetDatabase.Refresh();



    }

    void CreateArmor()
    {
    }
    void CreatePotion()
    {

    }
    void CreateScroll()
    {

    }


}
