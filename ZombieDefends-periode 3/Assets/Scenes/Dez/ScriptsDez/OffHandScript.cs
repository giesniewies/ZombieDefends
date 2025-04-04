using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffHandScript : MonoBehaviour
{
    public CursorManagerScript cursorManagerScript;
    public GameObject potionOfHealth;
    public GameObject potionOfSpeed;
    public ItemScript spawnedObject;
    public GameObject touchedObject;
    public InventorySlotScript offHandSlot;
    public GameObject player;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            if (offHandSlot.itemInSlot != null && offHandSlot.itemInSlot.tag == "PotionOfSpeed")
            {
                offHandSlot.DestroyInSlot();
                FinalLookScript playerScript = GetComponent<FinalLookScript>();
                playerScript.speed += 10;

                // Start a coroutine to wait 10 seconds before reducing speed
                StartCoroutine(RemoveSpeedAfterDelay(playerScript));
            }
        }
    }

    IEnumerator RemoveSpeedAfterDelay(FinalLookScript playerScript)
    {
        yield return new WaitForSeconds(10f); // Wait 10 seconds
        playerScript.speed -= 10;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "PotionOfHealth")
        {
            spawnedObject = Instantiate(potionOfHealth).GetComponent<ItemScript>();
            touchedObject = collision.gameObject;
            cursorManagerScript.FindEmptySlot(spawnedObject);
        }

        if (collision.collider.tag == "PotionOfSpeed")
        {
            spawnedObject = Instantiate(potionOfSpeed).GetComponent<ItemScript>();
            touchedObject = collision.gameObject;
            cursorManagerScript.FindEmptySlot(spawnedObject);
        }
    }

    public void DestroyPowerUp()
    {
        Destroy(touchedObject);
        touchedObject=null;
        spawnedObject = null;
    }

    public void DestroyItem()
    {
        Destroy(spawnedObject.gameObject);
        spawnedObject = null;
    }

}
