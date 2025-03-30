using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    [Header("Recoil Settings")]
    public float recoilX = 2f; // Vertical recoil (lower this if it's too strong)
    public float recoilY = 1f; // Horizontal sway
    public float recoilZ = 0.5f; // Slight tilt effect

    public float recoilSpeed = 6f;  // How fast the recoil happens
    public float returnSpeed = 4f;  // How fast it returns to normal

    private Vector3 originalRotation;
    private Vector3 currentRecoil;

    void Start()
    {
        originalRotation = transform.localEulerAngles;
    }

    void Update()
    {
        // Smooth recoil transition
        currentRecoil = Vector3.Lerp(currentRecoil, Vector3.zero, returnSpeed * Time.deltaTime);
        transform.localEulerAngles = originalRotation + currentRecoil;
    }

    public void ApplyRecoil()
    {
        // Apply small random variance for more realistic recoil
        float randomX = Random.Range(recoilX * 0.8f, recoilX * 1.2f);
        float randomY = Random.Range(-recoilY, recoilY);

        // Apply the recoil effect
        currentRecoil += new Vector3(-randomX, randomY, recoilZ);
    }
}
