using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class HapticFeedback : MonoBehaviour
{
    public PlayerInput playerInput;
    public int playerId;
    public CarController controller;
    public float speed;
    public bool isHoldingPole;
    public float rumble;

    public int myDeviceId;
    public Gamepad myGamepad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        playerId = playerInput.playerIndex;

        myDeviceId = playerInput.devices[0].deviceId;

        foreach(Gamepad g in Gamepad.all)
        {
            if(myDeviceId == g.deviceId) 
            { 
                myGamepad = g;
            }
        }
    }

    void Update()
    {
        speed = controller.speed;

        if(isHoldingPole)
        {
            rumble = (speed - controller.minInCartSpeed) / (controller.maxSpeed - controller.minInCartSpeed) * (0.6f - 0);
            myGamepad.SetMotorSpeeds(rumble, rumble);
        }


    }

    public void CannonHaptics()
    {
        StartCoroutine(CannonHapticsCoroutine());
    }

    public void CrashHaptics()
    {
        StartCoroutine(CrashHapticsCoroutine());
    }

    public void GrabPole()
    {
        isHoldingPole = true;
    }

    public void ReleasePole()
    {
        isHoldingPole = false;
        StartCoroutine(ReleasePoleCoroutine());
    }

    public void ExplosionHaptics()
    {
        StartCoroutine(ExplosionHapticsCoroutine());
    }

    public void CrateHaptics()
    {
        StartCoroutine(CrateHapticsCoroutine());
    }

    private IEnumerator CannonHapticsCoroutine()
    {
        myGamepad.SetMotorSpeeds(1f, 1f); // Left and Right motor
        yield return new WaitForSeconds(0.15f);
        myGamepad.SetMotorSpeeds(0.0f, 0.0f); // Left and Right motor
    }

    private IEnumerator CrashHapticsCoroutine()
    {
        myGamepad.SetMotorSpeeds(1f, 1f);
        yield return new WaitForSeconds(0.5f);
        myGamepad.SetMotorSpeeds(0f, 0f);
    }
    private IEnumerator ReleasePoleCoroutine()
    {
        myGamepad.SetMotorSpeeds(1f, 1f); // Left and Right motor
        yield return new WaitForSeconds(0.5f);
        myGamepad.SetMotorSpeeds(0.0f, 0.0f); // Left and Right motor
    }

    private IEnumerator ExplosionHapticsCoroutine()
    {
        myGamepad.SetMotorSpeeds(1f, 1f); // Left and Right motor
        yield return new WaitForSeconds(0.5f);
        myGamepad.SetMotorSpeeds(0.0f, 0.0f); // Left and Right motor
    }

    private IEnumerator CrateHapticsCoroutine()
    {
        myGamepad.SetMotorSpeeds(0.75f, 0.75f); // Left and Right motor
        yield return new WaitForSeconds(0.25f);
        myGamepad.SetMotorSpeeds(0.0f, 0.0f); // Left and Right motor
    }
}
