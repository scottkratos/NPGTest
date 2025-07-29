using UnityEngine;

public class Collectable : MonoBehaviour, Interactable
{
    [SerializeField] private InventoryItem item;
    [SerializeField] private string UUID;

    private void Start()
    {
        item.mesh = GetComponent<MeshFilter>().mesh;
    }

    public void Use(PlayerController player)
    {
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
    Weapon,
    PistolAmmo,
    KeyItem
}
