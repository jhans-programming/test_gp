using UnityEngine;

public class AutoShoot : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float shootSpeed = 10f;
    public float fireRate = 10f;          // Normal fire rate
    private float normalFireRate;         // Store original fire rate

    private bool enemyInSight = false;
    private float nextShootTime = 0f;

    [SerializeField] private Animator anim;

    // Debug bullet effects
    public Bullet.BulletEffect currentEffect = Bullet.BulletEffect.None;

    [Header("Multishot Settings (Debug)")]
    public bool multiShotEnabled = false;
    public int multiShotCount = 3;
    public float multiShotSpread = 10f;

    [Header("Long Range Modifier (Debug)")]
    public bool longRangeEnabled = false;
    public float longRangeSpeedMultiplier = 2f;
    public BoxCollider shootCollider;
    public AutoFace autoFace;
    public float longRangeDetection = 16f;
    public float normalDetection = 10f;

    [Header("Fast Fire Modifier (Debug)")]
    public bool fastFireEnabled = false;
    public float fastFireMultiplier = 3f; // Shoots 3x faster

    private void Start()
    {
        normalFireRate = fireRate;
    }

    private void Update()
    {
        anim.SetBool("Attacking", enemyInSight);

        // ------- DEBUG INPUTS -------

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentEffect = Bullet.BulletEffect.Burn;
            Debug.Log("DEBUG: Bullet set to BURN");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            multiShotEnabled = !multiShotEnabled;
            Debug.Log("DEBUG: Multi Shot: " + (multiShotEnabled ? "ON" : "OFF"));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentEffect = Bullet.BulletEffect.Slow;
            Debug.Log("DEBUG: Bullet set to SLOW");
        }

        // ✅ KEY 4 — LONG RANGE MODE
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            longRangeEnabled = !longRangeEnabled;

            // Change collider size
            if (shootCollider != null)
            {
                Vector3 size = shootCollider.size;
                size.z = longRangeEnabled ? 16f : 10f;
                shootCollider.size = size;
            }

            // Change detection radius
            if (autoFace != null)
                autoFace.detectionRadius = longRangeEnabled ? longRangeDetection : normalDetection;

            Debug.Log("DEBUG: Long Range: " + (longRangeEnabled ? "ON" : "OFF"));
        }

        // ✅ KEY 5 — FAST FIRE MODE
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            fastFireEnabled = !fastFireEnabled;

            fireRate = fastFireEnabled ? normalFireRate * fastFireMultiplier : normalFireRate;

            Debug.Log("DEBUG: Fast Fire: " + (fastFireEnabled ? "ON" : "OFF") +
                      " | FireRate=" + fireRate);
        }

        // ------- AUTO SHOOT -------
        if (enemyInSight && Time.time >= nextShootTime)
        {
            Shoot();
            nextShootTime = Time.time + (1f / fireRate);
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null || shootPoint == null)
            return;

        float finalSpeed = longRangeEnabled ? shootSpeed * longRangeSpeedMultiplier : shootSpeed;

        // Multi-shot mode
        if (multiShotEnabled && multiShotCount > 1)
        {
            int half = multiShotCount / 2;

            for (int i = 0; i < multiShotCount; i++)
            {
                float angle = (i - half) * multiShotSpread;
                Quaternion rotation = shootPoint.rotation * Quaternion.Euler(0, angle, 0);

                GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, rotation);

                Bullet bullet = projectile.GetComponent<Bullet>();
                if (bullet != null)
                    bullet.SetEffect(currentEffect);

                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.velocity = rotation * Vector3.forward * finalSpeed;
            }

            return;
        }

        // Single shot
        GameObject p = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

        Bullet b = p.GetComponent<Bullet>();
        if (b != null)
            b.SetEffect(currentEffect);

        Rigidbody r = p.GetComponent<Rigidbody>();
        if (r != null)
            r.velocity = shootPoint.forward * finalSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
            enemyInSight = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
            enemyInSight = false;
    }
}
