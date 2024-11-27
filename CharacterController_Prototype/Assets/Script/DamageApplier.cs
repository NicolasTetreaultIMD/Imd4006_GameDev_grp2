using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class DamageApplier : MonoBehaviour
{
    public int playerId;
    bool isReady;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        //isReady = false;
        StartCoroutine(LaunchPeriod());
    }

    private void OnTriggerEnter(Collider other)
    {

        if (transform.root.tag == "Bomb")
        {
            if (other.gameObject.GetComponent<DamageHandler>() != null)
            {
                other.gameObject.GetComponent<DamageHandler>().Hit(playerId);

            }
        }

        if (transform.root.tag == "Mine")
        {
            Debug.Log("Hooray");
            if (other.gameObject.GetComponent<DamageHandler>() != null)
            {
                other.gameObject.GetComponent<DamageHandler>().Hit(-1); //A parameter of -1 means that even the player who threw it can get damaged by it
                StartCoroutine(FadeOut());
            }
        }

        if (transform.root.tag == "Nuke")
        {
            if (other.gameObject.GetComponent<DamageHandler>() != null)
            {
                other.gameObject.GetComponent<DamageHandler>().Hit(playerId);
                other.gameObject.GetComponent<CarController>().audioHandler.impactExplosion();

            }
        }

        if (transform.root.tag == "Trap")
        {
            if (isReady)
            {
                if (other.gameObject.GetComponent<DamageHandler>() != null)
                {
                    Debug.Log("Bye bye trap");
                    other.gameObject.GetComponent<DamageHandler>().Stun(-1);
                    StartCoroutine(TrapFadeOut());
                }
            }
        }


    }

    private IEnumerator LaunchPeriod()
    {
        yield return new WaitForSeconds(0.5f);
        isReady = true;
    }

    private IEnumerator FadeOut()
    {
        // Wait for 2 seconds before continuing
        yield return new WaitForSeconds(0.75f);
        Destroy(transform.root.gameObject);
    }

    private IEnumerator TrapFadeOut()
    {
        // Wait for 2 seconds before continuing
        yield return new WaitForSeconds(3f);
        Destroy(transform.root.gameObject);
    }

}
