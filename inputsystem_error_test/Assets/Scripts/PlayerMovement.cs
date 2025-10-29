using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f; // Higher = faster rotation

    [HideInInspector] 
    public bool isDashing = false; 

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isMoving = false; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        if (isDashing)
            return;

        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);

        // Check if player is moving
        isMoving = moveDirection.magnitude > 0.01f;

        if (isMoving)
        {
            // Smoothly rotate toward movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            // Move player
            Vector3 velocity = moveDirection.normalized * moveSpeed;
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        }
        else
        {
            // Stop horizontal movement when no input
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }
    }

    // Called by Dash Script
    public bool IsMoving()
    {
        return isMoving;
    }
}
