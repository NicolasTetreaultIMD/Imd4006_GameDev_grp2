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

    private MeshRenderer[] meshRenderers;  // Array to store the mesh renderers
    private Color[] originalColors;        // Array to store original colors of mesh renderers

    // Start is called before the first frame update
    void Start()
    {
        uiHandler = GameObject.Find("Canvas").GetComponent<uiHandler>();
        carController = GetComponent<CarController>();
        isStunned = false;

        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        originalColors = new Color[meshRenderers.Length];

        // Save the original colors of all MeshRenderers
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            originalColors[i] = meshRenderers[i].material.color;
        }
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

        if(isImmune)
        {
            shield.GetComponent<Material>().color.a = 0;
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
}
