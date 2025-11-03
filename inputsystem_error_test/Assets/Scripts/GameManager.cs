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

    [Header("Scene Settings")]
    [SerializeField] private string nextSceneName; // Set in Inspector

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

    // Called when a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Dynamically find the EnemySpawner in the new scene
        spawner = FindObjectOfType<EnemySpawner>();

        // Optionally, clear the activeEnemies list for a fresh scene
        activeEnemies.Clear();

        // Optionally, hide the win screen in the new scene
        if (winCanvas != null)
            winCanvas.SetActive(false);
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

        int totalLeft = spawner != null ? spawner.TotalEnemiesLeftToSpawn() : 0;

        Debug.Log($"Enemies remaining: {activeEnemies.Count + totalLeft}");

        if (activeEnemies.Count == 0 && totalLeft == 0)
        {
            Debug.Log("You Win!");
            ShowWinScreen();
        }
    }

    private void ShowWinScreen()
    {
        if (winCanvas != null)
        {
            winCanvas.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Win Canvas is not assigned in the GameManager!");
        }
        // Time.timeScale = 0; // Optional pause
    }

    public void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            // Time.timeScale = 1; // Reset if paused
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("No scene name assigned in GameManager!");
        }
    }
}
