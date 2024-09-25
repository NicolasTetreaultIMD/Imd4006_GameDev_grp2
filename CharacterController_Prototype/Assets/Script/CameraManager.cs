using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CarController carController;
    public float maxCamDistance;
    public float cameraShakeAmount;

    public Vector3 camDisplacement;

    private Vector3 initLocalPos;
    private float currentDistance;
    private float targetPos;

    // Start is called before the first frame update
    void Start()
    {
        initLocalPos = transform.localPosition;
        currentDistance = 0;
    }

    void Update()
    {
        //Zoom out based on speed
        if (carController.cartState == CarController.CartState.PoleHolding)
        {
            targetPos = Mathf.Lerp(currentDistance, (carController.speed / carController.maxSpeed) * maxCamDistance * 2, 1f * Time.deltaTime);
        }
        else
        {
            targetPos = Mathf.Lerp(currentDistance, (carController.speed / carController.maxSpeed) * maxCamDistance, 8f * Time.deltaTime);
        }
        currentDistance = targetPos;
        camDisplacement.z -= targetPos;

        //Camera Shake based on speed
        Vector3 randomPoint = Random.insideUnitSphere * cameraShakeAmount * carController.speed;

        camDisplacement.x += randomPoint.x;
        camDisplacement.y += randomPoint.y;

        //transform.localPosition = new Vector3(initLocalPos.x + randomPoint.x, initLocalPos.y + randomPoint.y, initLocalPos.z - targetPos);
    }

    private void LateUpdate()
    {
        transform.localPosition = new Vector3(initLocalPos.x + camDisplacement.x, initLocalPos.y + camDisplacement.y, initLocalPos.z + camDisplacement.z);
        camDisplacement = new Vector3();
    }
}
