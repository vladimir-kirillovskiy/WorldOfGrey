using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float health = 100f;

    public void Damage(float damage)
    {
        Debug.Log("Damage");
        health -= damage;
        if (health <= 0)
            // Destroy(gameObject);
            Debug.Log("You're dead!");
    }
}
