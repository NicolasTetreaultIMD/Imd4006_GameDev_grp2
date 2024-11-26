using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCrashManager : MonoBehaviour
{
    public float minCarCrashSpeed;
    public CarController carController;

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
        otherDamageHandler.Hit(carController.playerId);
        coroutineOn = false;
    }
}
