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

    [Header("Bump Properties")]
    public float jumpStrengthY;
    public float jumpStrengthX;

    private float initYLevel;
    private int BumpDirection;
    private bool isBumping;
    private float elapsedTime;

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

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isBumping)
        {
            elapsedTime += Time.fixedDeltaTime; // Accumulate time each frame
            float positionYAtTime = jumpStrengthY * elapsedTime
                                   + 0.5f * Physics.gravity.y * elapsedTime * elapsedTime;

            transform.position += new Vector3(0, positionYAtTime, 0);

            if (transform.position.y < initYLevel)
            {
                transform.position = new Vector3(transform.position.x, initYLevel, transform.position.z);
                isBumping = false;
                carController.SwitchCartState(CarController.CartState.Running);
                Debug.Log("Done");
            }
        }
    }

    private void Bump (int direction)
    {
        if (!isBumping && carController.cartState == CarController.CartState.Running && cannon.projectile.Count > 0)
        {
            BumpDirection = direction;
            elapsedTime = 0;

            carController.SwitchCartState(CarController.CartState.InAir);
            cannon.projectile.RemoveAt(0);

            isBumping = true;
        }
    }

    private void BumpLeft(InputAction.CallbackContext context)
    {
        Debug.Log("Bump Left");
        Bump(1);
    }

    private void BumpRight(InputAction.CallbackContext context) 
    {
        Debug.Log("Bump Right");
        Bump(-1);
    }
}
