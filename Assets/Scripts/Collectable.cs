using UnityEngine;

public class Collectable : MonoBehaviour, Interactable
{
    [SerializeField] private CollectableType type;
    [SerializeField] private int ammount;
    [SerializeField] private string UUID;

    public void Use(PlayerController player)
    {
        InventoryItem item = new InventoryItem();
        item.type = type;
        item.ammount = ammount;
        if (player.AddItemInInventory(item)) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.currentInteractable = gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player.currentInteractable == gameObject) player.currentInteractable = null;
        }
    }
}

public enum CollectableType
{
    None,
    Heal,
    MixerHeal,
    Weapon,
    PistolAmmo,
    ShotgunAmmo
}
