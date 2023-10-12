using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour, IDamageable
{

    [SerializeField]
    private GameObject rotatedPart;
    [SerializeField]
    private float health = 1000f;

    // TODO: add constant spawner till tower destroyed

    public void Damage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            
            // Call EndGame()
            
            Rigidbody[] rbs = rotatedPart.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody rb in rbs) rb.isKinematic = false;
            gameObject.GetComponent<Rotator>().enabled = false;
            rotatedPart.GetComponent<Rotator>().enabled = false;
            
        }
    }
}
