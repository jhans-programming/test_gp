using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    [Header("Tag to count")]
    [SerializeField] private string enemyTag = "Enemy";

    void Update()
    {
        // Count all enemies with the given tag
        int enemyCount = GameObject.FindGameObjectsWithTag(enemyTag).Length;

        // Debug log the count
        Debug.Log("Number of enemies in the scene: " + enemyCount);
    }
}
