using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_GameEffect_Container", menuName = "Scriptable Objects/GameEffect/SO_GameEffect_Container")]
public class SO_GameEffect_Container : ScriptableObject
{
    public string GameEffectID;
    public List<FGameEffect> Effects;
}
