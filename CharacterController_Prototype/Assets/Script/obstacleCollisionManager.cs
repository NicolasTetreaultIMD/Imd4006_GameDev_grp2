using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleCollisionManager : MonoBehaviour
{
    public CarController carController;

    [Header("Camera Shake Properties")]
    public float cameraShakeDuration;
    public float cameraShakeStrength;

    [Header("Collision Properties")]
    public float minHitSpeed;

    private bool hit = false;
    private float timeElapsed;

    private float impactSpeed;
    private Camera mainCam;

    [Header("Audio & VFX")]
    public audioHandler audioHandler;

    private void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (hit)
        {
            if (timeElapsed < cameraShakeDuration)
            {
                Vector3 randomPoint = Random.insideUnitSphere * cameraShakeStrength * impactSpeed / timeElapsed;

                mainCam.GetComponent<CameraManager>().camDisplacement.x += randomPoint.x;
                mainCam.GetComponent<CameraManager>().camDisplacement.y += randomPoint.y;
                //Camera.main.transform.localPosition = new Vector3(initLocalCamPos.x + randomPoint.x, initLocalCamPos.y + randomPoint.y, initLocalCamPos.z);

                timeElapsed += Time.deltaTime;
                //Debug.Log(timeElapsed);
            }
            else
            {
                //Debug.Log("end hit");
                hit = false;
            }
            audioHandler.cartCrash();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("BANG");
        if (carController.speed > minHitSpeed)
        {
            impactSpeed = carController.speed;
            carController.speed = 0;

            timeElapsed = 0;
            hit = true;
        }
    }
}
