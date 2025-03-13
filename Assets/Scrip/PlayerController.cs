using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform orientation; // Reference used to orient the movement

    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction moveAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Get the PlayerInput component and the "Move" action from the Input Action Asset
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
    }

    private void OnEnable()
    {
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        // Read input from the Move action (x for sideways, y for forward/backward)
        Vector2 input = moveAction.ReadValue<Vector2>();

        // Calculate the move direction relative to the orientation transform.
        // For example, using the forward and right vectors from the orientation.
        Vector3 moveDirection = orientation.forward * input.y + orientation.right * input.x;
        moveDirection.y = 0f; // Optionally constrain movement to the XZ plane

        // Normalize to avoid faster diagonal movement then apply speed and deltaTime.
        moveDirection = moveDirection.normalized;

        // Apply the movement by modifying the transform's position.
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}