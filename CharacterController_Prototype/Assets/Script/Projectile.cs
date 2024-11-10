using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float totalTime = 0f;
    public Transform shootingPoint;
    public Vector3 direction;
    public float shootForce;
    public float mass;

    public bool forcesApplied;
    public bool madeContact;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Awake()
    {
        forcesApplied = false;
        madeContact = false;
        mass = gameObject.GetComponent<Rigidbody>().mass;
    }

    // Update is called once per frame
    void Update()
    {
        if (forcesApplied == true && madeContact == false)
        {
            totalTime += Time.deltaTime * 2; // Accumulate time each frame
            Vector3 positionAtTime = shootingPoint.position
                                   + direction * (shootForce / mass) * totalTime
                                   + 0.5f * Physics.gravity * totalTime * totalTime;

            transform.position = positionAtTime;
        }
    }

    public void applyProperties(Transform newShootingPoint, Vector3 newDirection, float newShootForce)
    {
        shootingPoint = newShootingPoint;
        direction = newDirection;
        shootForce = newShootForce;
        
        forcesApplied = true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 0 || collision.gameObject.layer == 7)
        {
            madeContact = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            explosion.gameObject.SetActive(true);
        }
    }
}
