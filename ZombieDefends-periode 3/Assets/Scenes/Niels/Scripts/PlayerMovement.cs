using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public float sprintMultiplier;
    public bool isSprinting;

    [Header("Jump Settings")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;


    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDir;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        ResetJump();
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        CheckGrounded();
        if (Input.GetButtonDown("Jump"))
        {
            print("jumped");
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (grounded)
        {
            Sprint();
        }

        if (Input.GetButtonDown("Jump") && readyToJump && grounded)
        {
           
            readyToJump = false;

            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    
    private void MovePlayer()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            rb.AddForce(moveDir.normalized * moveSpeed, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDir.normalized * moveSpeed * airMultiplier, ForceMode.Force);
        }
    }

   
    private void CheckGrounded()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void Sprint()
    {
        if (Input.GetButtonDown("Sprint") && !isSprinting)
        {
            isSprinting = true;
            moveSpeed = moveSpeed * 2f;
        }
        else if (!Input.GetButton("Sprint") && isSprinting)
        {
            isSprinting = false;
            moveSpeed = moveSpeed / 2f;
        }
    }
}
