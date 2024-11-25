using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class uiHandler : MonoBehaviour
{
    //public CarController carController;
    public GameObject[] players = new GameObject[4];

    [Header("UI Elements")]
    public TextMeshProUGUI ammoCountText;

    // Start is called before the first frame update

    void Start()
    {
        ammoCountText = FindObjectOfType<TextMeshProUGUI>();
        ammoCountText.text = "Ammo: 0";
    }

    // Update is called once per frame
    void Update()
    {
        // Update the UI text to show the projectile count
        //ammoCountText.text = "Ammo: " + carController.cannon.projectile.Count.ToString();

        if (players[1] != null) 
        {
            Debug.Log(players[1].GetComponent<CarController>().health);
        }
    }

}
