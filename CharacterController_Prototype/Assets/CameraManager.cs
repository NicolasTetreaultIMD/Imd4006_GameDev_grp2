using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CarController carController;
    public float maxCamDistance;
    public float cameraShakeAmount;

    private Vector3 initLocalPos;
    private float currentDistance;
    private float targetPos;

    // Start is called before the first frame update
    void Start()
    {
        initLocalPos = transform.localPosition;
        currentDistance = 0;
    }

    void FixedUpdate()
    {
        //Zoom out based on speed
        if (carController.cartState == CarController.CartState.PoleHolding)
        {
            targetPos = Mathf.Lerp(currentDistance, (carController.speed / carController.maxSpeed) * maxCamDistance * 2, 1f * Time.fixedDeltaTime);
        }
        else
        {
            targetPos = Mathf.Lerp(currentDistance, (carController.speed / carController.maxSpeed) * maxCamDistance, 2f * Time.fixedDeltaTime);
        }
        currentDistance = targetPos;

        //Camera Shake based on speed
        Vector3 randomPoint = Random.insideUnitSphere * cameraShakeAmount * carController.speed;

        transform.localPosition = new Vector3(initLocalPos.x + randomPoint.x, initLocalPos.y + randomPoint.y, initLocalPos.z - targetPos);
    }
}
