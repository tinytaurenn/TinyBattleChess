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

        SO_Potion sO_Potion = SO_Item as SO_Potion;

        GameObject drinkEffect = Instantiate(sO_Potion.DrinkEffect, transform.root.position, sO_Potion.DrinkEffect.transform.rotation,transform.root);

        ConnectionsHandler.Instance.LocalTinyPlayer.ApplyEffects(sO_Potion.GameEffectContainer.Effects);

        m_IsUsed = false;
        OnUsedItem();
    }

    public override void ThrowItem(Vector3 pos)
    {
        SO_Potion sO_Potion = SO_Item as SO_Potion;
        if(sO_Potion == null || sO_Potion.ThrowableGameObject == null)
        {
            Debug.Log("No Throwable GO");
            return; 
        }


        UseAmount--; 
        m_IsUsed = false; 
        
        PotionProjectile itemProjectile =  Instantiate(sO_Potion.ThrowableGameObject, transform.position, transform.rotation).GetComponent<PotionProjectile>();
        //itemProjectile.PotionEffects = PotionEffects;
        itemProjectile.m_ThrowForce = sO_Potion.ThrowForce;
        itemProjectile.m_ExplosionRadius = sO_Potion.ExplosionRadius;
        itemProjectile.SetupPotionProjectile((SO_Potion)SO_Item, m_MeshFilter.mesh,m_Renderer.material); 
        itemProjectile.Launch(pos);

        OnUsedItem();
        Debug.Log("Throwing potion");
    }


    #region Potions Effects


    #endregion

    private void OnDrawGizmos()
    {
        if(Throwable)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, ((SO_Potion) SO_Item).ExplosionRadius);
        }
    }
}
