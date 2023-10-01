using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UI_AmmoCount : MonoBehaviour
{
    [HideInInspector] public UnityEvent UpdateAmmo;

    private Gun[] guns;
    private Gun currentGun;

    [SerializeField] TextMeshProUGUI ammoText;

    private void Start()
    {
        // 
        guns = FindObjectsOfType<Gun>(true);
        UpdateAmmo.AddListener(UpdateUI);
    }
    private void OnDestroy()
    {
        UpdateAmmo.RemoveListener(UpdateUI);
    }

    private void CheckCurrentGun()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            if (guns[i].gameObject.activeSelf)
            {
                currentGun = guns[i];
            }
        }
    }

    private void UpdateUI()
    {
        CheckCurrentGun();

        if (currentGun.gunData.reloading)
        {
            ammoText.text = "--/" + currentGun.gunData.magSize;
        }
        else
        {
            ammoText.text = currentGun.gunData.currentAmmo + "/" + currentGun.gunData.magSize;
        }
    }
}
