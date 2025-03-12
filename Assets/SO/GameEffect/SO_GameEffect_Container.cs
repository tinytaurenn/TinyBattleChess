using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_GameEffect_Container", menuName = "Scriptable Objects/SO_GameEffect_Container")]
public class SO_GameEffect_Container : ScriptableObject
{
    public List<FGameEffect> Effects;
}
