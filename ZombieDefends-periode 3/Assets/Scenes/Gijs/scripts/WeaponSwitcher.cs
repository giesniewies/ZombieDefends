using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // For UI text

public class WeaponSwitcher : MonoBehaviour
{
    public GameObject[] weapons; // Array of weapons
    private int currentWeaponIndex = 0;

    [Header("UI Elements")]
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI ammoText;

    private WeaponBase[] weaponScripts; // Stores references to all weapon scripts
    private SniperScope sniperScope; // Reference to sniper scope script

    void Start()
    {
        // Store references to all weapon scripts
        weaponScripts = new WeaponBase[weapons.Length];
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
            {
                weaponScripts[i] = weapons[i].GetComponent<WeaponBase>();
            }
        }

        sniperScope = FindObjectOfType<SniperScope>(); // Find sniper scope script in the scene

        SelectWeapon(currentWeaponIndex); // Start with the first weapon
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f) // Scroll up
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;
            SelectWeapon(currentWeaponIndex);
        }
        else if (scroll < 0f) // Scroll down
        {
            currentWeaponIndex--;
            if (currentWeaponIndex < 0) currentWeaponIndex = weapons.Length - 1;
            SelectWeapon(currentWeaponIndex);
        }

        // Select weapon with number keys (1, 2, 3)
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Length > 1) SelectWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3) && weapons.Length > 2) SelectWeapon(2);
    }

    void SelectWeapon(int index)
    {
        // Disable sniper scope if switching weapons
        if (sniperScope != null)
        {
            sniperScope.DisableScope();
        }

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
            {
                bool isActive = (i == index);
                weapons[i].SetActive(isActive);

                // Stop muzzle flash if weapon is deactivated
                ParticleSystem muzzleFlash = weapons[i].GetComponentInChildren<ParticleSystem>();
                if (muzzleFlash != null)
                {
                    muzzleFlash.Stop();
                }
            }
        }

        // Ensure the UI updates instantly
        Invoke("UpdateWeaponUI", 0.05f);
    }

    void UpdateWeaponUI()
    {
        GameObject currentWeapon = weapons[currentWeaponIndex];

        if (weaponNameText != null)
        {
            weaponNameText.text = currentWeapon.name; // Update weapon name
        }

        // Get the weapon script and update ammo UI
        WeaponBase weaponScript = weaponScripts[currentWeaponIndex];
        if (weaponScript != null && ammoText != null)
        {
            ammoText.text = "Ammo: " + weaponScript.GetCurrentAmmo() + "/" + weaponScript.maxAmmo;
        }
    }
}
