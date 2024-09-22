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
    public Animator animationController;

    PlayerInput playerInput;
    private InputAction increase;


    public Rigidbody rb;

    public float moveInput;
    public float speed;
    public float currentTurnSpeed;
    public float maxTurnSpeed;
    float speedDecreaseCooldown; //The time in which a speed will decrease
    public float speedDecreaseValue; //How much the speed will decrease by

    public bool dynamicTurnBool;


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


        //VisualEffect leftVfx = leftWheelSmoke.GetComponent<VisualEffect>();
        //VisualEffect rightVfx = rightWheelSmoke.GetComponent<VisualEffect>();

        //if (Keyboard.current.aKey.isPressed)
        //{
        //    // Turn left, play left wheel smoke
        //    if (leftWheelSmoke != null)
        //    {
        //        if (leftVfx != null) leftVfx.Play(); // Start the visual effect
        //        Debug.Log("Left wheel smoke started");

        //    }

        //    // Stop right wheel smoke 
        //    if (rightWheelSmoke != null)
        //    {
        //        if (rightVfx != null) rightVfx.Stop(); // Stop the visual effect
        //    }
        //}
        //else if (Keyboard.current.dKey.isPressed)
        //{
        //    // Turn right, play right wheel smoke
        //    if (!rightVfx.HasAnySystemAwake())
        //    {
        //        rightVfx.Play(); // Start the visual effect
        //        Debug.Log("Right wheel smoke started");

        //    }

        //    // Stop left wheel smoke
        //    if (leftWheelSmoke != null)
        //    {
        //        //here
        //    }
        //}
        //else // Neither key pressed, stop both wheel smokes
        //{
        //    if (leftWheelSmoke != null)
        //    {
        //        VisualEffect leftVfx = leftWheelSmoke.GetComponent<VisualEffect>();
        //        if (leftVfx != null) leftVfx.Stop(); // Stop the visual effect
        //    }

        //    if (rightWheelSmoke != null)
        //    {
        //        VisualEffect rightVfx = rightWheelSmoke.GetComponent<VisualEffect>();
        //        if (rightVfx != null) rightVfx.Stop(); // Stop the visual effect
        //    }
        //}

        // ------------------------- END OF SMOKE FOR WHEELS --------------------------------------

        // Toggles the visibility of wind trails to on
        //void ShowTrails(string tag)
        //{
        //    GameObject[] trails = GameObject.FindGameObjectsWithTag(tag);
        //    foreach (GameObject trail in trails)
        //    {
        //         trail.SetActive(true);
        //    }
        //}

        //// Toggles the visibility of wind trails to off
        //void HideTrails(string tag)
        //{
        //    GameObject[] trails = GameObject.FindGameObjectsWithTag(tag);
        //    foreach (GameObject trail in trails)
        //    {
        //         trail.SetActive(false);
        //    }
        //}

        // ----------------------------- END OF TRAILS -------------------------------------
    }
    private void FixedUpdate()
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

        //Forumla to synchronize the player's maximum turn speed relative to their current speed.
        maxTurnSpeed = (0.2f * ((37.5f - speed) / 2.5f)) + 0.6f;

        /* JOYSTICK CONTROLS FOR TURNING
        Vector2 leftStick = playerInput.Gameplay.Movement.ReadValue<Vector2>();
        if(Math.Abs(leftStick.x) > 0.05f)
        {

        }
        */

        // Adjusts the turn speed to increase incrementally instead of just 0% -> 100% turn speed.
        if (dynamicTurnBool == true)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.dKey.isPressed)
            {
                if (currentTurnSpeed < maxTurnSpeed)
                {
                    currentTurnSpeed += (maxTurnSpeed/10);
                }
            }
            else
            {
                currentTurnSpeed = 0;
            }

        }

        moveInput = 1f; // 0 = Don't Move & 1 = Move
        float turnInput = Input.GetAxis("Horizontal"); // Left/Right, we can replace this with leftStick.x for joystick

        rb.MovePosition(transform.position + transform.forward * moveInput * speed * Time.deltaTime);

        if (turnInput != 0) //Spherical rotation to simulate steering and not sharp turns.
        {
            Quaternion targetRotation = Quaternion.Euler(0, turnInput * 45, 0) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentTurnSpeed * Time.deltaTime);
        }
    }

    private void Increase(InputAction.CallbackContext context) //Player Input function for increasing speed
    {
        speedDecreaseCooldown = Time.time + 0.15f;

        if (speed < 37.5f)
        {
            speed += 2.5f;
        }

        if(currentTurnSpeed > maxTurnSpeed) //Makes sure the current turn speed does not exceed the max turn speed
        {
            currentTurnSpeed = maxTurnSpeed;
        }    

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

            //if character is moving, play the audio, if not pause the audio
            if (speed == 0)
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
            else
            {

                if (!cartAudio.isPlaying)
                {
                    cartAudio.volume = 0;
                    cartAudio.Play();
                }

                if (cartAudio.volume < 1)
                {

                    //fade in the audio by adjusting the volume over time
                    cartAudio.volume += Time.deltaTime; 

                    if (cartAudio.volume > 1)
                    {

                        //play at full volume
                        cartAudio.volume = 1;

                    }

                }

                //map the pitch of the audio to the speed of the cart
                cartAudio.pitch = Mathf.Lerp(0.7f, 4f, speed / 37.5f);

            }

        }

    }

    //function for step audio
    private void stepAudioEffect()
    {

        if (speed == 0)
        {

            //stop audio when the player is not moving
            if (stepAudio.isPlaying)
            {
                stepAudio.Pause();
            }

        }
        else
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