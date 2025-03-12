using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Potion : InventoryItem
{
    
    //[SerializeField] List<FPotionEffect> m_PotionEffect;
    //public List<FPotionEffect> PotionEffects { get { return m_PotionEffect; } set { m_PotionEffect = value; }  }


    [SerializeField] bool m_IsUsed = false; 
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override bool UseInventoryItem(Transform parentTransform)
    {

        if (m_IsUsed) return false; 
        if(UseAmount <= 0 ) return false;

        m_IsUsed = true;
        

        Debug.Log("using potion");
        if (Throwable)
        {
            //aiming potion effect
        }
        else
        {
            UseAmount--;
            StartCoroutine(DrinkPotionRoutine(0.5f));
        }
        
        return true; 




    }

    public override void SetupItem()
    {
        base.SetupItem();
    }

    IEnumerator  DrinkPotionRoutine(float time)
    {

        yield return new WaitForSeconds(time);

        ConnectionsHandler.Instance.LocalTinyPlayer.ApplyEffects(((SO_Potion)SO_Item).Effects);

        m_IsUsed = false;
        OnUsedItem();
    }

    public override void ThrowItem(Vector3 pos)
    {
        if(ItemProjectile == null)
        {
            Debug.Log("No Throwable GO");
            return; 
        }


        UseAmount--; 
        m_IsUsed = false; 
        
        PotionProjectile itemProjectile =  Instantiate(ItemProjectile, transform.position, transform.rotation).GetComponent<PotionProjectile>();
        //itemProjectile.PotionEffects = PotionEffects;
        itemProjectile.SO_Potion = (SO_Potion)SO_Item;
        
        itemProjectile.Launch(pos);

        OnUsedItem();
        Debug.Log("Throwing potion");
    }


    #region Potions Effects


    #endregion
}
