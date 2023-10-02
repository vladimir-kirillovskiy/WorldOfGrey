using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] public GunData gunData;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Transform cam;

    [SerializeField] private GameObject vfxHit;
    [SerializeField] private GameObject vfxMiss;

    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] AudioSource shotSound;
    [SerializeField] AudioSource reloadSound;

    [SerializeField] private GameObject bulletHolePref;

    [SerializeField] TrailRenderer bulletTrail;

    private UI_AmmoCount uiAmmoCount;

    private CameraRecoil cameraRecoil;


    float timeSinceLastShot;

    Vector3 hitPosition;

    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReloading;

        cameraRecoil = FindObjectOfType<CameraRecoil>();
        uiAmmoCount = FindObjectOfType<UI_AmmoCount>();
        uiAmmoCount?.UpdateAmmo.Invoke();
        gunData.reloading = false;
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        Debug.DrawRay(muzzle.position, muzzle.forward * gunData.maxDistance, Color.black);
    }

    private void OnDisable() => gunData.reloading = false;

    public void StartReloading()
    {
        if (!gunData.reloading && this.gameObject.activeSelf)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        // TODO: make 2 types of reload, if there is a bullet in the barrel or not
        gunData.reloading = true;
        uiAmmoCount?.UpdateAmmo.Invoke();
        reloadSound.Play();

        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.reloading = false;
        gunData.currentAmmo = gunData.magSize;
        uiAmmoCount?.UpdateAmmo.Invoke();

    }

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    public void Shoot()
    {
        if (gunData.currentAmmo > 0)
        {
            if (CanShoot() && this.gameObject.activeSelf)
            {
                TrailRenderer trail = Instantiate(bulletTrail, muzzle.position, Quaternion.identity);
                Debug.DrawRay(cam.position, cam.forward * gunData.maxDistance, Color.red, 2);
                if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, gunData.maxDistance))
                {
                    IDamageable damageable = hitInfo.transform.GetComponent<IDamageable>();
                    damageable?.Damage(gunData.damage);

                    hitPosition = hitInfo.point;
                    
                    StartCoroutine(SpawnTrail(trail, hitInfo.point));

                    if (damageable != null) 
                        Instantiate(vfxHit, hitPosition, Quaternion.identity, hitInfo.transform);
                    else
                    {
                        Instantiate(vfxMiss, hitPosition, Quaternion.identity);
                        Instantiate(bulletHolePref, hitPosition + (hitInfo.normal * .01f), Quaternion.FromToRotation(Vector3.up, hitInfo.normal));
                    }
                } 
                else
                {
                    Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
                    Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

                    StartCoroutine(SpawnTrail(trail, ray.GetPoint(10)));
                }

                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShot();
            }
        }
    }

    private void OnGunShot()
    {
        shotSound.Play();
        muzzleFlash.Play();
        cameraRecoil.RecoiilFire();
        uiAmmoCount?.UpdateAmmo.Invoke();
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 point)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while(time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        // isShooting = false
        trail.transform.position = point;
        // instanciate hit particle
        Destroy(trail.gameObject, trail.time);
    }
}
