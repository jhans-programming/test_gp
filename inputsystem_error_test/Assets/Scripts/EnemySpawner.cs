using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyGroup
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private int count;
        [SerializeField] private float spawnInterval;
        [SerializeField] private float startSpawningTime;
        [SerializeField] public int spawnPointIndex;

        private float lastSpawnTime;
        private Vector3 spawnPoint;

        public void SetSpawnPoint(Vector3 sp)
        {
            spawnPoint = sp;
        }

        public bool Spawn()
        {
            // Only spawn if there are enemies left
            if (count > 0 && Time.time > startSpawningTime && Time.time - lastSpawnTime > spawnInterval)
            {
                count--;
                lastSpawnTime = Time.time;
                GameObject newEnemy = Object.Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);

                // Register with GameManager
                Enemy enemyComponent = newEnemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    GameManager.Instance.RegisterEnemy(enemyComponent);
                }

                return true; // spawned an enemy
            }

            return false; // no spawn
        }

        public int RemainingToSpawn => count; // number of enemies left to spawn
    }

    [SerializeField] private List<EnemyGroup> enemyWave;
    [SerializeField] private List<Transform> spawnPoints;

    private void Start()
    {
        foreach (EnemyGroup group in enemyWave)
        {
            group.SetSpawnPoint(spawnPoints[group.spawnPointIndex].position);
        }
    }

    private void Update()
    {
        foreach (EnemyGroup group in enemyWave)
        {
            group.Spawn();
        }
    }

    // Return total enemies left to spawn across all groups
    public int TotalEnemiesLeftToSpawn()
    {
        int total = 0;
        foreach (var group in enemyWave)
        {
            total += group.RemainingToSpawn;
        }
        return total;
    }
}
