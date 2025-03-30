using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Add this if you're using TextMeshPro

public class GunScript : MonoBehaviour
{
    [Header("Shooting Settings")]
    public float range = 100f;
    public float damage = 25f;
    public float fireRate = 0.2f;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public LayerMask hitMask;

    [Header("Ammo & Reload")]
    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1.5f;  // Adjusted for realistic timing
    private bool isReloading = false;
    private float nextTimeToFire = 0f;

    [Header("Recoil")]
    public Recoil recoilScript;

    [Header("UI Elements")]
    public TextMeshProUGUI ammoText;

    [Header("Audio")]
    public AudioSource gunAudioSource;
    public AudioClip gunshotSound;
    public AudioClip reloadSound; // New: Reload sound
    public AudioClip cockingSound; // New: Cocking sound after reload

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

        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        if (gunAudioSource != null && gunshotSound != null)
        {
            gunAudioSource.PlayOneShot(gunshotSound);
        }

        currentAmmo--;

        if (recoilScript != null)
        {
            recoilScript.ApplyRecoil();
        }

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, hitMask))
        {
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        UpdateAmmoText();
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

        // Play cocking sound
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
