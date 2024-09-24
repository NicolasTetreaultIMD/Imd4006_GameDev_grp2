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

    PlayerInput playerInput;
    private InputAction increase;

    public Transform testCharTrans;
    public Transform testCharTransNoAnim;
    public Rigidbody rb;

    public Transform cartLeftCtrl;
    public Transform cartRightCtrl;
    public Transform cartBodyCtrl;

    public Transform poleRotateLookatRef;

    public float moveInput;
    public float speed;
    public float currentTurnSpeed;
    public float minInCartSpeed = 10f;
    public float maxSpeed = 37.5f;
    public float maxTurnSpeed;
    float speedDecreaseCooldown; //The time in which a speed will decrease
    public float speedDecreaseValue; //How much the speed will decrease by
    public float speedPoleIncreaseRate;

    public bool dynamicTurnBool;

    public CartState cartState;

    private int poleTurnDirection = 1;
    public float cartShakeAmount = 1;

    // VISUAL EFFECTS
    public VisualEffect leftWheelSmoke;
    public VisualEffect rightWheelSmoke;

    public TrailRenderer topLeft_TRL;
    public TrailRenderer topRight_TRL;
    public TrailRenderer botLeft_TRL;
    public TrailRenderer botRight_TRL;

    public ParticleSystem featherEffect; 

    // minTime is used for Trail renderer for wind trails ( VFX )
    private float minTime = 0.0005f;
    private float maxTime = 0.035f;

    // AUDIO

    public AudioSource cartAudio;
    public AudioSource stepAudio;
    public AudioSource wheelAudio;


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

        // make sure the feather effect is stopped when game starts
        if (featherEffect != null)
        {
            featherEffect.Stop();
        }

        //init audio
        cartAudio = GetComponent<AudioSource>();

        // stop audio at the start
        cartAudio.Pause();
        stepAudio.Pause();

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


        // -------------- VISUAL EFFECTS -------------------------


        // Show speed lines when the player exceeds a certain speed
        if (speed >= 27)
        {
            // LERP the TrailRenderer.Time to ease the trail in when it appears
            float time = Mathf.Lerp(minTime, maxTime, Mathf.InverseLerp(30.0f, 40.0f, speed));
            
            topLeft_TRL.time = time;
            topRight_TRL.time = time;
            botLeft_TRL.time = time;
            botRight_TRL.time = time;

            topLeft_TRL.enabled = true;
            topRight_TRL.enabled = true;
            botLeft_TRL.enabled = true;
            botRight_TRL.enabled = true;

        }
        else
        {
            topLeft_TRL.enabled = false;
            topRight_TRL.enabled = false;
            botLeft_TRL.enabled = false;
            botRight_TRL.enabled = false;
        }

        // Check input to determine visual effects when turning
        if (Keyboard.current.aKey.isPressed)
        {
            // Turning left, show left wheel smoke
            if (leftWheelSmoke != null)
                leftWheelSmoke.enabled = true;

            // Hide right wheel smoke
            if (rightWheelSmoke != null)
                rightWheelSmoke.enabled = false;


        }
        else if (Keyboard.current.dKey.isPressed)
        {
            // Turning right, show right wheel smoke
            if (rightWheelSmoke != null)
                rightWheelSmoke.enabled = true;

            // Hide left wheel smoke
            if (leftWheelSmoke != null)
                leftWheelSmoke.enabled = false;

        }
        else // Neither key pressed, hide both wheel smokes
        {
            if (leftWheelSmoke != null)
                leftWheelSmoke.enabled = false;

            if (rightWheelSmoke != null)
                rightWheelSmoke.enabled = false;

        }

        ToggleFeatherEffect(); //call feather effect function

        CartAudioEffect(); //call cart audio effect function

        stepAudioEffect(); //call step audio effect function

        wheelAudioEffect(); //call wheel audio effect function

    }
    private void FixedUpdate()
    {
        //cart shake
        Vector3 randomPoint = UnityEngine.Random.insideUnitSphere * cartShakeAmount * speed + new Vector3(-0.467f, 0.359f, 0);
        cartBodyCtrl.localPosition = randomPoint;

        float shakeAngle = cartShakeAmount * speed * UnityEngine.Random.Range(-1.0f, 1.0f) * 2;
        Debug.Log(shakeAngle);
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
            else if(speed < 0) 
            {
                speed = 0;
            }

            if (speed < minInCartSpeed && cartState == CartState.InCart)
            {
                SwitchCartState(CartState.Running);
            }

            //Forumla to synchronize the player's maximum turn speed relative to their current speed.
            maxTurnSpeed = (0.2f * ((37.5f - speed) / 2.5f)) + 0.6f;

            //JOYSTICK CONTROLS FOR TURNING
            Vector2 leftStick = playerInput.Gameplay.Movement.ReadValue<Vector2>();
            if(Math.Abs(leftStick.x) > 0.05f)
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


            moveInput = 1f; // 0 = Don't Move & 1 = Move
            float turnInput = Input.GetAxis("Horizontal"); // Left/Right, we can replace this with leftStick.x for joystick

            rb.MovePosition(transform.position + transform.forward * moveInput * speed * Time.fixedDeltaTime);

            if (turnInput != 0) //Spherical rotation to simulate steering and not sharp turns.
            {
                Quaternion targetRotation = Quaternion.Euler(0, turnInput * 45, 0) * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentTurnSpeed * Time.fixedDeltaTime);
            }
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

            poleRotateLookatRef.rotation = Quaternion.Euler(0, 15 * Time.fixedDeltaTime * speed * poleTurnDirection, 0) * poleRotateLookatRef.rotation;
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

                cartLeftCtrl.localRotation = Quaternion.Euler(0, 0, 0);
                cartRightCtrl.localRotation = Quaternion.Euler(0, 0, 0);

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

    //function to turn on and off the effect
    private void ToggleFeatherEffect() 
    {
        //play feather effect when running above 5 speed
        if (speed >= 5f) 
        {
            if (featherEffect != null && !featherEffect.isPlaying)
            {
                featherEffect.Play(); //start the feather effect 
            }
        }
        else
        {
            if (featherEffect != null && featherEffect.isPlaying)
            {
                featherEffect.Stop(); //stop the feather effect if it is playing and speed is under 5
            }
        }
    }

    //function to turn on cart audio
    private void CartAudioEffect()
    {
        //Debug.Log("CartAudio enabled: " + cartAudio.enabled);
        if (cartAudio.enabled)
        {
            float fadeSpeed = 1.7f;

            if (speed == 0) // Player is not moving, pause the audio
            {
                if (cartAudio.volume > 0)
                {
                    //fade out the audio but slightly longer by multiplying time by the fadeSpeed constant 
                    cartAudio.volume -= Time.deltaTime*fadeSpeed; 
                    if (cartAudio.volume < 0)
                    {
                        cartAudio.volume = 0;
                        cartAudio.Pause(); //pause the audio when the volume is fully faded out 
                    }
                }
            }
            else // Player is moving
            {
                if (!cartAudio.isPlaying)
                {
                    cartAudio.volume = 0;
                    cartAudio.Play();
                }

                if (cartAudio.volume < 0.4f)
                {
                    //fade in the audio by adjusting the volume over time
                    cartAudio.volume += Time.deltaTime; 
                    if (cartAudio.volume > 0.4f)
                    {
                        //play at full volume
                        cartAudio.volume = 0.4f;
                    }
                }

                //map the pitch of the audio to the speed of the cart
                cartAudio.pitch = Mathf.Lerp(1.5f, 4f, speed / 37.5f);
            }
        }
    }

    //function for step audio
    private void stepAudioEffect()
    {
        if (speed == 0) // Player is not moving, pause audio
        {
            if (stepAudio.isPlaying)
            {
                stepAudio.Pause();
            }
        }
        else // Player is moving, audio plays
        {
            //play audio when player is moving
            if (!stepAudio.isPlaying)
            {
                stepAudio.Play();
            }
            //map speed to pitch
            stepAudio.pitch = Mathf.Lerp(0f, 1.49f, speed / 37.5f); //this should be synced as close as possible to the steps of the character
        }
    }

    //function for wheel audio
    private void wheelAudioEffect()
    {
        //if the player is going fast and turning, play the audio
        if (speed >= 20f && (Keyboard.current.aKey.isPressed || Keyboard.current.dKey.isPressed))
        {
            if (!wheelAudio.isPlaying)
            {
                wheelAudio.Play();
            }
        }
        else
        {       
            if (wheelAudio.isPlaying)
            {
                wheelAudio.Pause();
            }
        }
    }
}