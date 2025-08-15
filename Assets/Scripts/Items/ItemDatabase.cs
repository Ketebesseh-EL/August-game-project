using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Item Database")]

public class ItemDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemData[] Items;
    public Dictionary<ItemData, int> GetId = new Dictionary<ItemData, int>();
    public Dictionary<int, ItemData> GetItem = new Dictionary<int, ItemData>();

    public void OnAfterDeserialize()
    {
        GetId = new Dictionary<ItemData, int>();
        GetItem = new Dictionary<int, ItemData>();

        for (int i = 0; i < Items.Length; i++)
        {
            GetId.Add(Items[i], i);
            GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
    }
}
