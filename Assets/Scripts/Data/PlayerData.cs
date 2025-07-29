using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public InventoryItem[] inventoryItems;
    public InventoryItem[] boxItems;
    public List<string> usedUUIDs = new List<string>();
    public string lastLevel;
    public Vector3 lastPosition;

    public PlayerData()
    {
        inventoryItems = PlayerController.instance.inventory;
        boxItems = GameManager.instance.boxItems;
        usedUUIDs = GameManager.instance.usedUUIDs;
        lastLevel = GameManager.instance.lastLevel;
        lastPosition = GameManager.instance.lastPosition;
    }
}
