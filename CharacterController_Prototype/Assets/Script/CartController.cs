using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.VFX;
using static UnityEngine.Rendering.DebugUI;

public class CarController : MonoBehaviour
{
    public enum CartState { Running, InCart, PoleHolding };
    [Header("Car State")]
    public CartState cartState;

    [Header("Player Input")]
    public Vector2 leftStick;
    PlayerInput playerInput;
    private InputAction increase;

    [Header("Cannon")]
    public Cannon cannon;

    [Header("Character")]
    public Transform Runner;
    public Transform Sitter;
    public Rigidbody rb;

    [Header("Animation")]
    public AnimationController runnerAnimController;
    public AnimationController sitterAnimController;
    public Transform runnerSpot;
    public Transform shooterSpot;
    private float transitionProgress = 0f;
    public bool goingToRunnerSpot;
    public bool goingToShooterSpot;

    [Header("Movement")]
    public float moveInput;
    public float speed;
    public float minInCartSpeed = 10f;
    public float maxSpeed = 37.5f;
    float speedDecreaseCooldown; //The time in which a speed will decrease
    public float speedDecreaseValue; //How much the speed will decrease by

    [Header("Turning")]
    public float currentTurnSpeed;
    public bool turnSpeedToggle;
    public float maxTurnSpeed;
    public bool dynamicTurnBool;

    [Header("Pole Rotation")]
    public Transform cameraPivotRef;
    public Transform poleRotateLookatRef;
    public float cameraPivotSpeed;
    public float speedPoleIncreaseRate;
    private int poleTurnDirection = 1;

    [Header("Mass Shift")]
    public CenterMassManager centerMassManager;
    public float turnTiltStrength;
    private float turnTilt;
    private float prevTurnTilt;

    [Header("Car Shake")]
    public float cartShakeAmount = 1;
    private float prevCartShakePos = 0;



    [Header("Audio & VFX")]
    public Volume globalVolume;
    public float maxMotionBlurIntensity;
    private float prevMotionBlurChange;
    private MotionBlur motionBlur;
    public Animator animationController;
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

        //Transition for the runner to go to the shooter position
        if(goingToShooterSpot)
        {
            transitionProgress += Time.deltaTime * 0.5f;
            Runner.position = Vector3.Lerp(Runner.position, shooterSpot.position, transitionProgress);
            
            if(Runner.position == shooterSpot.position)
            {
                runnerAnimController.ChangeAnimation("Idle", 1);
                goingToShooterSpot = false;
            }

            if(goingToRunnerSpot) //Safety net if both happen at same time
            {
                goingToShooterSpot = false;
            }
        }
        //Transition for the runner to go to the running position
        if (goingToRunnerSpot)
        {
            transitionProgress += Time.deltaTime * 0.25f;
            Runner.position = Vector3.Lerp(Runner.position, runnerSpot.position, transitionProgress);

            if (Runner.position == runnerSpot.position)
            {
                goingToRunnerSpot = false;
                runnerAnimController.ChangeWeight(1, 1);
                runnerAnimController.ChangeAnimation("Idle", 1);
                runnerAnimController.ChangeAnimation("Run_LowerBody", 0);
            }
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

        //JOYSTICK CONTROLS FOR TURNING
        leftStick = playerInput.Gameplay.Movement.ReadValue<Vector2>();

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

            //Update MotionBlur
            globalVolume.profile.TryGet(out motionBlur);
            {
                float motionBlurChange = (speed / maxSpeed) * maxMotionBlurIntensity;
                motionBlur.intensity.value += motionBlurChange - prevMotionBlurChange;
                prevMotionBlurChange = motionBlurChange;
            }


            TurnCart();

            //MOVES CART
            Move();
        }
        else
        {
            CameraPivot();

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

    //Pivots the camera around the pole when in pole grabbing mode
    private void CameraPivot()
    {
        Quaternion targetRotation = Quaternion.Euler(0, leftStick.x * -10, 0) * cameraPivotRef.rotation;

        cameraPivotRef.rotation = Quaternion.Slerp(cameraPivotRef.rotation, targetRotation, cameraPivotSpeed * Time.fixedDeltaTime);
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
            if (leftStick.x > 0)
            {
                turnTilt = -turnTiltStrength * speed;
            }
            //left turn
            else if (leftStick.x < 0)
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
        transitionProgress = 0f; 

        //Perfoms different actions based on the new state
        switch (newState){
            case CartState.Running:
                Debug.Log("RUNNING");
                runnerAnimController.ChangeWeight(0, 1);
                runnerAnimController.ChangeAnimation("JumpBack", 0);
                goingToRunnerSpot = true;
                break;

            case CartState.InCart:
                Debug.Log("IN CART");
                runnerAnimController.ChangeWeight(0, 1);
                runnerAnimController.ChangeAnimation("Jump", 0);
                goingToShooterSpot = true;
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