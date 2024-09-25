using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class armController : MonoBehaviour
{
    PlayerInput playerControls;
    List<GameObject> grabableItems = new List<GameObject>();
    List<GameObject> grabablePoles = new List<GameObject>();

    public CarController carController;

    public Transform characterArm;
    public Transform cartLeftTilt;
    public Transform cartRightTilt;
    public Transform handTransform;
    public Transform lookatRef;
    public Transform stationaryCamRef;

    public float maxCartAngle;
    public float maxArmAngle;
    public float cartAngleDeadZone;
    public float armMoveSpeed;

    public bool mouseMode;
    public bool stationaryCam;

    private InputAction grab;
    private float poleHoldValue;

    private Vector2 armInput;
    private float armRotation;

    private float armAngle;
    private float inputRange;

    public ParticleSystem[] smokeEffect;

    //Audio

    public AudioSource pickupAudio;

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
            armAngle += armInput.x * armMoveSpeed * Time.fixedDeltaTime * 20;

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

        characterArm.localRotation = Quaternion.Euler(35.874f, 0, armRotation);

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

        //POLE GRAB -----------

        poleHoldValue = playerControls.Gameplay.PoleGrab.ReadValue<float>(); 

        if (poleHoldValue > 0.6f && carController.cartState != CarController.CartState.PoleHolding)
        {
            PoleHold();
        }
        else if (poleHoldValue < 0.6f && carController.cartState == CarController.CartState.PoleHolding)
        {
            carController.transform.parent = null;
            if (stationaryCam)
            {
                Camera.main.transform.parent = carController.transform;
                Camera.main.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, carController.transform.rotation.eulerAngles.y, Camera.main.transform.rotation.eulerAngles.z);
            }

            lookatRef.gameObject.SetActive(false);
            carController.SwitchCartState(CarController.CartState.InCart);

            ToggleSmoke(false); // Toggle smoke effects off

        }
    }

    

    public static float Map(float a, float b, float c, float d, float x)
    {
        return c + (x - a) * (d - c) / (b - a);
    }

    //ITEM GRAB --------------------------
    private void Grab(InputAction.CallbackContext context)
    {
        Debug.Log("GRAB");
        int itemCount = grabableItems.Count - 1;
        for (int i = itemCount; i > -1; i--)
        {
            GameObject temp = grabableItems[i];
            grabableItems.Remove(temp);
            Destroy(temp);
        }
    }

    public void AddGrab(GameObject newItem)
    {
        if (newItem.layer == LayerMask.NameToLayer("Item"))
        {
            Debug.Log("item");
            grabableItems.Add(newItem);
            pickupAudio.Play();

        }
        else if (newItem.layer == LayerMask.NameToLayer("Pole"))
        {
            Debug.Log("pole in");
            grabablePoles.Add(newItem);
        }
    }
    public void RemoveGrab(GameObject oldItem)
    {
        if (oldItem.layer == LayerMask.NameToLayer("Item"))
        {
            grabableItems.Remove(oldItem);
        }
        else if (oldItem.layer == LayerMask.NameToLayer("Pole"))
        {
            Debug.Log("pole out");
            grabablePoles.Remove(oldItem);
        }
    }

    private void PoleHold()
    {
        if (grabablePoles.Count > 0)
        {
            lookatRef.gameObject.SetActive(true);
            lookatRef.position = handTransform.position;

            if (stationaryCam)
            {
                stationaryCamRef.position = carController.gameObject.transform.position;
                stationaryCamRef.rotation = carController.gameObject.transform.rotation;
                Camera.main.transform.parent = stationaryCamRef;
            }

            carController.SwitchCartState(CarController.CartState.PoleHolding);

            ToggleSmoke(true); // Toggle smoke effects on

        }
    }

    // TOGGLE SMOKE EFFECT
    private void ToggleSmoke(bool hold)
    {
        if (hold) // If player is holding the pole
        {
            for (int i = 0; i < smokeEffect.Length; i++)
            {
                smokeEffect[i].Play(); // Play particle effect
            }
        }
        else // Player released
        {
            for (int i = 0; i < smokeEffect.Length; i++)
            {
                smokeEffect[i].Stop();
            }
        }

    }
}
