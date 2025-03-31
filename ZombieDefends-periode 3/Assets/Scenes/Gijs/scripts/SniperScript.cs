using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SniperScript : MonoBehaviour
{
    [Header("Shooting Settings")]
    public float range = 300f;
    public float damage = 100f;
    public float fireRate = 1.5f;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public LayerMask hitMask;

    [Header("Scope Settings")]
    public Camera sniperScopeCam; // Separate camera for scoping
    public GameObject scopeOverlay; // UI Scope Image
    private bool isScoped = false;

    [Header("Ammo & Reload")]
    public int maxAmmo = 5;
    private int currentAmmo;
    public float reloadTime = 3.5f;
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
    public AudioClip cockingSound;

    void Start()
    {
        currentAmmo = maxAmmo;
        sniperScopeCam.enabled = false; // Disable the scope camera at start
        if (scopeOverlay != null)
        {
            scopeOverlay.SetActive(false); // Hide the scope overlay at start
        }
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

        if (Input.GetMouseButtonDown(1)) // Right-click to scope
        {
            ToggleScope();
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
            Debug.Log("Hit: " + hit.transform.name);

            // Headshot detection
            if (hit.collider.CompareTag("Head"))
            {
                Debug.Log("Headshot!");
                EnemyHealth enemy = hit.collider.GetComponentInParent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage * 2); // Double damage for headshots
                }
            }
            else
            {
                EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }

        UpdateAmmoText();
    }

    void ToggleScope()
    {
        isScoped = !isScoped;

        if (isScoped)
        {
            sniperScopeCam.enabled = true; // Enable sniper cam
            fpsCam.enabled = false; // Disable main camera
            if (scopeOverlay != null) scopeOverlay.SetActive(true); // Show scope overlay
        }
        else
        {
            sniperScopeCam.enabled = false; // Disable sniper cam
            fpsCam.enabled = true; // Re-enable main camera
            if (scopeOverlay != null) scopeOverlay.SetActive(false); // Hide scope overlay
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        if (gunAudioSource != null && reloadSound != null)
        {
            gunAudioSource.PlayOneShot(reloadSound);
        }

        yield return new WaitForSeconds(reloadTime - 0.3f);

        if (gunAudioSource != null && cockingSound != null)
        {
            gunAudioSource.PlayOneShot(cockingSound);
        }

        yield return new WaitForSeconds(0.3f);

        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateAmmoText();
    }

    void UpdateAmmoText()
    {
        ammoText.text = "Ammo: " + currentAmmo + "/" + maxAmmo;
    }
}
