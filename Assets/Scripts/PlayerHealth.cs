using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float health = 100f;

    private UI_Health uiHealth;

    private void Start()
    {
        uiHealth = FindObjectOfType<UI_Health>();
    }

    public void Damage(float damage)
    {
        Debug.Log("Damage");
        health -= damage;

        uiHealth.UpdateHealth.Invoke((int)health);

        // TODO: Add damage camera effect and SFX

        if (health <= 0)
            // Destroy(gameObject);
            Debug.Log("You're dead!");

    }
}
