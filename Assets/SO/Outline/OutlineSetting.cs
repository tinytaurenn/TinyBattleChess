using UnityEngine;

[CreateAssetMenu(fileName = "OutlineSetting", menuName = "Scriptable Objects/Settings/OutlineSetting")]
public class OutlineSetting : ScriptableObject
{
    public Outline.Mode OutlineMode = Outline.Mode.OutlineVisible;

    public Color OutlineColor = Color.white;

    [Range(0f, 10f)]
    public float OutlineWidth = 2f;
}
