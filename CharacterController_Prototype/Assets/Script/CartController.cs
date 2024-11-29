using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CarController : MonoBehaviour
{
    public enum CartState { Running, InCart, PoleHolding, InAir };
    [Header("Car State")]
    public CartState cartState;

    [Header("Player Input")]
    public int playerId;
    public Vector2 leftStick;
    public float stickDeadzone;
    PlayerInput playerInput;
    private InputAction increase;
    public HapticFeedback haptics;

    [Header("Health")]
    public GameManager gameManager;
    public DamageHandler damageHandler;
    public CharacterHider characterHider;
    public int health;

    [Header("Cannon")]
    public Cannon cannon;
    public bool canPickUpItem;

    [Header("Character")]
    public GameObject[] Characters;
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
    public float speedDecreaseCooldown; //The time in which a speed will decrease
    public float speedDecreaseValue; //How much the speed will decrease by
    public float rotationAmount;

    [Header("Turning")]
    public float minTurn;
    public float currentTurnSpeed;
    public bool turnSpeedToggle;
    public float maxTurnSpeed;
    public bool dynamicTurnBool;
    public float speedForPeakTurn;

    [Header("Pole Rotation")]
    public GrabManager grabManager;
    public Transform cameraPivotRef;
    public Transform poleRotateLookatRef;
    public bool hasBoostGrace;
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
    public Animator animator;
    public ParticleSystem featherEffect;
    public vfxHandler vfxHandler;
    public audioHandler audioHandler;

    

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        health = 3;
        canPickUpItem = true;
        
        FindNeededObjects();
        playerInput = GetComponent<PlayerInput>();
        playerId = playerInput.playerIndex;

        increase = playerInput.actions["SpeedIncrease"]; // Use PlayerInput actions
        increase.performed += Increase;

        speed = 0;
        currentTurnSpeed = 0;
        maxTurnSpeed = 3.6f;
        speedDecreaseValue = 0.5f;

        dynamicTurnBool = true;

        cartState = CartState.Running;
    }

    private void Update()
    {
        if (gameManager.GameOver == false)
        {
            //Syncs the speed of the player's leg animation to the player's actual speed in the game.
            float newLowerBodySpeed = speed / 18.75f;
            animator.SetFloat("LowerBodySpeed", newLowerBodySpeed);

            //Transition for the runner to go to the shooter position
            if (goingToShooterSpot)
            {
                transitionProgress += Time.deltaTime * 0.5f;
                Runner.position = Vector3.Lerp(Runner.position, shooterSpot.position, transitionProgress);

                if (Runner.position == shooterSpot.position)
                {
                    runnerAnimController.ChangeAnimation("Idle", 1);
                    goingToShooterSpot = false;
                }

                if (goingToRunnerSpot) //Safety net if both happen at same time
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
    }

    private void FixedUpdate()
    {
        if (gameManager.GameOver == false)
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
            leftStick = playerInput.actions["Movement"].ReadValue<Vector2>();

            leftStick = StickDeadzone(leftStick);

            if (cartState != CartState.PoleHolding)
            {
                //Gradually decreases the player's speed over time if they aren't clicking the accelerate button.
                if (speed > 0)
                {
                    if (Time.time >= speedDecreaseCooldown && !hasBoostGrace)
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

                //APPLIES TURN TO CART
                TurnCart();

                //MOVES CART
                Move();
            }
            else
            {
                PoleGrabIncrease();
            }

            centerMassManager.massCenter.x += turnTilt - prevTurnTilt;
            prevTurnTilt = turnTilt;

            if (health <= 0)
            {
                characterHider.HideObjects();
            }
        }
    }

    //Makes sure the stickInputs follow the deadzone set
    private Vector2 StickDeadzone (Vector2 stickInput)
    {
        if (stickInput.x < stickDeadzone && stickInput.x > -stickDeadzone)
        {
            stickInput.x = 0;
        }
        if (stickInput.y < stickDeadzone && stickInput.y > -stickDeadzone)
        {
            stickInput.y = 0;
        }

        return stickInput;
    }

    private void PoleGrabIncrease()
    {
        CameraPivot();

        //Gradually increases the player's speed while on the pole

        if (speed < maxSpeed * grabManager.additionalSpeed)
        {
            speed += 2.5f;
        }
        else if (speed >= maxSpeed * grabManager.additionalSpeed)
        {
            speed = maxSpeed * grabManager.additionalSpeed;
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

    //Pivots the camera around the pole when in pole grabbing mode
    private void CameraPivot()
    {
        Vector2 rightStick = playerInput.actions["CannonAim"].ReadValue<Vector2>();
        rightStick = StickDeadzone(rightStick);

        Quaternion targetRotation = Quaternion.Euler(0, rightStick.x * -10, 0) * cameraPivotRef.rotation;

        cameraPivotRef.rotation = Quaternion.Slerp(cameraPivotRef.rotation, targetRotation, cameraPivotSpeed * Time.fixedDeltaTime);
    }

    private void Move()
    {
        moveInput = 1f; // 0 = Don't Move & 1 = Move

        rb.MovePosition(transform.position + transform.forward * moveInput * speed * Time.fixedDeltaTime);

        // Quaternion targetRotation = Quaternion.Euler(0, leftStick.x * 45, 0) * transform.rotation;
        // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, currentTurnSpeed * Time.fixedDeltaTime);

        Quaternion targetRotation = Quaternion.Euler(0, leftStick.x * (minTurn + rotationAmount * (Mathf.Min(1, speed/speedForPeakTurn))) + centerMassManager.turnIncrease, 0) * transform.rotation;
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

    private void Increase(InputAction.CallbackContext context) //Player Input function for increasing speed
    {
        if (damageHandler.isStunned == false)
        {
            if (cartState == CartState.Running)
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

            if (currentTurnSpeed > maxTurnSpeed) // Ensure current turn speed does not exceed the max
            {
                currentTurnSpeed = maxTurnSpeed;
            }
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

            case CartState.InAir:
                break;
        };
    }

    

    public void FindNeededObjects()
    {
        globalVolume = GameObject.Find("Global Volume").GetComponent<Volume>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            if (canPickUpItem == true && cannon.projectile.Count < 2)
            {
                if (other.tag == "Bomb Box")
                {
                    cannon.LoadCannon(GameObject.Find("Bomb Item"));

                    vfxHandler.PickupItem(); // Play Item Pickup VFX
                    audioHandler.PickupItem(); // Play Item Pickup AFX

                    other.GetComponent<ItemRespawn>().ItemPickedUp();
                }

                if (other.tag == "Mine Box")
                {
                    cannon.LoadCannon(GameObject.Find("Mine Item"));

                    vfxHandler.PickupItem(); // Play Item Pickup VFX
                    audioHandler.PickupItem(); // Play Item Pickup AFX

                    other.GetComponent<ItemRespawn>().ItemPickedUp();
                }

                if (other.tag == "Nuke Box")
                {
                    cannon.LoadCannon(GameObject.Find("Nuke Item"));

                    vfxHandler.PickupItem(); // Play Item Pickup VFX
                    audioHandler.PickupItem(); // Play Item Pickup AFX

                    other.GetComponent<ItemRespawn>().ItemPickedUp();
                }

                if (other.tag == "Trap Box")
                {
                    cannon.LoadCannon(GameObject.Find("Trap Item"));

                    vfxHandler.PickupItem(); // Play Item Pickup VFX
                    audioHandler.PickupItem(); // Play Item Pickup AFX

                    other.GetComponent<ItemRespawn>().ItemPickedUp();
                }
            }
            else
            {
                vfxHandler.PickupItem(); // Play Item Pickup VFX
                audioHandler.PickupItem(); // Play Item Pickup AFX

                haptics.CrateHaptics();
                other.GetComponent<ItemRespawn>().ItemPickedUp();
            }
        }
    }
}

