using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManagerScript : MonoBehaviour
{
    public ItemScript itemInHand;
    public Vector2 mousePos;
    public RectTransform canvas;
    public InventorySlotScript lastSlot;
    public InventorySlotScript inventorySlotScript;
    public InventorySlotScript[] slots;
    public CanvasGroup inventoryCanvasGroup;
    public bool isInventoryOpen;
    bool found = false;
    public OffHandScript offHandScript;

    private void Start()
    {
        inventoryCanvasGroup = GameObject.Find("Inventory").GetComponent<CanvasGroup>();
    }

    void Update()
    {
        mousePos = Input.mousePosition;

        if (itemInHand != null)
        {
            // Update the item position directly with mouse position
            RectTransform itemRect = itemInHand.GetComponent<RectTransform>();
            itemRect.position = mousePos;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (itemInHand != null)
            {
                lastSlot.AddItemToSlot(itemInHand);
            }

            if (isInventoryOpen == true)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
    }

    public void CloseInventory()
    {
        inventoryCanvasGroup.alpha = 0f;
        inventoryCanvasGroup.interactable = false;
        inventoryCanvasGroup.blocksRaycasts = false;
        isInventoryOpen = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OpenInventory()
    {
        inventoryCanvasGroup.alpha = 1f;
        inventoryCanvasGroup.interactable = true;
        inventoryCanvasGroup.blocksRaycasts = true;
        isInventoryOpen = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CheckWhatToDo(ItemScript itemToPlaceInHand)
    {
        if (itemInHand == null)
        {
            AddItemToHand(itemToPlaceInHand);
        }
        else
        {
            print("Hand Full!!");
        }
    }

    public void AddItemToHand(ItemScript itemToPlaceInHand)
    {
        itemInHand = itemToPlaceInHand;

        itemInHand.transform.SetParent(canvas.transform, false);

        Button button = itemInHand.GetComponent<Button>();
        if (button != null)
        {
            button.interactable = false;
        }

        Image image = itemInHand.GetComponent<Image>();
        if (image != null)
        {
            image.raycastTarget = false;
        }
    }

    public void RemoveItemFromHand()
    {
        if (itemInHand != null)
        {
            print("Removing " + itemInHand.name + " from hand");
            itemInHand = null;
        }
    }

    public void RemoveMemory()
    {
        if (lastSlot != null)
        {
            lastSlot = null;
        }
    }

    public void FindEmptySlot(ItemScript itemToAdd)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemInSlot == null) // Controleer of er nog geen item in het slot zit
            {
                // Roep AddItemToSlot aan vanuit het script van dit slot
                slots[i].AddItemToSlot(itemToAdd);
                itemToAdd = null;
                found = true;

                break; // Stop de loop zodra een leeg slot is gevonden
            }
        }

        if (!found)
        {
            offHandScript.DestroyItem();
            print("Inventory is full");
        }
        else
        {
            offHandScript.DestroyPowerUp();
            found = false;
        }
    }
}