using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CarCrashManager : MonoBehaviour
{
    public float minCarCrashSpeed;
    public CarController carController;
    public obstacleCollisionManager collisionManager;

    public bool coroutineOn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (carController.speed >= minCarCrashSpeed && !coroutineOn)
        {
            //Debug.Log(carController.speed); 
            if (other.GetComponent<obstacleCollisionManager>() != null)
            {
                DamageHandler otherDamageHandler = other.GetComponent<obstacleCollisionManager>().carController.gameObject.GetComponent<DamageHandler>();
                StartCoroutine(SendDamage(otherDamageHandler));
            }
        }
    }

    private IEnumerator SendDamage(DamageHandler otherDamageHandler)
    {
        coroutineOn = true;
        yield return new WaitForFixedUpdate();

        carController.speed = Mathf.Max(0, carController.speed - 10);
        carController.SwitchCartState(CarController.CartState.Running);

        collisionManager.cinemachine.m_Lens.FieldOfView = collisionManager.FOVImpact;
        collisionManager.cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = collisionManager.camManager.amplitudeChange + collisionManager.camManager.maxAmplitude;
        collisionManager.cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = collisionManager.camManager.frequencyChange + collisionManager.camManager.maxFrequency;

        otherDamageHandler.GetComponent<Rigidbody>().AddForce(transform.forward * 4000 * (carController.speed / carController.maxSpeed));
        otherDamageHandler.Hit(carController.playerId);
        coroutineOn = false;
    }
}
