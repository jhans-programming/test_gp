using UnityEngine;

public class AutoShoot : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject projectilePrefab;
    public AudioClip laserSound;
    public Transform shootPoint;
    public float shootSpeed = 10f;
    public float fireRate = 10f;
    [HideInInspector] public float normalFireRate;   // GameManager uses this

    private bool enemyInSight = false;
    private float nextShootTime = 0f;

    [SerializeField] private Animator anim;

    // Bullet effects
    public Bullet.BulletEffect currentEffect = Bullet.BulletEffect.None;

    [Header("Multi-Shot")]
    public bool multiShotEnabled = false;
    public int multiShotCount = 3;
    public float multiShotSpread = 10f;

    [Header("Long Range")]
    public bool longRangeEnabled = false;
    public float longRangeSpeedMultiplier = 2f;
    public BoxCollider shootCollider;
    public AutoFace autoFace;
    public float longRangeDetection = 16f;
    public float normalDetection = 10f;

    [Header("Fast Fire")]
    public bool fastFireEnabled = false;
    public float fastFireMultiplier = 3f;

    private void Start()
    {
        normalFireRate = fireRate;
    }

    private void Update()
    {
        anim.SetBool("Attacking", enemyInSight);

        // Auto shoot when enemy is detected
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

        // ✅ Multi-shot mode
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

                bullet.shooterTag = Bullet.ShooterTag.Player;

                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.velocity = rotation * Vector3.forward * finalSpeed;
            }

            return;
        }
        AudioManager.Instance.PlaySFX(laserSound);
        // ✅ Single shot
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
