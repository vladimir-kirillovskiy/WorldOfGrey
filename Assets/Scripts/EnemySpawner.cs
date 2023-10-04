using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private Vector3 spawnPosition;


    // TODO: pass number of spawn points and spawn an enemy for each of them
    private Transform[] spawnPoints;

    private void Start()
    {
        spawnPoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints[i] = transform.GetChild(i);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach(var tr in spawnPoints)
            {
                GameObject enemy = Instantiate(enemyPrefab, tr.position, Quaternion.identity);
                enemy.GetComponent<DissolvingController>().Appear();
            }

            Destroy(gameObject);
        }
    }



}
