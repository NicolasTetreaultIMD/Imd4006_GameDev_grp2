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
    float lerpDuration ;
    float lerpStartTime = Time.time;
    Color originalColor;

    private MeshRenderer[] meshRenderers;  // Array to store the mesh renderers
    private Color[] originalColors;        // Array to store original colors of mesh renderers

    // Start is called before the first frame update
    void Start()
    {
        uiHandler = GameObject.Find("Canvas").GetComponent<uiHandler>();
        carController = GetComponent<CarController>();
        isStunned = false;
        isImmune = true;
        shield.gameObject.SetActive(true);

        lerpDuration = 1f; // Time to complete one lerp cycle
        lerpStartTime = Time.time;
        originalColor = shield.GetComponent<Material>().color;
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
            // Calculate the new alpha value using Mathf.PingPong to smoothly oscillate between two values
            float lerpTime = (Time.time - lerpStartTime) / lerpDuration;
            float alpha = Mathf.Lerp(20f, 70f, Mathf.PingPong(lerpTime, 1));  // PingPong will oscillate between 0 and 1

            // Set the alpha in the material's color
            Color newColor = originalColor;
            newColor.a = alpha / 100f; // Divide by 100 to get the alpha between 0 and 1
            shield.GetComponent<Material>().color = newColor;
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
                isStunned=true;
                uiHandler.playerAliveIndex[playerId] = false;
                carController.health--;
                carController.speed = 0;

                isImmune = true;
                shield.SetActive(true);
                StartCoroutine(ImmunityTime());

                if (carController.health <= 0)
                {
                    isStunned = true;
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
    }

    private IEnumerator ShieldFlash()
    {
        float lerpDuration = 1f; // Time to complete one lerp cycle
        float lerpStartTime = Time.time;

        Color originalColor;

        originalColor = shield.GetComponent<Material>().color;

        while (isImmune) // Keep looping the alpha lerp back and forth
        {
            // Calculate the new alpha value using Mathf.PingPong to smoothly oscillate between two values
            float lerpTime = (Time.time - lerpStartTime) / lerpDuration;
            float alpha = Mathf.Lerp(20f, 70f, Mathf.PingPong(lerpTime, 1));  // PingPong will oscillate between 0 and 1

            // Set the alpha in the material's color
            Color newColor = originalColor;
            newColor.a = alpha / 100f; // Divide by 100 to get the alpha between 0 and 1
            shield.GetComponent<Material>().color = newColor;

            yield return null; // Wait for the next frame
        }
    }
}
