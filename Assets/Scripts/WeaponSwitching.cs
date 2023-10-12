using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitching : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] weapons;

    [Header("Keys")]
    [SerializeField] private InputActionReference[] keyActions;

    [Header("Settings")]
    [SerializeField] private float switchTime;

    private int selectedWeapon;
    private float timeSinceLastSwitch;

    private UI_AmmoCount uiAmmoCount;


    private void Start()
    {
        uiAmmoCount = FindObjectOfType<UI_AmmoCount>();

        SetWeapons();
        Select(selectedWeapon);

        timeSinceLastSwitch = 0;
    }

    private void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        for (int i = 0; i < keyActions.Length; i++)
        {
            if (keyActions[i].ToInputAction().IsPressed() && timeSinceLastSwitch >= switchTime)
            {
                GunData gunData = weapons[i].GetComponent<Gun>().gunData;
                if (!gunData.available) continue;
                selectedWeapon = i;
            }

            if (previousSelectedWeapon != selectedWeapon) 
            {
                Select(selectedWeapon);
            }

            timeSinceLastSwitch += Time.deltaTime;
        }
    }

    private void SetWeapons()
    {
        weapons = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            weapons[i] = transform.GetChild(i);
        }
    }


    private void Select(int weaponIndex)
    {
        
        
        for (int i = 0; i < weapons.Length; i++)
        {
            
            weapons[i].gameObject.SetActive(i == weaponIndex);
        }

        timeSinceLastSwitch = 0;
        OnWeaponSelected();
    }

    private void OnWeaponSelected()
    {
        uiAmmoCount.UpdateAmmo.Invoke();
    }
}
