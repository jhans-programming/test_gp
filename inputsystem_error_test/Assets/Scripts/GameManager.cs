using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private List<Enemy> activeEnemies = new List<Enemy>();
    [SerializeField] private EnemySpawner spawner;

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
        }
    }

    // Register a spawned enemy
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

        Debug.Log($"Enemies remaining: {activeEnemies.Count + spawner.TotalEnemiesLeftToSpawn()}");

        // Win condition: no more enemies alive AND none left to spawn
        if (activeEnemies.Count == 0 && spawner.TotalEnemiesLeftToSpawn() == 0)
        {
            Debug.Log("You Win!");
        }
    }
}
