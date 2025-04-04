using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;

public class InventorySlotScript : MonoBehaviour, IPointerEnterHandler
{
    public CursorManagerScript cursorManagerScript;
    public ItemScript itemInSlot;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    private void Start()
    {
        //zoekt de CursorManager, Description- en NameText Components in de scene
        cursorManagerScript = GameObject.Find("CursorManager").GetComponent<CursorManagerScript>();
        itemNameText = GameObject.Find("ItemNameText").GetComponent<TextMeshProUGUI>();
        itemDescriptionText = GameObject.Find("ItemDescriptionText").GetComponent<TextMeshProUGUI>();
    }
    public void CheckWhatToDo()
    {
        if (itemInSlot == null && cursorManagerScript.itemInHand != null)
        {
            AddItemToSlot(cursorManagerScript.itemInHand);
        }

        else if (itemInSlot != null && cursorManagerScript.itemInHand == null)
        {
            cursorManagerScript.AddItemToHand(itemInSlot);
            RemoveItemFromSlot(itemInSlot);
        }
    }
    public void AddItemToSlot(ItemScript itemToPlaceInSlot)
    {
        cursorManagerScript.RemoveMemory();
        itemInSlot = itemToPlaceInSlot;

        itemInSlot.transform.SetParent(transform);
        itemInSlot.transform.localPosition = Vector3.zero;

        print("adding " + itemInSlot.name);

        cursorManagerScript.RemoveItemFromHand();
    }

    public void RemoveItemFromSlot(ItemScript itemToRemove)
    {
        print(("Removing " + itemToRemove.name));
        itemInSlot = null;
        cursorManagerScript.lastSlot = this;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInSlot != null) 
        {
            itemNameText.text = itemInSlot.itemName;
            itemDescriptionText.text = itemInSlot.itemDescription;
        }
        else
        {
            itemNameText.text = "-";
            itemDescriptionText.text = "-";
        }
    }

    public void DestroyInSlot()
    {
        Destroy(itemInSlot.gameObject);
        itemInSlot= null;
    }
}