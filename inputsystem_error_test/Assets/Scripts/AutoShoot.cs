using UnityEngine;

public class AutoShoot : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject projectilePrefab;   // The projectile to spawn
    public Transform shootPoint;          // Point where projectile spawns
    public float shootSpeed = 10f;        // Speed of the projectile
    public float fireRate = 10f;          // Shots per second

    private bool enemyInSight = false;
    private float nextShootTime = 0f;

    private void Update()
    {

        if (enemyInSight)
        { 

            // Shoot continuously based on fireRate
            if (Time.time >= nextShootTime)
            {
                Shoot();
                nextShootTime = Time.time + (1f / fireRate);
            }
        }
    }

    private void Shoot()
    {
        if (projectilePrefab != null && shootPoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

            // Optional: give it velocity if it has a Rigidbody
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = shootPoint.forward * shootSpeed;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyInSight = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyInSight = false;
        }
    }
}
