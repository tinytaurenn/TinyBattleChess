using System.Collections;
using UnityEngine;

public class Scroll : InventoryItem
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

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

    public override bool UseInventoryItem(Transform parentTransform, Vector3 dir)
    {

        Debug.Log("using Scroll");

        if (UseAmount <= 0) return false;

        UseAmount--;

        SO_Scroll sO_Scroll = So_Item as SO_Scroll;

        foreach (SO_ScrollEffect effect in sO_Scroll.ScrollEffects)
        {
            effect.OnActivate(parentTransform,dir);
        }
        StartCoroutine(UsingMagicRoutine());

        return true; 

    }

    public override void SetupItem()
    {
        base.SetupItem();
    }

    IEnumerator UsingMagicRoutine()
    {
        
        yield return new WaitForSeconds(((SO_Scroll)So_Item).UseTime);
        OnUsedItem(); 
    }
}
