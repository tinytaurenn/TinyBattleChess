using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEffectsContainers", menuName = "Scriptable Objects/GameEffectsContainers")]
public class GameEffectsContainers : ScriptableObject
{
    public List<SO_GameEffect_Container> GameEffectContainers; 
}
