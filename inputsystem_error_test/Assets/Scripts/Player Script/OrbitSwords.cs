using UnityEngine;

public class OrbitSwords : MonoBehaviour
{
    [Tooltip("Degrees per second the sword cluster rotates around world Y (independent of player rotation).")]
    public float rotationSpeed = 180f;

    [Tooltip("The player transform to follow (position only).")]
    public Transform player;

    [Tooltip("If true, the orbit GameObject will be unparented from the player at Start so it won't inherit player's rotation.")]
    public bool detachFromPlayer = true;

    [Tooltip("Vertical offset from the player's position for the orbit center.")]
    public float yOffset = 0.0f;

    private void Start()
    {
        // If this object is parented to the player, detach so it won't inherit player's rotation
        if (detachFromPlayer && transform.parent != null)
        {
            // If parent is the player, unparent; otherwise still unparent to be safe
            transform.SetParent(null, true);
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        // 1) Keep orbit centered on player position (only position, no rotation)
        Vector3 targetPos = player.position;
        targetPos.y += yOffset;
        transform.position = targetPos;

        // 2) Rotate around world up axis so rotation is independent of player's facing
        // Using Space.World ensures we're rotating in world coordinates, not local.
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);

        // 3) Ensure the orbit GameObject's rotation only has yaw (no pitch/roll)
        // This prevents accidental tilt if some other script or physics touches it.
        Vector3 e = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, e.y, 0f);
    }
}
