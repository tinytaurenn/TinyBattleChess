using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class SO_Item : ScriptableObject
{
    public Sprite ItemIcon; 
   
    public string ItemName = "Basic Sword";

    public string ItemDescription;

    public GameObject Usable_GameObject;
    public GameObject Chest_GameObject;

    public int Cost = 10;

    public EItemRarity EItemRarity = EItemRarity.Common;

    




}
