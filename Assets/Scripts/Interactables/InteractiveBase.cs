using UnityEngine;

public class InteractiveBase : UniqueId, IInteractable
{
    public virtual void Use(PlayerController player)
    {

    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.SetInteractable(gameObject);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.RemoveInteractable(gameObject);
        }
    }
}
