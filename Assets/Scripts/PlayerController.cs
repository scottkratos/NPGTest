using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] private InputActionAsset playerInputActions;

    [Header("Movement")]
    [SerializeField] private float speed;
    private Vector2 movement;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInputActions.FindAction("Move").performed += SetMovement;
        playerInputActions.FindAction("Move").canceled += SetMovement;
        playerInputActions.FindAction("Interact").performed += SetInteraction;
    }

    public void SetInteraction(InputAction.CallbackContext value)
    {
    }

    public void SetMovement(InputAction.CallbackContext value)
    {
        movement = value.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(movement.x * speed, rb.linearVelocity.y, movement.y * speed);
        print(rb.linearVelocity);
        //transform.localRotation = Quaternion.Euler(new Vector3(1, 1, 1));
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
