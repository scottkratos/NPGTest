using UnityEngine;

[System.Serializable]
public struct InventoryItem
{
    public string itemName;
    public string itemDesc;
    public CollectableType type;
    public int ammount;
    public Sprite sprite;
    [HideInInspector] public Mesh mesh;
}
