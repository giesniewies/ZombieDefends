using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalShootScript : MonoBehaviour
{
    public RaycastHit hit;
    public float distance;
    public bool inSight = false;
    public RawImage crosshair;
    public bool buttonDown = false;
    void Start()
    {

    }

    void Update()
    {                       //Origin            //Direction        //Variable //Afstand
        if (Physics.Raycast(transform.position, transform.forward, out hit, distance))
        {
            if (hit.collider.tag == "Enemy")
            {
                crosshair.color = Color.red;
                if (Input.GetButtonDown("Fire1"))
                {
                    Destroy(hit.collider.gameObject);
                    print(hit.collider.gameObject.transform.position);
                }
            }

            if (hit.collider.tag == "Button")
            {
                crosshair.color = Color.red;
            }

            if (hit.collider.tag == "Enemy" || hit.collider.tag == "Button")
            {
                crosshair.color = Color.red;
            }
            else
            {
                crosshair.color = Color.white;
            }
        }
    }
}
