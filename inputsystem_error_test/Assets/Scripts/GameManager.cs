using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private List<Enemy> activeEnemies = new List<Enemy>();
    private EnemySpawner spawner;

    [Header("UI")]
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private TimerUI timerUI;

    [Header("Scene Settings")]
    [SerializeField] private string nextSceneName;

    [Header("Ability System")]
    public AutoShoot playerShoot;     // Reference to player shooting script
    private int killCount = 0;

    public int unlockMultiShot = 10;
    public int unlockLongRange = 20;
    public int unlockFastFire = 30;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        spawner = FindObjectOfType<EnemySpawner>();
        activeEnemies.Clear();

        timerUI = FindObjectOfType<TimerUI>(); 
        playerShoot = FindObjectOfType<AutoShoot>(); // ✅ Auto assign shooting script

        killCount = 0;

        if (winCanvas != null) winCanvas.SetActive(false);
        if (loseCanvas != null) loseCanvas.SetActive(false);
    }

    public void RegisterEnemy(Enemy enemy)
    {
        if (!activeEnemies.Contains(enemy))
        {
            activeEnemies.Add(enemy);
            enemy.OnDeath += OnEnemyDeath;
        }
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        activeEnemies.Remove(enemy);
        killCount++;

        Debug.Log($"Enemy killed! Total kills: {killCount}");

        UnlockAbilities(); // ✅ check abilities

        int totalLeftToSpawn = spawner != null ? spawner.TotalEnemiesLeftToSpawn() : 0;
        int currentlyAlive = activeEnemies.Count;

        if (currentlyAlive == 0 && totalLeftToSpawn == 0)
        {
            ShowWinScreen();
        }
    }

    private void UnlockAbilities()
    {
        if (playerShoot == null) return;

        // ✅ Unlock Multi-Shot
        if (killCount == unlockMultiShot && !playerShoot.multiShotEnabled)
        {
            playerShoot.multiShotEnabled = true;
            Debug.Log("🚀 Ability Unlocked: MULTI-SHOT!");
        }

        // ✅ Unlock Long Range
        if (killCount == unlockLongRange && !playerShoot.longRangeEnabled)
        {
            playerShoot.longRangeEnabled = true;

            // Update collider size
            if (playerShoot.shootCollider != null)
            {
                Vector3 size = playerShoot.shootCollider.size;
                size.z = playerShoot.longRangeDetection;
                playerShoot.shootCollider.size = size;
            }

            if (playerShoot.autoFace != null)
                playerShoot.autoFace.detectionRadius = playerShoot.longRangeDetection;

            Debug.Log("🎯 Ability Unlocked: LONG RANGE!");
        }

        // ✅ Unlock Fast Fire
        if (killCount == unlockFastFire && !playerShoot.fastFireEnabled)
        {
            playerShoot.fastFireEnabled = true;
            playerShoot.fireRate = playerShoot.normalFireRate * playerShoot.fastFireMultiplier;

            Debug.Log("🔥 Ability Unlocked: FAST FIRE!");
        }
    }

    private void ShowWinScreen()
    {
        if (timerUI != null)
            timerUI.StopTimer();

        if (winCanvas != null)
            winCanvas.SetActive(true);
    }

    public void ShowLoseScreen()
    {
        if (timerUI != null)
            timerUI.StopTimer();

        if (loseCanvas != null)
            loseCanvas.SetActive(true);

        Time.timeScale = 0;
    }

    public void RestartScene()
    {
        Time.timeScale = 1;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    public void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
