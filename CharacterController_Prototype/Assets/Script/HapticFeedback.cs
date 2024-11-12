using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class HapticFeedback : MonoBehaviour
{
    public CarController controller;
    public float speed;
    public bool isHoldingPole;
    public float rumble;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        speed = controller.speed;


        if(isHoldingPole)
        {
            rumble = (speed - controller.minInCartSpeed) / (controller.maxSpeed - controller.minInCartSpeed) * (0.6f - 0);
            Gamepad.current.SetMotorSpeeds(rumble, rumble);
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

    private IEnumerator CannonHapticsCoroutine()
    {
        Gamepad.current.SetMotorSpeeds(1f, 1f); // Left and Right motor
        yield return new WaitForSeconds(0.15f);
        Gamepad.current.SetMotorSpeeds(0.0f, 0.0f); // Left and Right motor
    }

    private IEnumerator CrashHapticsCoroutine()
    {
        Gamepad.current.SetMotorSpeeds(1f, 1f);
        yield return new WaitForSeconds(0.5f);
        Gamepad.current.SetMotorSpeeds(0f, 0f);
    }
    private IEnumerator ReleasePoleCoroutine()
    {
        Gamepad.current.SetMotorSpeeds(1f, 1f); // Left and Right motor
        yield return new WaitForSeconds(0.5f);
        Gamepad.current.SetMotorSpeeds(0.0f, 0.0f); // Left and Right motor
    }

    private IEnumerator ExplosionHapticsCoroutine()
    {
        Gamepad.current.SetMotorSpeeds(1f, 1f); // Left and Right motor
        yield return new WaitForSeconds(0.5f);
        Gamepad.current.SetMotorSpeeds(0.0f, 0.0f); // Left and Right motor
    }
}
