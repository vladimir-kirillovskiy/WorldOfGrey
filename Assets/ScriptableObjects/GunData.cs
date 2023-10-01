using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public new string name;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;
    public bool Auto;

    [Header("Reloading")]
    public int currentAmmo;
    public int magSize;
    public int fireRate;
    public float reloadTime;

    public bool reloading;

    [Header("Hipfire Recoil")]
    public float recoilX;
    public float recoilY;
    public float recoilZ;

    public float kickBackZ;

    [Header("Responce Settings")]
    public float snappiness;
    public float returnSpeed;
}
