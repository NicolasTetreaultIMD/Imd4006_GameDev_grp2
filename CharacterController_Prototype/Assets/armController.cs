using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class armController : MonoBehaviour
{
    PlayerInput playerControls;
    public Transform characterArm;
    public Transform cartLeftTilt;
    public Transform cartRightTilt;

    public float maxCartAngle;
    public float maxArmAngle;
    public float cartAngleDeadZone;
    public float armMoveSpeed;

    public bool mouseMode;

    private InputAction grab;
    private Vector2 armInput;
    private float armRotation;

    private float armAngle;
    private float inputRange;

    void Start()
    {
        playerControls = new PlayerInput();
        playerControls.Enable();

        grab = playerControls.Gameplay.ItemGrab;
        grab.Enable();
        grab.performed += Grab;

        armAngle = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //ARM MOVEMENT ----------------------

        if (!mouseMode)
        {
            armInput = playerControls.Gameplay.ArmMovement.ReadValue<Vector2>();
            armAngle += armInput.x * armMoveSpeed;

            armAngle = Mathf.Max(Mathf.Min(armAngle, 90), -90);
            inputRange = 90;
        }
        else
        {
            armInput = playerControls.Gameplay.ArmMovementMouse.ReadValue<Vector2>();
            armInput.x /= Screen.width;
            armInput.y /= Screen.height;

            armInput -= new Vector2(0.5f, 0.5f);

            armInput = new Vector2(Mathf.Max(Mathf.Min(armInput.x, 0.5f), -0.5f), Mathf.Max(Mathf.Min(armInput.y, 0.5f), -0.5f));
            armAngle = armInput.x;
            inputRange = 0.5f;
        }

        armRotation = Map(-inputRange, inputRange, maxArmAngle, -maxArmAngle, armAngle);

        characterArm.localRotation = Quaternion.Euler(0, 0, armRotation);

        //Right Tilt
        if (armAngle > cartAngleDeadZone)
        {
            float cartRotation = Map(cartAngleDeadZone, inputRange, 0, -maxCartAngle, armAngle);

            cartLeftTilt.localRotation = Quaternion.Euler(0, 0, 0);
            cartRightTilt.localRotation = Quaternion.Euler(0, 0, cartRotation);
        }
        //Left Tilt
        else if (armAngle < -cartAngleDeadZone)
        {
            float cartRotation = Map(-inputRange, -cartAngleDeadZone, maxCartAngle, 0, armAngle);

            cartLeftTilt.localRotation = Quaternion.Euler(0, 0, cartRotation);
            cartRightTilt.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            cartLeftTilt.localRotation = Quaternion.Euler(0, 0, 0);
            cartRightTilt.localRotation = Quaternion.Euler(0, 0, 0);
        }

        //ITEM GRAB --------------------------
    }

    public static float Map(float a, float b, float c, float d, float x)
    {
        return c + (x - a) * (d - c) / (b - a);
    } 

    private void Grab(InputAction.CallbackContext context)
    {
        Debug.Log("GRAB");
    }
}
