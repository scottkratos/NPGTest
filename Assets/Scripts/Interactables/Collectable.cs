using UnityEngine;

public class Collectable : InteractiveBase
{
    [SerializeField] private InventoryItem item;
    [SerializeField] private string UUID;

    private void Start()
    {
        item.mesh = GetComponent<MeshFilter>().mesh;
    }

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
