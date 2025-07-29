using UnityEngine;

public class BoxDeposit : MonoBehaviour, IInteractable
{
    public void Use(PlayerController player)
    {
        UIManager.instance.OpenBox();
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
