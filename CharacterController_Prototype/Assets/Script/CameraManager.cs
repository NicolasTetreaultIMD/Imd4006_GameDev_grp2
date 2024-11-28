using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    public CarController carController;
    public PlayerInput playerInput;
    public CinemachineVirtualCamera cinemachine;
    public CinemachineVirtualCamera backCam;

    [Header("Cam Properties")]
    public float minFOV;
    public float maxFOX;
    public float FOVChangeSpeed;
    public float maxCamDistance;
    public float cameraShakeAmount;

    [Header("Camera Shake Properties")]
    public float amplitudeChange;
    public float amplitudeChangeSpeed;
    public float maxAmplitude;
    public float frequencyChange;
    public float frequencyChangeSpeed;
    public float maxFrequency;

    private float prevAmplitude;
    private float prevFrequency;

    [Header("Cam Position")]
    public Vector3 camDisplacement;

    private InputAction backCamAction;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;

        backCamAction = playerInput.actions["LookBack"];
        backCamAction.Enable();

        backCamAction.performed += ctx =>
        {
            backCam.Priority = 11;
            cinemachine.Priority = 1;
        };

        backCamAction.canceled += ctx =>
        {
            cinemachine.Priority = 11;
            backCam.Priority = 1;
        };
    }

    void Update()
    {
        //Zoom out based on speed
        if (carController.cartState == CarController.CartState.PoleHolding)
        {
            cinemachine.m_Lens.FieldOfView = Mathf.Lerp(cinemachine.m_Lens.FieldOfView, Mathf.Max((carController.speed / carController.maxSpeed) * maxFOX * 1.5f, minFOV), FOVChangeSpeed * Time.deltaTime);
            backCam.m_Lens.FieldOfView = cinemachine.m_Lens.FieldOfView;
            //targetPos = Mathf.Lerp(currentDistance, (carController.speed / carController.maxSpeed) * maxCamDistance * 2, 1f * Time.deltaTime);
        }
        else
        {
            cinemachine.m_Lens.FieldOfView = Mathf.Lerp(cinemachine.m_Lens.FieldOfView, Mathf.Max((carController.speed / carController.maxSpeed) * maxFOX, minFOV), FOVChangeSpeed * Time.deltaTime);
            //targetPos = Mathf.Lerp(currentDistance, (carController.speed / carController.maxSpeed) * maxCamDistance, 8f * Time.deltaTime);
            backCam.m_Lens.FieldOfView = cinemachine.m_Lens.FieldOfView;
        }

        float newAmp = (carController.speed / carController.maxSpeed) * (maxAmplitude * 0.7f);
        amplitudeChange += newAmp - prevAmplitude;
        prevAmplitude = newAmp;

        float newFreq = (carController.speed / carController.maxSpeed) * (maxFrequency * 0.5f);
        frequencyChange += newFreq - prevFrequency;
        prevFrequency = newFreq;

        cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = Mathf.Lerp(cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain, Mathf.Min(amplitudeChange, maxAmplitude), amplitudeChangeSpeed * Time.deltaTime);
        cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = Mathf.Lerp(cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain, Mathf.Min(frequencyChange, maxFrequency), frequencyChangeSpeed * Time.deltaTime);

        backCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain;
        backCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain;
    }
}
