using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public int maxAmmo = 30;
    private int currentAmmo;

    void Start()
    {
        currentAmmo = maxAmmo; // Start with full ammo
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public void UseAmmo(int amount)
    {
        currentAmmo = Mathf.Max(0, currentAmmo - amount);
    }

    public void Reload()
    {
        currentAmmo = maxAmmo;
    }
}
