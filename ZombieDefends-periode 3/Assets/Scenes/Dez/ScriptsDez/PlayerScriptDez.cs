using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FinalLookScript : MonoBehaviour
{
    public Vector3 moveDir;
    public Vector3 rotateDir;
    public Vector3 camRotate;
    public Transform cam;
    public float speed;
    public float sensitivity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        //PlayerMovement
        moveDir.x = Input.GetAxis("Horizontal");
        moveDir.z = Input.GetAxis("Vertical");
        transform.Translate(moveDir * Time.deltaTime * speed);

        //CameraMovement
        rotateDir.y = Input.GetAxis("Mouse X");
        camRotate.x = Input.GetAxis("Mouse Y");
        transform.Rotate(rotateDir * Time.deltaTime * sensitivity);
        cam.Rotate(-camRotate * Time.deltaTime * sensitivity);
    }
}
