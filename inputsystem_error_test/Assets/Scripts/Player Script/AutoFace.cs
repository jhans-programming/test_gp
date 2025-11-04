using UnityEngine;

public class AutoFace : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRadius = 10f;   // How far to look for enemies
    public string enemyTag = "Enemy";     // Tag of enemy objects
    public float rotateSpeed = 5f;        // How quickly to face the enemy

    [Header("References")]
    public AutoShoot autoShoot;           // Reference to AutoShoot script
    public PlayerMovement playerMovement; // Reference to player movement script

    private Transform currentTarget;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("Attacking", playerMovement != null && !playerMovement.IsMoving() && currentTarget != null);
        Debug.Log("Current target is null : " + (currentTarget == null));

        // Only look for enemies when the player is NOT moving
        if (playerMovement != null && playerMovement.IsMoving())
        {
            if (currentTarget != null)
            {
                Debug.Log($"Stopped facing {currentTarget.name} because player started moving.");
                currentTarget = null;
            }

            if (autoShoot != null)
                autoShoot.enabled = false; // Stop shooting while moving
            return;
        }

        // Find nearest enemy
        Transform previousTarget = currentTarget;
        FindNearestEnemy();

        // DEBUG LOGG, to track taget changes
        if (currentTarget != previousTarget)
        {
            if (currentTarget != null)
                Debug.Log($"Now facing enemy: {currentTarget.name}");
            else if (previousTarget != null)
                Debug.Log($"Lost target: {previousTarget.name}");
        }

        if (currentTarget != null)
        {
            // Rotate toward enemy
            Vector3 direction = currentTarget.position - transform.position;
            direction.y = 0f; // Keep rotation flat on ground
            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            }

            // Start auto shooting
            if (autoShoot != null)
                autoShoot.enabled = true;
        }
        else
        {
            // No target in range → stop auto shooting
            if (autoShoot != null)
                autoShoot.enabled = false;
        }
    }

    private void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float closestDist = Mathf.Infinity;
        Transform nearest = null;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < detectionRadius && dist < closestDist)
            {
                closestDist = dist;
                nearest = enemy.transform;
            }
        }

        currentTarget = nearest;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
