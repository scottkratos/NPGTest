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
    public InventoryItem[] maxItemTypeBySlot;

    [Header("Combat")]
    [SerializeField] private int health;

    public static PlayerController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        rb = GetComponent<Rigidbody>();
        speed = walkingSpeed;
        playerInputActions.FindAction("Move").performed += SetMovement;
        playerInputActions.FindAction("Move").canceled += SetMovement;
        playerInputActions.FindAction("Interact").performed += SetInteraction;
        playerInputActions.FindAction("Interact").canceled += SetInteraction;
        playerInputActions.FindAction("Sprint").performed += SetSprint;
        playerInputActions.FindAction("Sprint").canceled += SetSprint;
        playerInputActions.FindAction("Inventory").performed += SetInventory;
        playerInputActions.FindAction("Inventory").canceled += SetInventory;
    }

    public void SetMovement(InputAction.CallbackContext value)
    {
        movement = value.ReadValue<Vector2>();
    }

    public void SetInteraction(InputAction.CallbackContext value)
    {
        if (value.ReadValueAsButton())
        {
            if (currentInteractable != null) currentInteractable.GetComponent<IInteractable>().Use(this);
        }
    }

    public void SetSprint(InputAction.CallbackContext value)
    {
        if (value.ReadValueAsButton()) speed = sprintingSpeed;
        else speed = walkingSpeed;
    }

    public void SetInventory(InputAction.CallbackContext value)
    {
        if (value.ReadValueAsButton()) GameManager.instance.PauseGame();
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

    public bool AddItemInInventory(InventoryItem item)
    {
        InventoryItem localItem = new InventoryItem();
        localItem.type = item.type;
        localItem.ammount = item.ammount;
        InventoryItem itemRef = System.Array.Find(maxItemTypeBySlot, i => i.type == item.type);

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].type == CollectableType.None)
            {
                inventory[i] = item;
                return true;
            }
            else if (inventory[i].type == item.type)
            {
                if (inventory[i].ammount + localItem.ammount <= itemRef.ammount)
                {
                    inventory[i].ammount += localItem.ammount;
                    return true;
                }
            }
        }
        return false;
    }

    public void Heal()
    {
        health = 100;
    }

    public void Damage(int ammount)
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
