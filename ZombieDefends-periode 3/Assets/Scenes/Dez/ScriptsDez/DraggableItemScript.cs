using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItemScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Vector2 originalPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("Canvas not found! Make sure this object is inside a Canvas.");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        GameObject closestSlot = FindClosestSlot();
        if (closestSlot != null)
        {
            rectTransform.position = closestSlot.transform.position;
        }
        else
        {
            rectTransform.anchoredPosition = originalPosition;
        }
    }

    private GameObject FindClosestSlot()
    {
        GameObject[] slots = GameObject.FindGameObjectsWithTag("InventorySlot");
        GameObject closestSlot = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject slot in slots)
        {
            float distance = Vector2.Distance(rectTransform.position, slot.transform.position);
            if (distance < closestDistance && distance < 50f) // 50f is de snap afstand
            {
                closestDistance = distance;
                closestSlot = slot;
            }
        }

        return closestSlot;
    }
}
