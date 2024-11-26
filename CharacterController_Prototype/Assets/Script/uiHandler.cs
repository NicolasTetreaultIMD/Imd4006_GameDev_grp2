using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.XR;


public class uiHandler : MonoBehaviour
{
    public GameObject[] players = new GameObject[4];

    [Header("Ammo Types")]
    public GameObject p0_currentBomb;
    public GameObject p0_currentMine;
    public GameObject p0_currentNuke;

    public GameObject p0_nextBomb;
    public GameObject p0_nextMine;
    public GameObject p0_nextNuke;

    public GameObject p1_currentBomb;
    public GameObject p1_currentMine;
    public GameObject p1_currentNuke;

    public GameObject p1_nextBomb;
    public GameObject p1_nextMine;
    public GameObject p1_nextNuke;

    [Header("Ammo Types")]
    public GameObject p0_health3;
    public GameObject p0_health2;
    public GameObject p0_health1;

    public GameObject p1_health3;
    public GameObject p1_health2;
    public GameObject p1_health1;


    // Start is called before the first frame update

    void Start()
    {

        p0_currentBomb.SetActive(false);
        p0_currentMine.SetActive(false);
        p0_currentNuke.SetActive(false);
        p0_nextBomb.SetActive(false);
        p0_nextMine.SetActive(false);
        p0_nextNuke.SetActive(false);
        p1_currentBomb.SetActive(false);
        p1_currentMine.SetActive(false);
        p1_currentNuke.SetActive(false);
        p1_nextBomb.SetActive(false);
        p1_nextMine.SetActive(false);
        p1_nextNuke.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        // Update the UI text to show the projectile count
        if (players[0] != null)
        {

            // PLAYER 1 has AMMO
            if (players[0].GetComponent<CarController>().cannon.projectile.Count > 0) 
            {
                ShowCurrentAmmoType(0, players[0].GetComponent<CarController>().cannon.projectile[0].name); // Show current ammo type
                ShowNextAmmoType(0, players[0].GetComponent<CarController>().cannon.projectile[1].name); // Show current ammo type
            }
            else // PLAYER 1 has no ammo
            {
                ShowCurrentAmmoType(0, "Empty");
            }
        }

        if (players[1] != null) 
        {
            // PLAYER 2 has AMMO
            if (players[1].GetComponent<CarController>().cannon.projectile.Count > 0)
            {
                ShowCurrentAmmoType(1, players[1].GetComponent<CarController>().cannon.projectile[0].name); // Show current ammo type
                ShowNextAmmoType(1, players[1].GetComponent<CarController>().cannon.projectile[1].name); // Show current ammo type
            }
            else // PLAYER 2 has no ammo
            {
                ShowCurrentAmmoType(1, "Empty");
            }

        }
    }

    // Function toggles element visibility state using the bool type
    public void ShowCurrentAmmoType(int playerID, string type) 
    {
        if (playerID == 0) // FIRST PLAYER
        {
            if (type == "Bomb Item")
            {
                p0_currentBomb.SetActive(true);
                p0_currentMine.SetActive(false);
                p0_currentNuke.SetActive(false);
            }
            else if (type == "Mine Item")
            {
                p0_currentBomb.SetActive(false);
                p0_currentMine.SetActive(true);
                p0_currentNuke.SetActive(false);
            }
            else if (type == "Nuke Item")
            {
                p0_currentBomb.SetActive(false);
                p0_currentMine.SetActive(false);
                p0_currentNuke.SetActive(true);

            }
            else if (type == "Empty") // No ammo is left
            {
                p0_currentBomb.SetActive(false);
                p0_currentMine.SetActive(false);
                p0_currentNuke.SetActive(false);
            }
        }
        else if (playerID == 1) // SECOND PLAYER
        {
            if (type == "Bomb Item")
            {
                p1_currentBomb.SetActive(true);
                p1_currentMine.SetActive(false);
                p1_currentNuke.SetActive(false);
            }
            else if (type == "Mine Item")
            {
                p1_currentBomb.SetActive(false);
                p1_currentMine.SetActive(true);
                p1_currentNuke.SetActive(false);
            }
            else if (type == "Nuke Item")
            {
                p1_currentBomb.SetActive(false);
                p1_currentMine.SetActive(false);
                p1_currentNuke.SetActive(true);

            }
            else if (type == "Empty") // No ammo is left
            {
                p1_currentBomb.SetActive(false);
                p1_currentMine.SetActive(false);
                p1_currentNuke.SetActive(false);
            }
        }
    }

    // Function toggles element visibility state using the bool type
    public void ShowNextAmmoType(int playerID, string type)
    {
        if (playerID == 0) // PLAYER ONE
        {
            if (type == "Bomb Item")
            {
                p0_currentBomb.SetActive(true);
                p0_currentMine.SetActive(false);
                p0_currentNuke.SetActive(false);
            }
            else if (type == "Mine Item")
            {
                p0_currentBomb.SetActive(false);
                p0_currentMine.SetActive(true);
                p0_currentNuke.SetActive(false);
            }
            else if (type == "Nuke Item")
            {
                p0_currentBomb.SetActive(false);
                p0_currentMine.SetActive(false);
                p0_currentNuke.SetActive(true);

            }
            else if (type == "Empty") // No ammo is left
            {
                p0_currentBomb.SetActive(false);
                p0_currentMine.SetActive(false);
                p0_currentNuke.SetActive(false);
            }
        }

        if (playerID == 1) // PLAYER 2
        {
            if (type == "Bomb Item")
            {
                p1_currentBomb.SetActive(true);
                p1_currentMine.SetActive(false);
                p1_currentNuke.SetActive(false);
            }
            else if (type == "Mine Item")
            {
                p1_currentBomb.SetActive(false);
                p1_currentMine.SetActive(true);
                p1_currentNuke.SetActive(false);
            }
            else if (type == "Nuke Item")
            {
                p1_currentBomb.SetActive(false);
                p1_currentMine.SetActive(false);
                p1_currentNuke.SetActive(true);

            }
            else if (type == "Empty") // No ammo is left
            {
                p1_currentBomb.SetActive(false);
                p1_currentMine.SetActive(false);
                p1_currentNuke.SetActive(false);
            }
        }
    }

}
