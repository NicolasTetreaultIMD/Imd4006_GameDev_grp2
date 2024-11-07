using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.VFX;
using static UnityEngine.Rendering.DebugUI;

public class CarController : MonoBehaviour
{
    public enum CartState { Running, InCart, PoleHolding };

    public Animator animationController;
    public CenterMassManager centerMassManager;

    PlayerInput playerInput;
    private InputAction increase;
    public Vector2 leftStick;
    public Cannon cannon;

    public Transform testCharTrans;
    public Transform testCharTransNoAnim;
    public Rigidbody rb;

    /*public Transform cartLeftCtrl;
    public Transform cartRightCtrl;
    public Transform cartBodyCtrl;*/

    public Transform poleRotateLookatRef;

    public float moveInput;
    public float speed;
    public float currentTurnSpeed;
    public float minInCartSpeed = 10f;
    public float maxSpeed = 37.5f;
    public bool turnSpeedToggle;
    public float maxTurnSpeed;
    float speedDecreaseCooldown; //The time in which a speed will decrease
    public float speedDecreaseValue; //How much the speed will decrease by
    public float speedPoleIncreaseRate;

    public bool dynamicTurnBool;

    public CartState cartState;

    private int poleTurnDirection = 1;
    public float cartShakeAmount = 1;
    private float prevCartShakePos = 0;

    public float turnTiltStrength;
    private float turnTilt;
    private float prevTurnTilt;

    public ParticleSystem featherEffect; 


    private void Start()
    {
        speed = 0;
        currentTurnSpeed = 0;
        maxTurnSpeed = 3.6f;
        speedDecreaseValue = 0.5f;

        playerInput = new PlayerInput();
        playerInput.Enable();

        increase = playerInput.Gameplay.SpeedIncrease;
        increase.Enable();
        increase.performed += Increase;

        dynamicTurnBool = true;

        cartState = CartState.Running;
    }

    private void Update()
    {
        //Syncs the speed of the player's leg animation to the player's actual speed in the game.
        float newLowerBodySpeed = speed / 18.75f;
        animationController.SetFloat("LowerBodySpeed",newLowerBodySpeed);

        if (speed == 0)
        {
            animationController.SetBool("IsIdle", true);
            //animationController.Play("Idle", 1);
        }
        else
        {
            animationController.SetBool("IsIdle", false);
            //animationController.Play("Run_LowerBody", 1);
        }
    }

    private void FixedUpdate()
    {
        turnTilt = 0;

        //cart shake
        float shakePos = speed * (UnityEngine.Random.Range(centerMassManager.minHeight, centerMassManager.maxHeight) / maxSpeed);
        float motorShake = UnityEngine.Random.Range(centerMassManager.minHeight, centerMassManager.maxHeight) / 4;

        centerMassManager.massCenter.y += shakePos + motorShake - prevCartShakePos;
        prevCartShakePos = shakePos + motorShake;

        float shakeAngle = cartShakeAmount * speed * UnityEngine.Random.Range(-1.0f, 1.0f) * 2;
        //Debug.Log(shakeAngle);
        transform.rotation = Quaternion.Euler(0, shakeAngle, 0) * transform.rotation;

        if (cartState != CartState.PoleHolding)
        {
            //Gradually decreases the player's speed over time if they aren't clicking the accelerate button.
            if (speed > 0)
            {
                if (Time.time >= speedDecreaseCooldown)
                {
                    speed -= speedDecreaseValue;
                    speedDecreaseCooldown = Time.time + 0.5f;
                }
            }
            else if (speed < 0)
            {
                speed = 0;
            }

            if (speed < minInCartSpeed && cartState == CartState.InCart)
            {
                SwitchCartState(CartState.Running);
            }

            //JOYSTICK CONTROLS FOR TURNING
            leftStick = playerInput.Gameplay.Movement.ReadValue<Vector2>();
            // if (Math.Abs(leftStick.x) > 0.05f)
            // {
            //     if (currentTurnSpeed < maxTurnSpeed)
            //     {
            //         currentTurnSpeed += (maxTurnSpeed / 10);
            //     }
            // }
            // else
            // {
            //     currentTurnSpeed = 0;
            // }


            // moveInput = 1f; // 0 = Don't Move & 1 = Move
            // float turnInput = Input.GetAxis("Horizontal"); // Left/Right, we can replace this with leftStick.x for joystick

            // rb.MovePosition(transform.position + transform.forward * moveInput * speed * Time.fixedDeltaTime);

            // if (speed != 0) //Spherical rotation to simulate steering and not sharp turns.
            // {
            //     /*Quaternion targetRotation = Quaternion.Euler(0, turnInput * 45, 0) * transform.rotation;
            //     transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentTurnSpeed * Time.fixedDeltaTime);*/

            //     Quaternion targetRotation = Quaternion.Euler(0, turnInput * 45 + centerMassManager.turnIncrease, 0) * transform.rotation;
            //     transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, maxTurnSpeed * Time.fixedDeltaTime);

            //     //right turn
            //     if (turnInput > 0)
            //     {
            //         turnTilt = -turnTiltStrength * speed;
            //     }
            //     //left turn
            //     else if (turnInput < 0)
            //     {
            //         turnTilt = turnTiltStrength * speed;
            //     }
            // }
            TurnCart();

            //MOVES CART
            Move();
        }
        else
        {
            //Gradually increases the player's speed while on the pole
            if (speed < maxSpeed)
            {
                speed += speedPoleIncreaseRate;
            }
            else if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }

            //right turn
            if (poleTurnDirection > 0)
            {
                turnTilt = -turnTiltStrength * 1.5f * speed;
            }
            //left turn
            else if (poleTurnDirection < 0)
            {
                turnTilt = turnTiltStrength * 1.5f * speed;
            }

            poleRotateLookatRef.rotation = Quaternion.Euler(0, 15 * Time.fixedDeltaTime * speed * poleTurnDirection, 0) * poleRotateLookatRef.rotation;
        }

