using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class DamageApplier : MonoBehaviour
{
    public int playerId;
    public bool isReady;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if (gameObject.transform.root.tag == "Nuke")
        {
            playerId = gameObject.GetComponentInParent<NukeTracker>().playerId;
        }
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

            if (other.gameObject.GetComponent<DamageHandler>() != null)
            {
                other.gameObject.GetComponent<DamageHandler>().Hit(-1); //A parameter of -1 means that even the player who threw it can get damaged by it
                StartCoroutine(FadeOut());
            }

        }

        if (transform.root.tag == "Nuke")
        {
            if (other.gameObject.GetComponentInParent<DamageHandler>() != null)
            {
                if (other.GetComponentInParent<CarController>().playerId != playerId)
                {
                    other.gameObject.GetComponentInParent<DamageHandler>().Hit(playerId);
                    other.gameObject.GetComponentInParent<CarController>().audioHandler.impactExplosion();
                }
            }
        }

        if (transform.root.tag == "Trap")
        {

            if (other.gameObject.GetComponent<DamageHandler>() != null)
            {
                other.gameObject.GetComponent<DamageHandler>().Stun(-1);
                StartCoroutine(TrapFadeOut());
            }

        }

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
