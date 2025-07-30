using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public InventoryItem[] inventoryItems;
    public InventoryItem[] boxItems;
    public List<string> usedUUIDs = new List<string>();
    public string lastLevel;
    public float lastPositionX;
    public float lastPositionY;
    public float lastPositionZ;
    public bool isWeaponEquipped;
    public int playerHealth;

    public PlayerData()
    {
        inventoryItems = PlayerController.instance.inventory;
        boxItems = GameManager.instance.boxItems;
        usedUUIDs = GameManager.instance.usedUUIDs;
        lastLevel = GameManager.instance.lastLevel;
        lastPositionX = PlayerController.instance.gameObject.transform.position.x;
        lastPositionY = PlayerController.instance.gameObject.transform.position.y;
        lastPositionZ = PlayerController.instance.gameObject.transform.position.z;
        isWeaponEquipped = PlayerController.instance.isWeaponEquipped;
        playerHealth = PlayerController.instance.health;
    }
}
