using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotScript : MonoBehaviour
{
    public CursorManagerScript cursorManagerScript;
    public ItemScript itemIHold;
    public ItemScript itemInSlot;
    private void Start()
    {
        cursorManagerScript = GameObject.Find("CursorManager").GetComponent<CursorManagerScript>();
    }

    void Update()
    {

        if (itemInSlot != null)
        {
            RectTransform itemRect = itemInSlot.GetComponent<RectTransform>();
            RectTransform slotRect = GetComponent<RectTransform>();

            itemRect.anchoredPosition = slotRect.anchoredPosition;
        }
    }

    public void CheckWhatToDo()
    {
        if (itemIHold == null && cursorManagerScript.itemInHand != null)
        {
            if (itemIHold == null)
            {
                AddItemToSlot(cursorManagerScript.itemInHand);
            }
        }

        else if (itemInSlot != null && cursorManagerScript.itemInHand == null)
        {
            cursorManagerScript.AddItemToHand(itemInSlot);
        }
    }
    public void AddItemToSlot(ItemScript itemToPlaceInSlot)
    {
        itemInSlot = itemToPlaceInSlot;
        print("adding Item");
        print(itemInSlot.name);

        cursorManagerScript.RemoveItemFromHand();

    }

    public void RemoveItemFromSlot(ItemScript itemToRemove)
    {
        print(("Removing " + itemToRemove.name));
        itemInSlot = null;
    }

}