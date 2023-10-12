using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    private Transform[] spawnPoints;

    private Collider col;

    //private GameObject[] enemies;
    private List<GameObject> enemies = new List<GameObject>();

    // TODO: add list of object to activate after all the enemies defeated
    [SerializeField]
    private GameObject[] objects;
    // check if they dead?

    private bool isTriggered = false;



    private void Start()
    {
        col = GetComponent<Collider>();

        spawnPoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints[i] = transform.GetChild(i);
        }

    }

    private void Update()
    {
        if (!isTriggered) { return; }

        foreach(var enemy in enemies)
        {
            if (!enemy)
            {
                enemies.Remove(enemy);
            }
        }

        if (enemies.Count == 0)
        {
            if (objects.Length> 0)
            {
                foreach(var obj in objects)
                {
                    obj.SetActive(true);
                }
            }
            Destroy(gameObject, 1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            //for (int i = 0; i < spawnPoints.Length; i++)
            //{
            //    GameObject enemy = Instantiate(enemyPrefab, spawnPoints[i].position, Quaternion.identity);
            //    enemy.GetComponent<DissolvingController>().Appear();
            //    enemies[i] = enemy;
            //  }

            isTriggered = true;

            foreach(var tr in spawnPoints)
            {
                GameObject enemy = Instantiate(enemyPrefab, tr.position, Quaternion.identity);
                enemy.GetComponent<DissolvingController>().Appear();
                enemies.Add(enemy);
            }

            col.enabled = false;
        }
    }



}
