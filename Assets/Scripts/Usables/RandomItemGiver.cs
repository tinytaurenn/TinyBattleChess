using UnityEditor;
using UnityEngine;

public class RandomItemGiver : Usable
{
    [SerializeField] SO_Items m_Items;

    public override void TryUse()
    {
        //base.TryUse();
        Debug.Log("using random item giver");
        SO_Item item = m_Items.Items.RandomInList();

        ConnectionsHandler.Instance.LocalTinyPlayer.m_PlayerLoadout.CreateAndEquipgrabbable(item);
    }
}
