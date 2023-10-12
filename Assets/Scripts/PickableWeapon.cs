using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject equipedWeapon;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GunData gunData = equipedWeapon?.GetComponent<Gun>().gunData;
            gunData.available = true;

            Destroy(gameObject);
        }
    }
}
