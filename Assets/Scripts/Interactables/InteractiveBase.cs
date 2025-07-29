using UnityEngine;

public class InteractiveBase : MonoBehaviour, IInteractable
{
    public virtual void Use(PlayerController player)
    {

    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.currentInteractable = gameObject;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player.currentInteractable == gameObject) player.currentInteractable = null;
        }
    }
}
