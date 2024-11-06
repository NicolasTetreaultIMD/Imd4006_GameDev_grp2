using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabManager : MonoBehaviour
{
    PlayerInput playerControls;

    public CarController carController;
    public vfxHandler vfxHandler;

    public GameObject leftArmDefault;
    public GameObject leftArmGrab;
    public GameObject rightArmDefault;
    public GameObject rightArmGrab;

    public Transform lookatRef;
    public Transform stationaryCamRef;

    private GameObject activeHand;

    private InputAction leftGrab;
    private InputAction rightGrab;


    // Start is called before the first frame update
    void Start()
    {
        playerControls = new PlayerInput();
        playerControls.Enable();

        leftGrab = playerControls.Gameplay.PoleGrabLeft;
        leftGrab.Enable();
        leftGrab.performed += StartLeftPoleGrabInput;
        leftGrab.canceled += ExitLeftPoleGrabInput;

        rightGrab = playerControls.Gameplay.PoleGrabRight;
        rightGrab.Enable();
        rightGrab.performed += StartRightPoleGrabInput;
        rightGrab.canceled += ExitRightPoleGrabInput;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (rightGrab.IsPressed() && activeHand == rightArmGrab)
        {
            //Debug.Log("R");
        }
        if (leftGrab.IsPressed() && activeHand == leftArmGrab)
        {
            //Debug.Log("L");
        }*/
    }

    private void StartLeftPoleGrab()
    {
        if (activeHand == null)
        {
            //Debug.Log("Start Left");
            leftArmDefault.SetActive(false);
            leftArmGrab.SetActive(true);

            activeHand = leftArmGrab;
        }
    }

    private void ExitLeftPoleGrab()
    {
        //Debug.Log("Exit Left");
        leftArmDefault.SetActive(true);
        leftArmGrab.SetActive(false);

        activeHand = null;
        if (rightGrab.IsPressed())
        {
            StartRightPoleGrab();
        }

        StopGrabPole();
    }

    private void StartRightPoleGrab()
    {
        if (activeHand == null)
        {
            //Debug.Log("Right");
            rightArmDefault.SetActive(false);
            rightArmGrab.SetActive(true);

            activeHand = rightArmGrab;
        }
    }

    private void ExitRightPoleGrab()
    {
        //Debug.Log("Exit Right");
        rightArmDefault.SetActive(true);
        rightArmGrab.SetActive(false);

        activeHand = null;
        if (leftGrab.IsPressed())
        {
            StartLeftPoleGrab();
        }

        StopGrabPole();
    }

    public void GrabPole(Transform poleRef)
    {
        if (carController.cartState != CarController.CartState.PoleHolding)
        {
            Vector3 handPos = activeHand.GetComponentInChildren<ArmGrabCollider>().gameObject.transform.position;

            Vector2 DistToPole = new Vector2(poleRef.position.x, poleRef.position.z) - new Vector2(handPos.x, handPos.z);
            carController.transform.position += new Vector3(DistToPole.x, 0, DistToPole.y);


            lookatRef.gameObject.SetActive(true);
            lookatRef.position = activeHand.GetComponentInChildren<ArmGrabCollider>().gameObject.transform.position;

            stationaryCamRef.position = carController.gameObject.transform.position;
            stationaryCamRef.rotation = carController.gameObject.transform.rotation;
            Camera.main.transform.parent = stationaryCamRef;

            carController.SwitchCartState(CarController.CartState.PoleHolding);
        }
    }

    private void StopGrabPole()
    {
        if (carController.cartState == CarController.CartState.PoleHolding)
        {
            carController.transform.parent = null;
            Camera.main.transform.parent = carController.transform;
            Camera.main.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, carController.transform.rotation.eulerAngles.y, Camera.main.transform.rotation.eulerAngles.z);

            vfxHandler.PlayFireTrail(); // PLAY FIRE TRAIL (1.5 seconds long)

            lookatRef.gameObject.SetActive(false);
            carController.SwitchCartState(CarController.CartState.InCart);
        }
    }





    private void StartLeftPoleGrabInput(InputAction.CallbackContext context)
    {
        StartLeftPoleGrab();
    }
    private void ExitLeftPoleGrabInput(InputAction.CallbackContext context)
    {
        ExitLeftPoleGrab();
    }
    private void StartRightPoleGrabInput(InputAction.CallbackContext context)
    {
        StartRightPoleGrab();
    }
    private void ExitRightPoleGrabInput(InputAction.CallbackContext context)
    {
        ExitRightPoleGrab();
    }
}
