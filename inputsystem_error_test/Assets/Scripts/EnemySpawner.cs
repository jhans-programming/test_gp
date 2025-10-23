using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    class EnemyGroup
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

        public void Spawn()
        {
            if (count > 0 && Time.time > startSpawningTime && Time.time - lastSpawnTime > spawnInterval)
            {
                count--;
                lastSpawnTime = Time.time;
                Instantiate(enemyPrefab, spawnPoint, Quaternion.Euler(0, 0, 0));
            }
        }
    }
    

    [SerializeField]
    private List<EnemyGroup> enemyWave;

    [SerializeField]
    private List<Transform> spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        foreach (EnemyGroup group in enemyWave)
        {
            group.SetSpawnPoint(spawnPoints[group.spawnPointIndex].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (EnemyGroup group in enemyWave)
        {
            group.Spawn();
        }
    }
}
