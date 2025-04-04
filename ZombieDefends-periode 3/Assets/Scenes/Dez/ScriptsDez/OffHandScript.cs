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
