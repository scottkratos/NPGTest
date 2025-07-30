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
    private GameObject currentInteractable;
    private GameObject objectOfInterest;

    [Header("Inventory")]
    public InventoryItem[] inventory;
    public InventoryItem[] maxItemTypeBySlot;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private float maxHeadAngle;
    private Quaternion lastHeadRotation;
    private float headReset = 2f;

    [Header("Combat")]
    [SerializeField] private int health;
    private bool isAiming;
    [SerializeField] private LayerMask layerMask;
    private bool isWeaponEquipped;

    [Header("References")]
    [SerializeField] private GameObject flashLight;
    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject neck;


    public static PlayerController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.SetParent(null);
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
        playerInputActions.FindAction("Aim").performed += SetAim;
        playerInputActions.FindAction("Aim").canceled += SetAim;
        playerInputActions.FindAction("Shoot").performed += SetShoot;
        playerInputActions.FindAction("Shoot").canceled += SetShoot;
        playerInputActions.FindAction("Flashlight").performed += SetFlashlight;
        playerInputActions.FindAction("Flashlight").canceled += SetFlashlight;
    }

    private void Update()
    {
        animator.SetFloat("Speed", rb.linearVelocity.magnitude);
        animator.SetFloat("WalkingMultiplier", Mathf.Min(speed - walkingSpeed, 0.5f) + 1);
        animator.SetInteger("Health", health);
        animator.SetBool("IsAiming", isWeaponEquipped && isAiming);
        //Injury Layer
        animator.SetLayerWeight(1, health <= 50 && !isAiming ? 1 : 0);
        //Weapon Layer
        animator.SetLayerWeight(2, health > 50 && !isAiming && isWeaponEquipped ? 1 : 0);
        if (isAiming) Aim();
    }

    private void LateUpdate()
    {
        if (isAiming) return;
        if (objectOfInterest != null)
        {
            Vector3 direction = (objectOfInterest.transform.position - neck.transform.position).normalized;
            float angle = Vector3.SignedAngle(direction, transform.forward, transform.up);
            if (angle <= maxHeadAngle && angle >= -maxHeadAngle)
            {
                neck.transform.LookAt(objectOfInterest.transform);
                lastHeadRotation = neck.transform.rotation;
                headReset = 2f;
            }
            else
            {
                if (headReset > 0)
                {
                    lastHeadRotation = Quaternion.Slerp(lastHeadRotation, neck.transform.rotation, headReset * Time.deltaTime);
                    neck.transform.rotation = lastHeadRotation;
                    headReset -= Time.deltaTime;
                }
            }
        }
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
        if (value.ReadValueAsButton()) GameManager.instance.OpenInventory();
    }

    public void SetAim(InputAction.CallbackContext value)
    {
        isAiming = value.ReadValueAsButton();
    }

    public void SetShoot(InputAction.CallbackContext value)
    {
        if (isAiming && value.ReadValueAsButton()) GameManager.instance.OpenInventory();
    }

    public void SetFlashlight(InputAction.CallbackContext value)
    {
        if (value.ReadValueAsButton()) flashLight.SetActive(!flashLight.activeSelf);
    }

    private void Aim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            Vector3 targetPosition = hit.point;
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.Euler(0, (Mathf.Atan2(-direction.z, direction.x) * Mathf.Rad2Deg) + 90, 0);
        }
    }

    public void EquipWeapon()
    {
        isWeaponEquipped = !isWeaponEquipped;
        weapon.SetActive(isWeaponEquipped);
    }

    public void ForceUnequip()
    {
        isWeaponEquipped = false;
        weapon.SetActive(isWeaponEquipped);
    }

    private void Shoot()
    {
        if (!isWeaponEquipped) return;
    }

    public void SetObjectOfInterest(GameObject ooi)
    {
        objectOfInterest = ooi;
    }

    public void RemoveObjectOfInterest(GameObject ooi)
    {
        if (objectOfInterest == ooi) objectOfInterest = null;
    }

    public void SetInteractable(GameObject interactable)
    {
        currentInteractable = interactable;
    }

    public void RemoveInteractable(GameObject interactable)
    {
        if (currentInteractable == interactable) currentInteractable = null;
    }

    private void FixedUpdate()
    {
        if (isAiming) return;
        if (health <= 0) return;
        rb.linearVelocity = new Vector3(movement.x * speed, rb.linearVelocity.y, movement.y * speed);
        if (movement.magnitude > 0.1f)
        {
            if (transform.localRotation != Quaternion.Euler(0, Mathf.Atan2(-movement.y, movement.x) * Mathf.Rad2Deg + 90, 0))
            {
                desiredRotation = Quaternion.Euler(0, Mathf.Atan2(-movement.y, movement.x) * Mathf.Rad2Deg + 90, 0);
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
