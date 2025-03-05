
using Coherence.Toolkit;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ItemEditor : EditorWindow
{
    public SO_Item SO_Item;
    public Mesh itemMesh;
    string matPath = "Assets/Art/DEBUG/M_ArenaDebug.mat";
    Material baseMaterial; 



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

        baseMaterial = AssetDatabase.LoadAssetAtPath<Material>(matPath);

        if (SO_Item == null)
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


        GameObject weaponGameObject = new GameObject(itemName);
        weaponGameObject.layer = LayerMask.NameToLayer("Usable");
        
        
        //parent.AddComponent(typeof(BasicWeapon));
        BasicWeapon weaponScript =  weaponGameObject.AddComponent<BasicWeapon>();
        Debug.Log($"Added '{typeof(BasicWeapon)}' script to {itemName}");





        

        // Add Collider
        BoxCollider boxColl =  weaponGameObject.AddComponent<BoxCollider>();
        boxColl.enabled = false;    

        Rigidbody rb = weaponGameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        CoherenceSync coherenceSync = weaponGameObject.AddComponent<CoherenceSync>();
        coherenceSync.simulationType = CoherenceSync.SimulationType.ClientSide;
        coherenceSync.lifetimeType = CoherenceSync.LifetimeType.Persistent;
        coherenceSync.orphanedBehavior = CoherenceSync.OrphanedBehavior.AutoAdopt;
        coherenceSync.uniquenessType = CoherenceSync.UniquenessType.AllowDuplicates;
        coherenceSync.authorityTransferType = CoherenceSync.AuthorityTransferType.Request;
        coherenceSync.approveAuthorityTransferRequests = true;

        

        weaponGameObject.AddComponent<CoherenceNode>(); 

        AudioSource audioSource = weaponGameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1;
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic;

        // Store visuals 

        GameObject weaponMesh= new GameObject("WeaponMesh");

        weaponMesh.transform.parent = weaponGameObject.transform;

        MeshFilter meshFilter = weaponMesh.AddComponent<MeshFilter>();
        if(itemMesh != null)
        {
            meshFilter.mesh = itemMesh;
        }
        

        MeshRenderer meshRenderer = weaponMesh.AddComponent<MeshRenderer>();
        meshRenderer.material = baseMaterial;

        //triggers

        GameObject DamageCollider = new GameObject("DamageCollider");
        DamageCollider.transform.parent = weaponGameObject.transform;
        BoxCollider boxCollider = DamageCollider.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;


        weaponScript.SO_Item = SO_Item;
        weaponScript.m_IsHeld = false;
        weaponScript.IsNPCHeld = false;

        SO_Weapon soWeapon = SO_Item as SO_Weapon;
        BasicWeapon.FWeaponParameters weaponParameters = new BasicWeapon.FWeaponParameters(soWeapon.Damage,soWeapon.Speed,soWeapon.Cost,soWeapon.WeaponType,soWeapon.WeaponSize);

        weaponScript.m_ParryAudios = soWeapon.ParrySounds;
        weaponScript.HitSounds = soWeapon.HitSounds;



        weaponScript.SetupWeapon(boxCollider, weaponParameters); 





        // Save prefab to Assets folder
        string path = $"Assets/Prefabs/Weapons/Usable/{itemName}.prefab";
        var prefab = PrefabUtility.SaveAsPrefabAsset(weaponGameObject, path);

        SO_Item.Usable_GameObject = prefab;

        // Clean up the scene by destroying the temporary object
        DestroyImmediate(weaponGameObject);

        // Refresh the asset database
        

        Debug.Log($"Prefab '{itemName}' created with Collider and '{typeof(BasicWeapon)}' script at {path}");

        //
        //creating store prefab 
        //

        GameObject storeItem = new GameObject("store " + itemName);
        storeItem.layer = LayerMask.NameToLayer("Store");

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
        storeMeshRenderer.material = baseMaterial;

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

        string itemName = SO_Item.ItemName;

        GameObject armorItem = new GameObject(itemName);
        armorItem.layer = LayerMask.NameToLayer("Usable");

        SO_Armor sO_Armor = SO_Item as SO_Armor;

        Armor armorScript = null; 
        switch (sO_Armor.ArmorPlace)
        {
            case SO_Armor.EArmorPlace.Helmet:

                armorScript = armorItem.AddComponent<Head_Armor>();
                GameObject helmetMesh = new GameObject("Helmet_Mesh");
                helmetMesh.transform.parent = armorItem.transform;

                helmetMesh.AddComponent<MeshFilter>().mesh = itemMesh;
                helmetMesh.AddComponent<MeshRenderer>().material = baseMaterial;


                break;
            case SO_Armor.EArmorPlace.Chest:
                armorScript = armorItem.AddComponent<Chest_Armor>();
                GameObject chestMesh = new GameObject("Chest_Mesh");
                chestMesh.transform.parent = armorItem.transform;
                chestMesh.AddComponent<MeshFilter>().mesh = itemMesh;
                chestMesh.AddComponent<MeshRenderer>().material = baseMaterial;

                break;
            case SO_Armor.EArmorPlace.Shoulders:
                armorScript = armorItem.AddComponent<Shoulders_Armor>();
                //add other shoulders
                

                GameObject leftShoulderMesh = new GameObject("LeftShoulder_Mesh");
                GameObject rightShoulderMesh = new GameObject("RightShoulder_Mesh");
                leftShoulderMesh.transform.parent = armorItem.transform;
                rightShoulderMesh.transform.parent = armorItem.transform;
                leftShoulderMesh.AddComponent<MeshFilter>().mesh = itemMesh;
                rightShoulderMesh.AddComponent<MeshFilter>().mesh = itemMesh;
                leftShoulderMesh.AddComponent<MeshRenderer>().material = baseMaterial;
                rightShoulderMesh.AddComponent<MeshRenderer>().material = baseMaterial; 

                Shoulders_Armor shoulders_Armor = armorScript as Shoulders_Armor;
                shoulders_Armor.m_LeftShoulderVisual = leftShoulderMesh.GetComponent<Renderer>();
                shoulders_Armor.m_RightShoulderVisual = rightShoulderMesh.GetComponent<Renderer>();



                //
                GameObject LeftShoulder = new GameObject(itemName + "_Left_Shoulder");
                GameObject RightShoulder = new GameObject(itemName + "_Right_Shoulder");

                LeftShoulder.AddComponent<MeshFilter>().mesh = itemMesh;
                RightShoulder.AddComponent<MeshFilter>().mesh = itemMesh;
                LeftShoulder.AddComponent<MeshRenderer>().material = baseMaterial;
                RightShoulder.AddComponent<MeshRenderer>().material = baseMaterial;
                CoherenceSync leftShoulderSync = LeftShoulder.AddComponent<CoherenceSync>(); 
                leftShoulderSync.simulationType = CoherenceSync.SimulationType.ClientSide;
                leftShoulderSync.lifetimeType = CoherenceSync.LifetimeType.SessionBased;
                leftShoulderSync.uniquenessType = CoherenceSync.UniquenessType.AllowDuplicates;
                leftShoulderSync.authorityTransferType = CoherenceSync.AuthorityTransferType.NotTransferable; 

                CoherenceSync rightShoulderSync = RightShoulder.AddComponent<CoherenceSync>();
                rightShoulderSync.simulationType = CoherenceSync.SimulationType.ClientSide;
                rightShoulderSync.lifetimeType = CoherenceSync.LifetimeType.SessionBased;
                rightShoulderSync.uniquenessType = CoherenceSync.UniquenessType.AllowDuplicates;
                rightShoulderSync.authorityTransferType = CoherenceSync.AuthorityTransferType.NotTransferable;
                LeftShoulder.AddComponent<CoherenceNode>(); 
                RightShoulder.AddComponent<CoherenceNode>();
                

                string leftShoulderPath = $"Assets/Prefabs/Armors/Usable/{itemName}_Left_Shoulder.prefab";
                string rightShoulderPath = $"Assets/Prefabs/Armors/Usable/{itemName}_Right_Shoulder.prefab";
                var leftShoulderPrefab = PrefabUtility.SaveAsPrefabAsset(LeftShoulder, leftShoulderPath);
                var rightShoulderPrefab = PrefabUtility.SaveAsPrefabAsset(RightShoulder, rightShoulderPath);

                shoulders_Armor.m_LeftShoulder = leftShoulderPrefab;
                shoulders_Armor.m_RightShoulder = rightShoulderPrefab;

                break;
            default:
                break;
        }
        armorScript.SO_Item = SO_Item;

        armorScript.m_IsHeld = false;
        armorScript.IsNPCHeld = false;
        armorScript.ArmorParameters = new Armor.FArmorParameters(sO_Armor.MagicArmor, sO_Armor.Armor, sO_Armor.Cost, sO_Armor.ArmorType, sO_Armor.ArmorPlace);


        Rigidbody rb =  armorItem.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        BoxCollider collider = armorItem.AddComponent<BoxCollider>();
        collider.enabled = false;
        CoherenceSync sync = armorItem.AddComponent<CoherenceSync>();
        sync.simulationType = CoherenceSync.SimulationType.ClientSide;
        sync.lifetimeType = CoherenceSync.LifetimeType.SessionBased;
        sync.uniquenessType = CoherenceSync.UniquenessType.AllowDuplicates;
        sync.authorityTransferType = CoherenceSync.AuthorityTransferType.Request;
        sync.approveAuthorityTransferRequests = true;

        armorItem.AddComponent<CoherenceNode>();

        

        //

        string path = $"Assets/Prefabs/Armors/Usable/{itemName}.prefab";
        var prefab = PrefabUtility.SaveAsPrefabAsset(armorItem, path);

        SO_Item.Usable_GameObject = prefab;

        // Clean up the scene by destroying the temporary object
        DestroyImmediate(armorItem);


        ////
        ///store prefab
        ///
        GameObject storeItem = new GameObject("store " + itemName);

        storeItem.layer = LayerMask.NameToLayer("Store");

        StoreItem storeItemScript = storeItem.AddComponent<StoreItem>();
        storeItemScript.m_Rotating = true;
        storeItemScript.m_RotationSpeed = 1;

        GameObject storeArmorMesh = new GameObject("ItemMesh");
        storeArmorMesh.transform.parent = storeItem.transform;
        storeArmorMesh.AddComponent<MeshFilter>().mesh = itemMesh;
        storeArmorMesh.AddComponent<MeshRenderer>().material = baseMaterial;

        if(sO_Armor.ArmorPlace == SO_Armor.EArmorPlace.Shoulders)
        {
            GameObject storeRightShoulder = new GameObject("RightShoulder_Mesh");
            storeRightShoulder.transform.parent = storeItem.transform;
            storeRightShoulder.AddComponent<MeshFilter>().mesh = itemMesh;
            storeRightShoulder.AddComponent<MeshRenderer>().material = baseMaterial;
        }




        string storeItemname = "store " + itemName;
        string storePath = $"Assets/Prefabs/Armors/Store/{storeItemname}.prefab";
        var storePrefab = PrefabUtility.SaveAsPrefabAsset(storeItem, storePath);

        SO_Item.Chest_GameObject = storePrefab;

        DestroyImmediate(storeItem);

        AssetDatabase.Refresh();

    }
    void CreatePotion()
    {
        string itemName = SO_Item.ItemName;
        SO_Potion soPotion = SO_Item as SO_Potion;

        GameObject potionItem = new GameObject(itemName);
        potionItem.layer = LayerMask.NameToLayer("Usable");
        //
        GameObject potionMesh = new GameObject("Potion_Mesh");
        potionMesh.transform.parent = potionItem.transform;
        MeshFilter meshFilter = potionMesh.AddComponent<MeshFilter>();
        meshFilter.mesh = itemMesh;
        Renderer renderer = potionMesh.AddComponent<MeshRenderer>();
        renderer.material = baseMaterial;
        Potion potionScript = potionItem.AddComponent<Potion>();
        potionScript.PotionEffects = soPotion.Effects; 
        potionScript.PotionCharges = soPotion.Charges;
        potionScript.SO_Item = SO_Item;
        potionScript.m_IsHeld = false;
        potionScript.IsNPCHeld = false;
        //potionScript.renderer = renderer;
        CoherenceSync sync = potionItem.AddComponent<CoherenceSync>();
        sync.simulationType = CoherenceSync.SimulationType.ClientSide;
        sync.lifetimeType = CoherenceSync.LifetimeType.Persistent;
        sync.orphanedBehavior = CoherenceSync.OrphanedBehavior.AutoAdopt;
        sync.uniquenessType = CoherenceSync.UniquenessType.AllowDuplicates;
        sync.authorityTransferType = CoherenceSync.AuthorityTransferType.Request;
        sync.approveAuthorityTransferRequests = true;
        potionScript.AddComponent<CoherenceNode>(); 
        Rigidbody rigidbody = potionItem.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        BoxCollider collider = potionItem.AddComponent<BoxCollider>();
        collider.enabled = false;


        //
        string path = $"Assets/Prefabs/Potions/Usable/{itemName}.prefab";
        var prefab = PrefabUtility.SaveAsPrefabAsset(potionItem, path);

        SO_Item.Usable_GameObject = prefab;

        // Clean up the scene by destroying the temporary object
        DestroyImmediate(potionItem);

        GameObject storeItem = new GameObject("store " + itemName);
        storeItem.layer = LayerMask.NameToLayer("Store");

        StoreItem storeItemScript = storeItem.AddComponent<StoreItem>();
        GameObject storePotionMesh = new GameObject("Potion_Mesh");
        storePotionMesh.transform.parent = storeItem.transform;

        storePotionMesh.AddComponent<MeshFilter>().mesh = itemMesh;
        storePotionMesh.AddComponent<MeshRenderer>().material = baseMaterial;

        storeItemScript.m_Rotating = true;
        storeItemScript.m_RotationSpeed = 1;

        string storeItemname = "store " + itemName;
        string storePath = $"Assets/Prefabs/Potions/Store/{storeItemname}.prefab";
        var storePrefab = PrefabUtility.SaveAsPrefabAsset(storeItem, storePath);

        SO_Item.Chest_GameObject = storePrefab;

        DestroyImmediate(storeItem);


        AssetDatabase.Refresh();
    }
    void CreateScroll()
    {
        string itemName = SO_Item.ItemName;

        SO_Scroll soScroll = SO_Item as SO_Scroll;

        GameObject ScrollItem = new GameObject(itemName);
        ScrollItem.layer = LayerMask.NameToLayer("Usable");
        Scroll scrollScript = ScrollItem.AddComponent<Scroll>();
        //
        GameObject scrollMesh = new GameObject("Scroll_Mesh");
        scrollMesh.transform.parent = ScrollItem.transform;

        MeshFilter meshFilter = scrollMesh.AddComponent<MeshFilter>();
        meshFilter.mesh = itemMesh;
        Renderer renderer = scrollMesh.AddComponent<MeshRenderer>();
        renderer.material = baseMaterial;
        scrollScript.ScrollElems = soScroll.ScrollElem; 
        scrollScript.ScrollCharges = soScroll.Charges;
        scrollScript.SO_Item = SO_Item;
        scrollScript.m_IsHeld = false;
        scrollScript.IsNPCHeld = false;
        //potionScript.renderer = renderer;
        CoherenceSync sync = ScrollItem.AddComponent<CoherenceSync>();
        sync.simulationType = CoherenceSync.SimulationType.ClientSide;
        sync.lifetimeType = CoherenceSync.LifetimeType.Persistent;
        sync.orphanedBehavior = CoherenceSync.OrphanedBehavior.AutoAdopt;
        sync.uniquenessType = CoherenceSync.UniquenessType.AllowDuplicates;
        sync.authorityTransferType = CoherenceSync.AuthorityTransferType.Request;
        sync.approveAuthorityTransferRequests = true;
        scrollScript.AddComponent<CoherenceNode>();
        Rigidbody rigidbody = ScrollItem.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        BoxCollider collider = ScrollItem.AddComponent<BoxCollider>();
        collider.enabled = false;


        //
        string path = $"Assets/Prefabs/Scrolls/Usable/{itemName}.prefab";
        var prefab = PrefabUtility.SaveAsPrefabAsset(ScrollItem, path);

        SO_Item.Usable_GameObject = prefab;

        // Clean up the scene by destroying the temporary object
        DestroyImmediate(ScrollItem);

        GameObject storeItem = new GameObject("store " + itemName);
        storeItem.layer = LayerMask.NameToLayer("Store");
        StoreItem storeItemScript = storeItem.AddComponent<StoreItem>();
        GameObject storeScrollMesh = new GameObject("Scroll_Mesh");
        storeScrollMesh.transform.parent = storeItem.transform;

        storeScrollMesh.AddComponent<MeshFilter>().mesh = itemMesh;
        storeScrollMesh.AddComponent<MeshRenderer>().material = baseMaterial;

        storeItemScript.m_Rotating = true;
        storeItemScript.m_RotationSpeed = 1;
        string storeItemname = "store " + itemName;

        string storePath = $"Assets/Prefabs/Scrolls/Store/{storeItemname}.prefab";
        var storePrefab = PrefabUtility.SaveAsPrefabAsset(storeItem, storePath);

        SO_Item.Chest_GameObject = storePrefab;

        DestroyImmediate(storeItem);


        AssetDatabase.Refresh();

    }


}
