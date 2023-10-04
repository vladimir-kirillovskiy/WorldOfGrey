using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    [SerializeField]
    private EnemyMelee enemy;

    private PlayerHealth player;


    void Start()
    {
        player = FindAnyObjectByType<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            enemy.DamageDealer.Invoke();
        }
    }
}
