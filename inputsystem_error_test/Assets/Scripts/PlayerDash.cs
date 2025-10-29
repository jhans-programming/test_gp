using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerDash : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public PlayerMovement playerMovement; // Link to movement script

    [Header("Dash Settings")]
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private bool canDash = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private System.Collections.IEnumerator Dash()
    {
        canDash = false;
        playerMovement.isDashing = true; // disable movement

        // Dash direction = facing direction
        Vector3 dashDirection = transform.forward.normalized;

        rb.useGravity = false;
        rb.velocity = dashDirection * dashForce;

        yield return new WaitForSeconds(dashDuration);

        // Stop dash
        rb.velocity = Vector3.zero;
        rb.useGravity = true;

        playerMovement.isDashing = false; // re-enable movement

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
