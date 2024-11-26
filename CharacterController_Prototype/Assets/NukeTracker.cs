using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeTracker : MonoBehaviour
{
    public Transform target;
    public Transform newTarget;
    public Projectile projectileScript;

    public int playerId;

    // Speed at which the object will move
    public float speed = 350f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null) 
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(other.gameObject.GetComponent<CarController>().playerId != playerId)
            {
                Debug.Log("Yoo");
                target = other.gameObject.transform;
                projectileScript.madeContact = true;
            }
        }    
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            projectileScript.explosion.SetActive(true);
        }
    }
}
