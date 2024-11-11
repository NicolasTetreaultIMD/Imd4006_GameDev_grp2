using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CarController carController;
    public CinemachineVirtualCamera cinemachine;

    [Header("Cam Properties")]
    public float minFOV;
    public float maxFOX;
    public float FOVChangeSpeed;
    public float maxCamDistance;
    public float cameraShakeAmount;

    [Header("Cam Position")]
    public Vector3 camDisplacement;

    private Vector3 initLocalPos;
    private float currentDistance;
    private float targetPos;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
        cinemachine.transform.parent = null;

        initLocalPos = transform.localPosition;
        currentDistance = 0;
    }

    void Update()
    {
        //Zoom out based on speed
        if (carController.cartState == CarController.CartState.PoleHolding)
        {
            cinemachine.m_Lens.FieldOfView = Mathf.Lerp(cinemachine.m_Lens.FieldOfView, Mathf.Max((carController.speed / carController.maxSpeed) * maxFOX * 1.5f, minFOV), FOVChangeSpeed * Time.deltaTime);
            //targetPos = Mathf.Lerp(currentDistance, (carController.speed / carController.maxSpeed) * maxCamDistance * 2, 1f * Time.deltaTime);
        }
        else
        {
            cinemachine.m_Lens.FieldOfView = Mathf.Lerp(cinemachine.m_Lens.FieldOfView, Mathf.Max((carController.speed / carController.maxSpeed) * maxFOX, minFOV), FOVChangeSpeed * Time.deltaTime);
            Debug.Log(cinemachine.m_Lens.FieldOfView);
            //targetPos = Mathf.Lerp(currentDistance, (carController.speed / carController.maxSpeed) * maxCamDistance, 8f * Time.deltaTime);
        }

    }
}
