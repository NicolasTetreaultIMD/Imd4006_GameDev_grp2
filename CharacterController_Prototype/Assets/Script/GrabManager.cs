using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;
using UnityEngine.UIElements;

//Controls the arms for the pole grab mechanic
public class GrabManager : MonoBehaviour
{
    PlayerInput playerControls;

    public CarController carController;
    public CinemachineVirtualCamera cinemachine;
    public HapticFeedback haptics;

    [Header("Arms Objects")]
    public GameObject leftArmGrab;      //left arm in grab state
    public GameObject rightArmGrab;     //right arm in grab state

    [Header("Transform References")]
    public Transform lookatRef;         //Transform reference for lookat while pole grabbing
    public Transform stationaryCamRef;  //Stationary cam reference for when grabbing the pole
    public Transform cameraPivotRef;

    private GameObject activeHand;      //Which hand is currently active

    private InputAction leftGrab;       //Left trigger pressed
    private InputAction rightGrab;      //Right trigger presseed

    [Header("Boost Properties")]
    public float gracePeriod;
    public float additionalSpeed;

    [Header("Center Mass")]
    public CenterMassManager centerMassManager;
    public float armMassShift;          //By how much does the arm causes the mass to shift

    [Header("Motion Blur")]
    public Volume globalVolume;
    public float maxMotionBlurIntensity;
    public float motionBlurDuration;
    private float motionBlurChange;
    private float prevMotionBlurChange;
    private bool motionBlurActive;
    private MotionBlur motionBlur;

    private float timeElapsedMB;
    private float timeElapsedG;

    [Header("Audio & VFX")]
    public vfxHandler vfxHandler;
    public audioHandler audioHandler;


    // Input manager setup
    void Start()
    {
        playerControls = GetComponentInParent<PlayerInput>();

        globalVolume = GameObject.Find("Global Volume").GetComponent<Volume>();


        leftGrab = playerControls.actions["PoleGrabLeft"];
        leftGrab.performed += StartLeftPoleGrabInput;
        leftGrab.canceled += ExitLeftPoleGrabInput;

        rightGrab = playerControls.actions["PoleGrabRight"];
        rightGrab.performed += StartRightPoleGrabInput;
        rightGrab.canceled += ExitRightPoleGrabInput;

        armMassShift = Mathf.Min(armMassShift, centerMassManager.maxRotationInput);

        lookatRef.parent = null;
        stationaryCamRef.parent = null;
        cameraPivotRef.parent = null;
    }

    // Update is called once per frame
    void Update()
    {

        if (motionBlurActive)
        {
            timeElapsedMB += Time.deltaTime;

            if (timeElapsedMB > motionBlurDuration)
            {
                motionBlurChange = 0;
                motionBlurActive = false;
            }
            else
            {
                motionBlurChange -= maxMotionBlurIntensity * (Time.deltaTime / motionBlurDuration);
            }
        }

        if (carController.hasBoostGrace)
        {
            timeElapsedG += Time.deltaTime;

            if (timeElapsedG > gracePeriod)
            {
                carController.hasBoostGrace = false;
            }
        }

        //Update MotionBlur
        globalVolume.profile.TryGet(out motionBlur);
        {
            motionBlur.intensity.value += motionBlurChange - prevMotionBlurChange;
            prevMotionBlurChange = motionBlurChange;
        }

        if ((leftArmGrab.activeSelf || rightArmGrab.activeSelf) && carController.cartState == CarController.CartState.Running)
        {
            leftArmGrab.SetActive(false);
            rightArmGrab.SetActive(false);
        }

        //If needed can add code that will run in update if one of the arm is active

        /*if (rightGrab.IsPressed() && activeHand == rightArmGrab)
        {
            //Debug.Log("R");
        }
        if (leftGrab.IsPressed() && activeHand == leftArmGrab)
        {
            //Debug.Log("L");
        }*/
    }

    //If no arm currently active, activate left arm
    private void StartLeftPoleGrab()
    {
        if (activeHand == null && carController.cartState != CarController.CartState.Running)
        {
            //Debug.Log("Start Left");
            leftArmGrab.SetActive(true);

            centerMassManager.massCenter.x -= armMassShift;
            activeHand = leftArmGrab;
        }
    }

