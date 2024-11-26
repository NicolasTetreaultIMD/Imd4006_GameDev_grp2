using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageApplier : MonoBehaviour
{
    public int playerId;

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
            if (other.gameObject.GetComponent<DamageHandler>() != null)
            {
                other.gameObject.GetComponent<DamageHandler>().Hit(playerId); //A parameter of -1 means that even the player who threw it can get damaged by it
                StartCoroutine(FadeOut());
            }
        }
    }

    private IEnumerator FadeOut()
    {
        // Wait for 2 seconds before continuing
        yield return new WaitForSeconds(0.75f);
        Destroy(transform.root.gameObject);
    }
}
