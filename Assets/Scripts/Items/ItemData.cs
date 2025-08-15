using UnityEngine;


public enum ItemType
{
    Weapon,
    Upgrade,
    Default
}


[CreateAssetMenu(fileName = "new ItemData", menuName = "Inventory System/ItemData")]
public class ItemData : ScriptableObject
{
    public string ItemName;
    public int Id;
    [TextArea(5, 20)] public string description;
    public ItemType type;
    public Sprite Icon;
    
    public void Awake()
    {

        type = ItemType.Default;
    }
}
