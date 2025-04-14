using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    [SerializeField] Image m_Border; 
    [SerializeField] Image m_Icon;

    public void ChangeIcon(Sprite Icon)
    {

        m_Icon.sprite = Icon;
    }

    public void SelectSlot()
    {
        m_Border.color = Color.green;
    }

    public void UnSelectSlot()
    {
        m_Border.color = Color.white;
    }

    public void ColorSlot(Color color)
    {
        m_Border.color = color; 
    }

}
