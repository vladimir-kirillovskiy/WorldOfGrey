using StarterAssets;
using System;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;
    public static Action reloadInput;

    private StarterAssetsInputs starterAssetsInputs;

    private void Start()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (starterAssetsInputs.shoot)
        {
            shootInput?.Invoke();
        }

        if (starterAssetsInputs.reload)
        {
            reloadInput?.Invoke();
            starterAssetsInputs.reload = false;
        }
    }
}
