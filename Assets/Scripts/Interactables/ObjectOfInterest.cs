using UnityEngine;

public class ObjectOfInterest : MonoBehaviour
{
    [SerializeField] private BoxCollider parentCollider;
    [SerializeField] private BoxCollider localCollider;

    private void Start()
    {
        localCollider.size = parentCollider.size * 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.SetObjectOfInterest(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.RemoveObjectOfInterest(gameObject);
        }
    }
}