    //deactivate left arm; enable other arm if its button is already pressed
    private void ExitLeftPoleGrab()
    {
        if (activeHand == leftArmGrab)
        {
            //Debug.Log("Exit Left");
            leftArmGrab.SetActive(false);

            centerMassManager.massCenter.x += armMassShift;
            activeHand = null;
            if (rightGrab.IsPressed())
            {
                StartRightPoleGrab();
            }

            StopGrabPole();
        }
    }

    //If no arm currently active, activate right arm
    private void StartRightPoleGrab()
    {
        if (activeHand == null && carController.cartState != CarController.CartState.Running)
        {
            //Debug.Log("Right");
            rightArmGrab.SetActive(true);

            centerMassManager.massCenter.x += armMassShift;
            activeHand = rightArmGrab;
        }
    }

    //deactivate right arm; enable other arm if its button is already pressed
    private void ExitRightPoleGrab()
    {
        if (activeHand == rightArmGrab)
        {
            //Debug.Log("Exit Right");
            rightArmGrab.SetActive(false);

            centerMassManager.massCenter.x -= armMassShift;
            activeHand = null;
            if (leftGrab.IsPressed())
            {
                StartLeftPoleGrab();
            }

            StopGrabPole();
        }
    }

    //Inititate the pole holding state
    public void GrabPole(Transform poleRef)
    {
        if (carController.cartState != CarController.CartState.PoleHolding)
        {
            Vector3 handPos = activeHand.GetComponentInChildren<ArmGrabCollider>().gameObject.transform.position;

            //Get the distance from the hand to the pole and move the car by that amount
            //Allows the hand to visualy hold on to the pole
            Vector2 DistToPole = new Vector2(poleRef.position.x, poleRef.position.z) - new Vector2(handPos.x, handPos.z);
            carController.transform.position += new Vector3(DistToPole.x, 0, DistToPole.y);


            lookatRef.gameObject.SetActive(true);
            //Set the lookat ref to the hand trigger
            lookatRef.position = activeHand.GetComponentInChildren<ArmGrabCollider>().gameObject.transform.position;

            cameraPivotRef.position = lookatRef.position;

            cameraPivotRef.rotation = carController.gameObject.transform.rotation;

            //Save the camera position to the position it was when the pole was grabbed
            stationaryCamRef.position = carController.gameObject.transform.position;
            stationaryCamRef.rotation = carController.gameObject.transform.rotation;
            //Camera.main.transform.parent = stationaryCamRef;

            cinemachine.Follow = stationaryCamRef;
            cinemachine.LookAt = stationaryCamRef;

            stationaryCamRef.parent = cameraPivotRef;

            //Switch state to the pole holding
            carController.SwitchCartState(CarController.CartState.PoleHolding);

            haptics.GrabPole();
        }
    }

    //Leave the pole grabbing state
    public void StopGrabPole()
    {
        if (carController.cartState == CarController.CartState.PoleHolding)
        {
            carController.gameObject.transform.position = stationaryCamRef.position;
            carController.gameObject.transform.rotation = stationaryCamRef.rotation;

            stationaryCamRef.parent = null;

            //Reset camera to follow the car
            carController.transform.parent = null;

            cinemachine.Follow = carController.transform;
            cinemachine.LookAt = carController.transform;
            //Camera.main.transform.parent = carController.transform;
            //Camera.main.transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, carController.transform.rotation.eulerAngles.y, Camera.main.transform.rotation.eulerAngles.z);

            vfxHandler.PlayFireTrail(); // VFX - PLAY FIRE TRAIL (1.5 seconds long)
            audioHandler.poleRelease(); // AFX - Play tire squeal upon release


            //Start Motion Blur
            timeElapsedMB = 0;
            motionBlurActive = true;
            motionBlurChange = maxMotionBlurIntensity;

            lookatRef.gameObject.SetActive(false);
            carController.SwitchCartState(CarController.CartState.InCart);

            carController.hasBoostGrace = true;
            timeElapsedG = 0;

            haptics.ReleasePole();
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
