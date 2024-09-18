using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    PlayerInput playerInput;
    private InputAction increase;

    public Rigidbody rb;
    public float speed;
    public float currentTurnSpeed;
    public float maxTurnSpeed;

    public bool dynamicTurnBool;


    private void Start()
    {
        playerInput = new PlayerInput();
        playerInput.Enable();

        increase = playerInput.Gameplay.ItemGrab;
        increase.Enable();
        increase.performed += Increase;

        dynamicTurnBool = true;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {

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

        float moveInput = 0;
        float turnInput = Input.GetAxis("Horizontal"); // Left/Right

        rb.MovePosition(transform.position + transform.forward * moveInput * speed * Time.deltaTime);

        if (turnInput != 0)
        {
            Quaternion targetRotation = Quaternion.Euler(0, turnInput * 45, 0) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentTurnSpeed * Time.deltaTime);
        }
    }

    private void Increase(InputAction.CallbackContext context)
    {
        Debug.Log("HELP");
    }
}