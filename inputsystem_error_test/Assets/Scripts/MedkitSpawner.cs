using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedkitSpawner : MonoBehaviour
{
    [System.Serializable]
    public class MedkitGroup
    {
        [SerializeField] private GameObject medkitPrefab;
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
            if (count > 0 && Time.time > startSpawningTime && Time.time - lastSpawnTime > spawnInterval)
            {
                count--;
                lastSpawnTime = Time.time;
                GameObject newMedkit = Object.Instantiate(medkitPrefab, spawnPoint, Quaternion.identity);

                // Optional: set some properties on the medkit if needed
                // e.g., newMedkit.GetComponent<Medkit>().Init();

                return true; // spawned a medkit
            }

            return false; // nothing spawned
        }

        public int RemainingToSpawn => count;
    }

    [SerializeField] private List<MedkitGroup> medkitWaves;
    [SerializeField] private List<Transform> spawnPoints;

    private void Start()
    {
        foreach (MedkitGroup group in medkitWaves)
        {
            group.SetSpawnPoint(spawnPoints[group.spawnPointIndex].position);
        }
    }

    private void Update()
    {
        foreach (MedkitGroup group in medkitWaves)
        {
            group.Spawn();
        }
    }

    public int TotalMedkitsLeftToSpawn()
    {
        int total = 0;
        foreach (var group in medkitWaves)
        {
            total += group.RemainingToSpawn;
        }
        return total;
    }
}
