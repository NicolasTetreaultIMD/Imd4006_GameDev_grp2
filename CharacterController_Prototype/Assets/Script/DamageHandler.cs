using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public Material testMaterial;
    public Material currentMaterial;

    public CarController carController;
    public int playerId;

    public uiHandler uiHandler;

    public bool isProjectile;
    public bool isImmune;
    public bool isStunned;

    // Start is called before the first frame update
    void Start()
    {
        uiHandler = GameObject.Find("Canvas").GetComponent<uiHandler>();
        carController = GetComponent<CarController>();
        isStunned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (carController != null)
        {
            playerId = carController.playerId;
        }

        if(isStunned)
        {
            carController.speed = 0;
        }

    }

    public void Hit(int explosivePlayerId)
    {
        if (gameObject.tag == "Target")
        {
            gameObject.GetComponent<Renderer>().material = testMaterial;
            carController.audioHandler.HitTarget();
        }

        if (isImmune == false)
        {
            if (gameObject.tag == "Player" && playerId != explosivePlayerId)
            {
 everetts-branch
                isStunned=true;
                uiHandler.playerAliveIndex[playerId] = false;
                carController.health--;
                carController.speed = 0;
                isImmune = true;
                StartCoroutine(ImmunityTime());

                if (carController.health <= 0)
                {
                    isStunned = true;
                }
 main
            }
        }
    }

    public void Stun(int explosivePlayerId) 
    {
        if (gameObject.tag == "Player" && playerId != explosivePlayerId)
        {
            isStunned = true;
            StartCoroutine(StunDuration());
        }
    }

    private IEnumerator StunDuration()
    {
        // Wait for 2 seconds before continuing
        yield return new WaitForSeconds(3f);
        isStunned = false;
    }

    private IEnumerator ImmunityTime()
    {
        // Wait for 2 seconds before continuing
        yield return new WaitForSeconds(4f);
        isImmune = false;
    }

}
