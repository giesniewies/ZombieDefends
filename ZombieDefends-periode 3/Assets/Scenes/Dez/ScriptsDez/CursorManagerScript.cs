using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManagerScript : MonoBehaviour
{
    public ItemScript itemInHand;
    public Vector2 mousePos;
    public RectTransform canvas;

    void Update()
    {
        mousePos = Input.mousePosition;

        if (itemInHand != null)
        {
            RectTransform itemRect = itemInHand.GetComponent<RectTransform>();

            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, mousePos, canvas.GetComponent<Canvas>().worldCamera, out localPoint))
            {
                itemRect.anchoredPosition = localPoint;
            }
        }
    }

    public void CheckWhatToDo(ItemScript itemToPlaceInHand)
    {
        if (itemInHand == null) 
        {
            AddItemToHand(itemToPlaceInHand);
        }else
        {
            print("Hand Full!!");
        }
    }

    public void AddItemToHand(ItemScript itemToPlaceInHand)
    {
        itemInHand = itemToPlaceInHand;

        // Zoek de Button-component en schakel deze uit
        Button button = itemInHand.GetComponent<Button>();
        if (button != null)
        {
            button.interactable = false; // Zorgt ervoor dat de knop niet meer klikbaar is
        }

        // Zoek de Image-component en zet RaycastTarget uit
        Image image = itemInHand.GetComponent<Image>();
        if (image != null)
        {
            image.raycastTarget = false; // Zorgt ervoor dat de afbeelding geen raycasts ontvangt
        }
    }

    public void RemoveItemFromHand()
    {
        if (itemInHand != null)
        {
            print("Removing " + itemInHand.name);
            // Clear the item in hand
            itemInHand = null;
        }
    }
}