using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineManager : MonoBehaviour
{
    public string targetTag = "OutlineTarget"; // Tag for objects that should be outlined
    public Material outlineMaterial; // Outline effect material
    private GameObject currentTarget;
    private Renderer currentRenderer;
    private Material[] originalMaterials;

    void Update()
    {
        HandleHover();
        HandleClick();
    }

    void HandleHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag(targetTag))
            {
                if (currentTarget != hitObject)
                {
                    ResetOutline();
                    currentTarget = hitObject;
                    currentRenderer = currentTarget.GetComponent<Renderer>();

                    if (currentRenderer != null)
                    {
                        originalMaterials = currentRenderer.materials;
                        ApplyOutline();
                    }
                }
                return;
            }
        }

        ResetOutline();
    }

    void HandleClick()
    {
        if (Input.GetMouseButtonDown(0) && currentTarget != null)
        {
            Animator anim = currentTarget.GetComponent<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("PlayAnimation");
            }
        }
    }

    void ApplyOutline()
    {
        if (currentRenderer != null && outlineMaterial != null)
        {
            // Add outline without removing original material
            Material[] newMaterials = new Material[originalMaterials.Length + 1];
            originalMaterials.CopyTo(newMaterials, 0);
            newMaterials[newMaterials.Length - 1] = outlineMaterial;
            currentRenderer.materials = newMaterials;
        }
    }

    void ResetOutline()
    {
        if (currentTarget != null && currentRenderer != null)
        {
            currentRenderer.materials = originalMaterials;
            currentTarget = null;
        }
    }
}
