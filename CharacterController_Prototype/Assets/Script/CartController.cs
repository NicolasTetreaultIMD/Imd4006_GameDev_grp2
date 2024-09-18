using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    public Rigidbody rb;
    public float speed;
    public float currentTurnSpeed;
    public float maxTurnSpeed;

    public bool dynamicTurnBool;

    private void Start()
    {
        dynamicTurnBool = true;
    }

    private void FixedUpdate()
    {
        //if(Input.GetKeyDown(KeyCode.O)) 
        //{ 
        //    if(dynamicTurnBool == true)
        //    {
        //        dynamicTurnBool = false;
        //        turnSpeed = 0;
        //    }

        //    if (dynamicTurnBool == false)
        //    {
        //        dynamicTurnBool = true;
        //        turnSpeed = 2;
        //    }
        //}

        

        if (dynamicTurnBool == true)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            {
                if (currentTurnSpeed < maxTurnSpeed)
                {
                    currentTurnSpeed += 0.1f;
                }
            }
            else
            {
                currentTurnSpeed = 0;
            }
        }


        float moveInput = Input.GetAxis("Vertical"); // Forward/Backward
        Debug.Log(moveInput);
        float turnInput = Input.GetAxis("Horizontal"); // Left/Right

        rb.MovePosition(transform.position + transform.forward * moveInput * speed * Time.deltaTime);

        if (turnInput != 0)
        {
            Quaternion targetRotation = Quaternion.Euler(0, turnInput * 45, 0) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentTurnSpeed * Time.deltaTime);
        }
    }
}