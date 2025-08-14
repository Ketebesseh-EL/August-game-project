using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>();

    public void AddItem(ItemData _item)
    {
        Container.Add(new InventorySlot(_item));
    }
}


[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public InventorySlot(ItemData _item)
    {
        item = _item;
    }


}