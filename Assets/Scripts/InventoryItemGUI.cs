using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemGUI", menuName = "Scriptable Objects/InventoryItemGUI")]
[System.Serializable]
public class InventoryItemGUI : ScriptableObject
{
    public Sprite sprite;
    public Mesh mesh;
    public Material material;
}