        centerMassManager.massCenter.x += turnTilt - prevTurnTilt;
        prevTurnTilt = turnTilt;
    }

    private void Move()
    {
        moveInput = 1f; // 0 = Don't Move & 1 = Move

        rb.MovePosition(transform.position + transform.forward * moveInput * speed * Time.fixedDeltaTime);

        if (speed != 0) //Spherical rotation to simulate steering and not sharp turns.
        {
            // Quaternion targetRotation = Quaternion.Euler(0, leftStick.x * 45, 0) * transform.rotation;
            // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentTurnSpeed * Time.fixedDeltaTime);

            Quaternion targetRotation = Quaternion.Euler(0, leftStick.x * 45 + centerMassManager.turnIncrease, 0) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, maxTurnSpeed * Time.fixedDeltaTime);

            //right turn
            if (turnInput > 0)
            {
                turnTilt = -turnTiltStrength * speed;
            }
            //left turn
            else if (turnInput < 0)
            {
                turnTilt = turnTiltStrength * speed;
            }
        }
    }

    private void Increase(InputAction.CallbackContext context) //Player Input function for increasing speed
    {
        if (cartState == CartState.Running || cartState == CartState.PoleHolding)
        {
            speedDecreaseCooldown = Time.time + 0.15f;
            if (speed < maxSpeed)
            {
                speed += 2.5f;
            }
            else if (speed >= maxSpeed)
            {
                speed = maxSpeed;
            
                if (cartState == CartState.Running)
                {
                    SwitchCartState(CartState.InCart);
                }
            }
        }

        if(currentTurnSpeed > maxTurnSpeed) //Makes sure the current turn speed does not exceed the max turn speed
        {
            currentTurnSpeed = maxTurnSpeed;
        }    
    }

    private void TurnCart()
    {
        //Forumla to synchronize the player's maximum turn speed relative to their current speed.
        if (turnSpeedToggle == false)
        {
            maxTurnSpeed = (0.2f * ((37.5f - speed) / 2.5f)) + 0.6f;
        }
        else
        {
            maxTurnSpeed = (0.2f * ((37.5f - speed) / 2.5f)) + 1.2f;
        }

        if (Math.Abs(leftStick.x) > 0.05f)
        {
            if (currentTurnSpeed < maxTurnSpeed)
            {
                currentTurnSpeed += (maxTurnSpeed / 10);
            }
        }
        else
        {
            currentTurnSpeed = 0;
        }
    }

    //Controls the current state of the controller
    public void SwitchCartState(CartState newState)
    {
        cartState = newState;

        //Perfoms different actions based on the new state
        switch (newState){
            case CartState.Running:
                Debug.Log("RUNNING");
                testCharTrans.gameObject.SetActive(true);
                testCharTransNoAnim.gameObject.SetActive(false);

                /*centerMassManager.massCenter.x = 0;*/

                //CODE FOR CREATING A TRANSITION BETWEEN THE TWO STATES:
                //animationController.SetBool("IsInCart", false);
                //testCharTrans.localPosition = new Vector3(testCharTrans.localPosition.x, testCharTrans.localPosition.y, testCharTrans.localPosition.z - 1);

                break;
            case CartState.InCart:
                Debug.Log("IN CART");
                testCharTrans.gameObject.SetActive(false);
                testCharTransNoAnim.gameObject.SetActive(true);


                //CODE FOR CREATING A TRANSITION BETWEEN THE TWO STATES:
                //animationController.SetBool("IsInCart", true);
                //testCharTrans.localPosition = new Vector3(testCharTrans.localPosition.x, testCharTrans.localPosition.y, testCharTrans.localPosition.z + 1);
                break;
            case CartState.PoleHolding:
                Debug.Log("Holding Pole");

                //then rotate point to the right of cart
                if (Vector3.Dot(transform.right, Vector3.Normalize(poleRotateLookatRef.position - transform.position)) > 0)
                {
                    transform.LookAt(poleRotateLookatRef);
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 90, 0);
                    poleTurnDirection = 1;
                }
                //Rotate point to the left of cart
                else
                {
                    transform.LookAt(poleRotateLookatRef);
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 90, 0);
                    poleTurnDirection = -1;
                }

                transform.parent = poleRotateLookatRef;
                break;
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ItemBox")
        {
            Debug.Log("Item loaded");
            cannon.LoadCannon(GameObject.Find(other.name + " Item"));
            Destroy(other.gameObject);
        }
    }
}