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
    [SerializeField] private GameObject loseCanvas; // ✅ NEW
    [SerializeField] private TimerUI timerUI; // ✅ Reference to timer script


    [Header("Scene Settings")]
    [SerializeField] private string nextSceneName;

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

        timerUI = FindObjectOfType<TimerUI>(); // ✅ Auto assign

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

        int totalLeftToSpawn = spawner != null ? spawner.TotalEnemiesLeftToSpawn() : 0;
        int currentlyAlive = activeEnemies.Count;

        // ✅ Debug: show how many enemies are left in the level
        Debug.Log($"Enemy died! Alive right now: {currentlyAlive}, left to spawn: {totalLeftToSpawn}");

        if (currentlyAlive == 0 && totalLeftToSpawn == 0)
        {
            Debug.Log("✅ All enemies defeated! Player wins!");
            ShowWinScreen();
        }
    }

    private void ShowWinScreen()
    {
        if (timerUI != null)
            timerUI.StopTimer(); // ✅ Stop timer

        if (winCanvas != null)
            winCanvas.SetActive(true);
    }

    public void ShowLoseScreen()
    {
        if (timerUI != null)
            timerUI.StopTimer(); // ✅ Stop timer

        if (loseCanvas != null)
            loseCanvas.SetActive(true);

        Time.timeScale = 0;
    }

    // ✅ Button function to restart scene
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
