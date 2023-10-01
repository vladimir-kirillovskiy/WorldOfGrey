using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    // https://www.youtube.com/watch?v=geieixA4Mqc
    // [SerializeField] private PlayerShoot playerShoot; // or gun 


    // Rotation
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    private Vector3 gunPosition;

    private Gun gun;

    private Vector3 initGunPos;
    private void Start()
    {
        gun = FindObjectOfType<Gun>();
    }

    private void Update()
    {

        // shooting recoil
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, gun.gunData.returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, gun.gunData.snappiness * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(currentRotation);

        // gun kickback
        gunPosition = Vector3.Lerp(gunPosition, Vector3.zero, gun.gunData.returnSpeed * Time.deltaTime);
        gun.transform.localPosition = gunPosition;
    }


    public void RecoiilFire()
    {
        targetRotation += new Vector3(gun.gunData.recoilX, Random.Range(-gun.gunData.recoilY, gun.gunData.recoilY), Random.Range(-gun.gunData.recoilZ, gun.gunData.recoilZ));
        gunPosition -= new Vector3(0, 0, gun.gunData.kickBackZ);
    }
}
