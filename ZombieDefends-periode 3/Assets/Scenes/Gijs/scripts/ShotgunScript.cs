using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShotgunScript : MonoBehaviour
{
    [Header("Shooting Settings")]
    public float range = 50f; // Shorter range for shotguns
    public float damage = 10f; // Lower individual pellet damage
    public int pelletsPerShot = 8; // Number of pellets fired per shot
    public float spreadAngle = 10f; // Spread angle for shotgun pellets
    public float fireRate = 1.0f; // Lower fire rate for shotguns
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public LayerMask hitMask;

    [Header("Ammo & Reload")]
    public int maxAmmo = 6; // Smaller magazine size
    private int currentAmmo;
    public float reloadTime = 2.5f; // Slower reload for shotgun
    private bool isReloading = false;
    private float nextTimeToFire = 0f;

    [Header("Recoil")]
    public Recoil recoilScript;

    [Header("UI Elements")]
    public TextMeshProUGUI ammoText;

    [Header("Audio")]
    public AudioSource gunAudioSource;
    public AudioClip gunshotSound;
    public AudioClip reloadSound;
    public AudioClip cockingSound; // Pump-action sound

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoText();
    }

    void Update()
    {
        if (isReloading)
            return;

        if (currentAmmo <= 0 && Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }

    void Shoot()
    {
        if (currentAmmo <= 0) return;

        // Play muzzle flash effect
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // Play gunshot sound
        if (gunAudioSource != null && gunshotSound != null)
        {
            gunAudioSource.PlayOneShot(gunshotSound);
        }

        currentAmmo--;

        // Apply recoil
        if (recoilScript != null)
        {
            recoilScript.ApplyRecoil();
        }

        // Fire multiple pellets in a spread pattern
        for (int i = 0; i < pelletsPerShot; i++)
        {
            FirePellet();
        }

        // Play pump-action sound (if it's a pump shotgun)
        if (gunAudioSource != null && cockingSound != null)
        {
            StartCoroutine(PlayPumpActionSound());
        }

        UpdateAmmoText();
    }

    void FirePellet()
    {
        Vector3 shootDirection = fpsCam.transform.forward;

        // Apply random spread to each pellet
        shootDirection += new Vector3(
            Random.Range(-spreadAngle, spreadAngle) * 0.01f,
            Random.Range(-spreadAngle, spreadAngle) * 0.01f,
            0);

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, shootDirection, out hit, range, hitMask))
        {
            Debug.Log("Hit: " + hit.transform.name);
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    IEnumerator PlayPumpActionSound()
    {
        yield return new WaitForSeconds(0.3f); // Small delay before pump sound
        gunAudioSource.PlayOneShot(cockingSound);
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        // Play reload sound
        if (gunAudioSource != null && reloadSound != null)
        {
            gunAudioSource.PlayOneShot(reloadSound);
        }

        yield return new WaitForSeconds(reloadTime - 0.3f); // Small delay before cocking sound

        // Play pump-action sound after reload
        if (gunAudioSource != null && cockingSound != null)
        {
            gunAudioSource.PlayOneShot(cockingSound);
        }

        yield return new WaitForSeconds(0.3f); // Small delay after cocking sound

        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateAmmoText();
    }

    void UpdateAmmoText()
    {
        ammoText.text = "Ammo: " + currentAmmo + "/" + maxAmmo;
    }
}
