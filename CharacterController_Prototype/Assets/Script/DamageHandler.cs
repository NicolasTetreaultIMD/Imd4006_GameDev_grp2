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

    public GameObject shield;
    public Material shieldTerial;
    float lerpDuration;
    float lerpStartTime;


    // Start is called before the first frame update
    void Start()
    {

        uiHandler = GameObject.Find("Canvas").GetComponent<uiHandler>();
        carController = GetComponent<CarController>();
        isStunned = false;

        shield.gameObject.SetActive(false);
        shieldTerial = shield.GetComponent<Renderer>().material;

        lerpDuration = 1f; // Time to complete one lerp cycle
        lerpStartTime = Time.time;
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

        if (isImmune) // Keep looping the alpha lerp back and forth
        {
            float lerpTime = (Time.time - lerpStartTime) / lerpDuration;
            float alpha = Mathf.Lerp(40f, 100f, Mathf.PingPong(lerpTime, 1));  // PingPong will oscillate between 0 and 1

            Color newColor = shieldTerial.color;
            newColor.a = alpha / 255f;
            shieldTerial.color = newColor;
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
                uiHandler.playerAliveIndex[playerId] = false;
                carController.health--;
                carController.speed = 0;

                if (carController.health <= 0)
                {
                    shield.SetActive(true);
                    isStunned = true;
                }
                else
                {
                    isImmune = true;
                    StartCoroutine(ImmunityTime());
                    shield.SetActive(true);

                }
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
        yield return new WaitForSeconds(3f);
        isStunned = false;
    }

    private IEnumerator ImmunityTime()
    {
        yield return new WaitForSeconds(4f);
        isImmune = false;
        shield.SetActive(false);
    }
}
