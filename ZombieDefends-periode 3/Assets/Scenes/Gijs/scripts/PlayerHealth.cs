using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro
using UnityEngine.EventSystems; // Allows UI interaction

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI Elements")]
    public Slider healthBar; // Assign in Inspector
    public TextMeshProUGUI healthText; // Now using TextMeshProUGUI
    public GameObject gameOverPanel; // Assign in Inspector (Make sure it's disabled at start)

    [Header("Damage Settings")]
    public float damageCooldown = 1.0f; // Time between damage instances
    private bool canTakeDamage = true;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // Hide game over screen at start
        }
    }

    void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth; // Update slider
        }
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth; // Update TextMeshPro text
        }
    }

    public void TakeDamage(int damage)
    {
        if (canTakeDamage && !isDead)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Prevent negative HP

            UpdateHealthUI();

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(DamageCooldown());
            }
        }
    }

    IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    void Die()
    {
        if (isDead) return; // Prevent multiple deaths

        isDead = true;
        Debug.Log("Player Died!");

        // Show the game over UI
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Disable player controls
        if (GetComponent<PlayerMovement>() != null)
            GetComponent<PlayerMovement>().enabled = false; // Disable movement script

        if (GetComponent<WeaponSwitcher>() != null)
            GetComponent<WeaponSwitcher>().enabled = false; // Disable weapon switching

        if (GetComponent<GunScript>() != null)
            GetComponent<GunScript>().enabled = false; // Disable shooting script

        if (GetComponent<SniperScope>() != null)
            GetComponent<SniperScope>().enabled = false; // Disable sniper zoom

        // Disable all weapons
        foreach (Transform weapon in transform)
        {
            if (weapon.gameObject.activeSelf)
                weapon.gameObject.SetActive(false);
        }

        // Unlock and show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Select the first UI button
        Button firstButton = gameOverPanel.GetComponentInChildren<Button>();
        if (firstButton != null)
        {
            firstButton.Select();
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(25); // Adjust damage as needed
        }
    }
}
