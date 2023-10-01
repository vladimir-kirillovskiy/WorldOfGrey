using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] private float interval = 50f;

    void Start()
    {
        Destroy(gameObject, interval);
    }

}
