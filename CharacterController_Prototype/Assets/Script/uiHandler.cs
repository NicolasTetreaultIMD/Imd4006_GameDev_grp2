using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.XR;


public class uiHandler : MonoBehaviour
{
    public GameObject[] players = new GameObject[4];

    [Header("UI Elements")]
    public TextMeshProUGUI p1_ammoCount;
    public TextMeshProUGUI p0_ammoCount;

    // Start is called before the first frame update

    void Start()
    {
        p0_ammoCount = FindObjectOfType<TextMeshProUGUI>();
        p1_ammoCount = FindObjectOfType<TextMeshProUGUI>();

        p0_ammoCount.text = "Ammo: 0";
        p1_ammoCount.text = "Ammo: 0";

    }

    // Update is called once per frame
    void Update()
    {
        // Update the UI text to show the projectile count
        if (players[0] != null)
        {
            p0_ammoCount.text = "Ammo: " + players[0].GetComponent<CarController>().cannon.projectile.Count.ToString();
        }

        if (players[1] != null) 
        {
            p1_ammoCount.text = "Ammo: " + players[1].GetComponent<CarController>().cannon.projectile.Count.ToString();

            //Debug.Log(players[1].GetComponent<CarController>().health);

        }
    }

}
