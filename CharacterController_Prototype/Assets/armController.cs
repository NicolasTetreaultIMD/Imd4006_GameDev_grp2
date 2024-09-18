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
    private InputAction grab;

    void Start()
    {
        playerControls = new PlayerInput();
        playerControls.Enable();

        grab = playerControls.Gameplay.ItemGrab;
        grab.Enable();
        grab.performed += Grab;
    }

    // Update is called once per frame
    void Update()
    {
        //ARM MOVEMENT ----------------------

        Vector2 armInput = playerControls.Gameplay.ArmMovement.ReadValue<Vector2>();
        //armInput.Normalize();
        armInput.x /= Screen.width;
        armInput.y /= Screen.height;

        armInput -= new Vector2(0.5f,0.5f);

        armInput = new Vector2(Mathf.Max(Mathf.Min(armInput.x, 0.3f), -0.3f), Mathf.Max(Mathf.Min(armInput.y, 0.5f), -0.5f));
        //Debug.Log(armInput);

        float armRotation = Map(-0.3f, 0.3f, 90, -90, armInput.x);
        characterArm.rotation = Quaternion.Euler(0, 0, armRotation);

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
