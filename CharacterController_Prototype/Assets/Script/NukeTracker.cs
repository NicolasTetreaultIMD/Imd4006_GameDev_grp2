using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NukeTracker : MonoBehaviour
{
    public GameObject target;
    public GameObject newTarget;
    public Projectile projectileScript;

    public int playerId;
    public bool foundPlayer;

    // Speed at which the object will move
    public float speed;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        foundPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (foundPlayer)
        {
            if (target.gameObject.GetComponent<CarController>().playerId != playerId)
            {
                projectileScript.madeContact = true;
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
                transform.LookAt(target.transform.position);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (projectileScript != null)
        {
            if (projectileScript.carController != null)
            {
                playerId = projectileScript.carController.playerId;
            }
        }

        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("Sender PlayerId: " + playerId + " Target PlayerId: " + other.gameObject.GetComponent<CarController>().playerId);
            if (other.gameObject.GetComponent<CarController>().playerId != playerId)
            {
                target = other.gameObject;
                foundPlayer = true;
                StartCoroutine(FadeOut());
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<CarController>().playerId != playerId)
            {
                projectileScript.explosion.SetActive(true);
                projectileScript.nukeMesh.SetActive(false);
                StopCoroutine(FadeOut());
                StartCoroutine(QuickFadeout());
            }
        }
    }

    private IEnumerator FadeOut()
    {
        // Wait for 2 seconds before continuing
        yield return new WaitForSeconds(5f);
        projectileScript.explosion.SetActive(true);
        projectileScript.nukeMesh.SetActive(false);
        yield return new WaitForSeconds(0.75f);
        Destroy(transform.root.gameObject);
    }

    private IEnumerator QuickFadeout()
    {
        // Wait for 2 seconds before continuing
        yield return new WaitForSeconds(0.75f);
        Destroy(transform.root.gameObject);
    }
}
