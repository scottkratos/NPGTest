using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private InputActionAsset playerInputActions;

    [Header("Movement")]
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float sprintingSpeed;
    private float speed;

    private Vector2 movement;
    private Rigidbody rb;
    private Quaternion desiredRotation;
    private float lerpTimer = .2f;
    private Coroutine lerpCoroutine;

    //Interactable
    [HideInInspector] public GameObject currentInteractable;

    [Header("Inventory")]
    public InventoryItem[] inventory;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        speed = walkingSpeed;
        playerInputActions.FindAction("Move").performed += SetMovement;
        playerInputActions.FindAction("Move").canceled += SetMovement;
        playerInputActions.FindAction("Interact").performed += SetInteraction;
        playerInputActions.FindAction("Interact").canceled += SetInteraction;
        playerInputActions.FindAction("Sprint").performed += SetSprint;
        playerInputActions.FindAction("Sprint").canceled += SetSprint;
    }

    public void SetMovement(InputAction.CallbackContext value)
    {
        movement = value.ReadValue<Vector2>();
    }

    public void SetInteraction(InputAction.CallbackContext value)
    {
        if (value.ReadValueAsButton())
        {
            if (currentInteractable != null) currentInteractable.GetComponent<Interactable>().Use(this);
        }
    }

    public void SetSprint(InputAction.CallbackContext value)
    {
        if (value.ReadValueAsButton()) speed = sprintingSpeed;
        else speed = walkingSpeed;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(movement.x * speed, rb.linearVelocity.y, movement.y * speed);
        if (movement.magnitude > 0.1f)
        {
            if (desiredRotation != Quaternion.Euler(0, Mathf.Atan2(-movement.y, movement.x) * Mathf.Rad2Deg - 90, 0))
            {
                desiredRotation = Quaternion.Euler(0, Mathf.Atan2(-movement.y, movement.x) * Mathf.Rad2Deg - 90, 0);
                if (lerpCoroutine != null) StopCoroutine(lerpCoroutine);
                StartCoroutine(LerpRotation());
            }
        }
        else if (lerpCoroutine != null) StopCoroutine(lerpCoroutine);
    }

    private IEnumerator LerpRotation()
    {
        float localTimer = 0;
        Quaternion initialRot = transform.localRotation;

        while (localTimer < lerpTimer)
        {
            transform.localRotation = Quaternion.Lerp(initialRot, desiredRotation, localTimer / lerpTimer);
            localTimer += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = desiredRotation;
    }

    public void AddItemInInventory(InventoryItem item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].type == CollectableType.None)
            {
                inventory[i] = item;
                break;
            }
        }
    }

    public void RemoveItemFromInventory()
    {

    }

    public void SwapItemInInventory()
    {

    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }
}
