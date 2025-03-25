using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    public string targetTag = "OutlineTarget"; // Tag for objects that should be outlined
    public Material outlineMaterial; // Assign an outline material in Inspector
    private GameObject currentTarget;
    private Material originalMaterial;

    void Update()
    {
        HandleHover();
        HandleClick();
    }

    void HandleHover()
    {
        // Raycast from mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            // Check if the object has the correct tag
            if (hitObject.CompareTag(targetTag))
            {
                if (currentTarget != hitObject) // If it's a new object, update it
                {
                    ResetOutline();
                    currentTarget = hitObject;

                    Renderer rend = currentTarget.GetComponent<Renderer>();
                    if (rend != null)
                    {
                        originalMaterial = rend.material;
                        rend.material = outlineMaterial; // Apply outline
                    }
                }
                return;
            }
        }

        // Reset if no valid target
        ResetOutline();
    }

    void HandleClick()
    {
        if (Input.GetMouseButtonDown(0) && currentTarget != null) // Left mouse click
        {
            Animator anim = currentTarget.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("PlayAnimation");
            }
        }
    }

    void ResetOutline()
    {
        if (currentTarget != null)
        {
            Renderer rend = currentTarget.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material = originalMaterial; // Restore original material
            }
            currentTarget = null;
        }
    }
}
