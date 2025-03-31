using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperScope : MonoBehaviour
{
    public Camera sniperCam; // Assign the sniper zoom camera
    private bool isScoped = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click to scope in
        {
            ToggleScope();
        }
    }

    public void ToggleScope()
    {
        isScoped = !isScoped;
        sniperCam.gameObject.SetActive(isScoped); // Enable/disable sniper cam
    }

    public void DisableScope()
    {
        isScoped = false;
        sniperCam.gameObject.SetActive(false); // Always disable scope when switching weapons
    }
}
