 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarBump : MonoBehaviour
{
    PlayerInput playerInput;
    private InputAction leftBumpInput;
    private InputAction rightBumpInput;

    public Cannon cannon;
    public CarController carController;
    public CenterMassManager centerMassManager;

    [Header("Bump Properties")]
    public float jumpStrengthY;
    public float jumpStrengthX;
    public float bumpSpeed;
    public bool invertBump;

    private float initYLevel;
    private int BumpDirection;
    private bool isBumping;
    private float elapsedTime;
    private float prevMassChange;


    public audioHandler audioHandler;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        leftBumpInput = playerInput.actions["BumpLeft"]; // Use PlayerInput actions
        leftBumpInput.performed += BumpLeft;

        rightBumpInput = playerInput.actions["BumpRight"]; // Use PlayerInput actions
        rightBumpInput.performed += BumpRight;

        initYLevel = gameObject.transform.position.y;
    }

    private void Update()
    {
        if (Input.GetKeyDown("b"))
        {
            invertBump = !invertBump;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isBumping)
        {
            elapsedTime += Time.fixedDeltaTime * bumpSpeed; // Accumulate time each frame
            float positionYAtTime = jumpStrengthY * elapsedTime
                                   + 0.5f * Physics.gravity.y * elapsedTime * elapsedTime;
            float positionXAtTime = jumpStrengthX * elapsedTime * BumpDirection;

            transform.position += positionXAtTime * transform.right + positionYAtTime * transform.up + transform.forward * carController.moveInput * carController.speed * Time.fixedDeltaTime;

            centerMassManager.massCenter.x += 100 * BumpDirection - prevMassChange;
            prevMassChange = 100 * BumpDirection;

            if (transform.position.y < initYLevel)
            {
                transform.position = new Vector3(transform.position.x, initYLevel, transform.position.z);
                isBumping = false;
                carController.SwitchCartState(CarController.CartState.InCart);
                centerMassManager.overideTiltScale = false;
                centerMassManager.massCenter.x -= prevMassChange;
            }
        }
    }

    private void Bump (int direction)
    {
        

        if (!isBumping && cannon.projectile.Count > 0 && carController.cartState == CarController.CartState.InCart)
        {
            BumpDirection = direction;
            elapsedTime = 0;
            prevMassChange = 0;
            centerMassManager.overideTiltScale = true;

            carController.SwitchCartState(CarController.CartState.InAir);
            cannon.projectile.RemoveAt(0);

            audioHandler.carBump();

            // VFX for bump
            if (direction == 1) { carController.vfxHandler.BumpLeft(); }
            if (direction == -1) { carController.vfxHandler.BumpRight(); }

            isBumping = true;
        }
    }

    private void BumpLeft(InputAction.CallbackContext context)
    {
        if (invertBump)
        {
            Bump(-1);
        }
        else
        {
            Bump(1);
        }
    }

    private void BumpRight(InputAction.CallbackContext context) 
    {
        if (invertBump)
        {
            Bump(1);
        }
        else
        {
            Bump(-1);
        }
    }
}
