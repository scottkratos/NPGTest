using UnityEngine;

public class Collectable : InteractiveBase
{
    [SerializeField] private InventoryItem item;

    public override void Use(PlayerController player)
    {
        base.Use(player);
        if (player.AddItemInInventory(item)) Destroy(gameObject);
    }
}

public enum CollectableType
{
    None,
    Heal,
    Weapon,
    PistolAmmo,
    KeyItem
}
