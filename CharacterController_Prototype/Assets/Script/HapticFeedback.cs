using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class HapticFeedback : MonoBehaviour
{
    public CarController controller;
    public float speed;
    public bool isHoldingPole;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        speed = controller.speed;


        if(isHoldingPole)
        {
            Gamepad.current.SetMotorSpeeds(1f, 1f); // Left and Right motor
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

    public void PoleHaptics()
    {
        
    }



    private IEnumerator CannonHapticsCoroutine()
    {
        Debug.Log("YES");
        Gamepad.current.SetMotorSpeeds(1f, 1f); // Left and Right motor
        yield return new WaitForSeconds(0.15f);
        Gamepad.current.SetMotorSpeeds(0.0f, 0.0f); // Left and Right motor
    }

    private IEnumerator CrashHapticsCoroutine()
    {
        Debug.Log("CRASH");
        Gamepad.current.SetMotorSpeeds(1f, 1f);
        yield return new WaitForSeconds(0.5f);
        Gamepad.current.SetMotorSpeeds(0f, 0f);
    }
}
