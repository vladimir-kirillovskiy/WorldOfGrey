using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab; // Assign the enemy prefab in the Inspector
    [SerializeField]
    private Transform[] spawnLocations; // Assign the spawn points as child Transforms in the Inspector
    [SerializeField]
    private float minSpawnInterval = 2f; // Minimum time between spawns
    [SerializeField]
    private float maxSpawnInterval = 5f; // Maximum time between spawns

    void Start()
    {
        // Start spawning enemies
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        // Pick a random spawn location
        Transform randomSpawnPoint = spawnLocations[Random.Range(0, spawnLocations.Length)];

        // Instantiate and position the enemy
        GameObject newEnemy = Instantiate(enemyPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);

        // Optional: Set up any additional properties or scripts on the enemy

        // Set a new random interval for the next spawn
        float randomInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        Invoke("SpawnEnemy", randomInterval);
    }
}
