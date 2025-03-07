using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Potion : InventoryItem
{
    [SerializeField] bool m_Throwable = false;

    public bool Throwable { get { return m_Throwable; } set { m_Throwable = value; } }
    [SerializeField] List<FPotionEffect> m_PotionEffect;
    public List<FPotionEffect> PotionEffects { get { return m_PotionEffect; } set { m_PotionEffect = value; }  }
    [SerializeField] int m_PotionCharges = 1;
    public int PotionCharges { get { return m_PotionCharges; } set { m_PotionCharges = value; } }

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

    public override bool UseInventoryItem()
    {
        base.UseInventoryItem();

        if (m_IsUsed) return false; 
        if(PotionCharges <= 0 ) return false;

        m_IsUsed = true;
        PotionCharges--; 

        Debug.Log("using potion");
        if (Throwable)
        {
            //throw potion
        }
        else
        {
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

        foreach (FPotionEffect effect in PotionEffects)
        {
            ConnectionsHandler.Instance.LocalTinyPlayer.PotionEffect(effect.Effect, effect.Value, effect.EffectDuration);
        }
    }
    

    #region Potions Effects

    
    #endregion
}
