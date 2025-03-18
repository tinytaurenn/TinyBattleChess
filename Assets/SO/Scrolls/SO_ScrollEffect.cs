using System.Collections.Generic;
using UnityEngine;

public abstract class SO_ScrollEffect : ScriptableObject

{

    //public string ScrollID; 

    public SO_GameEffect_Container SO_GameEffect_Container;
    public LayerMask HitMask; 
    public abstract void OnActivate(Transform parent);
}
