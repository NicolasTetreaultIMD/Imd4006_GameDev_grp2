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

    [Header("Player Health")]
    public GameObject p0_health3;
    public GameObject p0_health2;
    public GameObject p0_health1;

    public GameObject p1_health3;
    public GameObject p1_health2;
    public GameObject p1_health1;


    // Start is called before the first frame update

    void Start()
    {
        // hide UI's by default
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

        p0_health2.SetActive(false);
        p0_health1.SetActive(false);

        p1_health2.SetActive(false);
        p1_health1.SetActive(false);
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

                if (players[0].GetComponent<CarController>().cannon.projectile.Count > 1)
                {
                    ShowNextAmmoType(0, players[0].GetComponent<CarController>().cannon.projectile[1].name); // Show current ammo type
                }
                else // if the player only has 1 ammo set the next ammo type to empty
                {
                    ShowNextAmmoType(0, "Empty"); // Show current ammo type
                }
            }
            else // PLAYER 1 has no ammo
            {
                ShowCurrentAmmoType(0, "Empty");
            }

            CurrentHealth(0, players[0].GetComponent<CarController>().health); // Show current player health
        }

        if (players[1] != null) 
        {
            // PLAYER 2 has AMMO
            if (players[1].GetComponent<CarController>().cannon.projectile.Count > 0)
            {
                ShowCurrentAmmoType(1, players[1].GetComponent<CarController>().cannon.projectile[0].name); // Show current ammo type

                if (players[0].GetComponent<CarController>().cannon.projectile.Count > 1)
                {
                    ShowNextAmmoType(1, players[1].GetComponent<CarController>().cannon.projectile[1].name); // Show current ammo type
                }
                else // if the player only has 1 ammo set the next ammo type to empty
                {
                    ShowNextAmmoType(1, "Empty"); // Show current ammo type
                }
            }
            else // PLAYER 2 has no ammo
            {
                ShowCurrentAmmoType(1, "Empty");
            }

            CurrentHealth(1, players[1].GetComponent<CarController>().health); // Show current player health

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
                p0_nextBomb.SetActive(true);
                p0_nextMine.SetActive(false);
                p0_nextNuke.SetActive(false);
            }
            else if (type == "Mine Item")
            {
                p0_nextBomb.SetActive(false);
                p0_nextMine.SetActive(true);
                p0_nextNuke.SetActive(false);
            }
            else if (type == "Nuke Item")
            {
                p0_nextBomb.SetActive(false);
                p0_nextMine.SetActive(false);
                p0_nextNuke.SetActive(true);

            }
            else if (type == "Empty") // No ammo is left
            {
                p0_nextBomb.SetActive(false);
                p0_nextMine.SetActive(false);
                p0_nextNuke.SetActive(false);
            }
        }

        if (playerID == 1) // PLAYER TWO
        {
            if (type == "Bomb Item")
            {
                p1_nextBomb.SetActive(true);
                p1_nextMine.SetActive(false);
                p1_nextNuke.SetActive(false);
            }
            else if (type == "Mine Item")
            {
                p1_nextBomb.SetActive(false);
                p1_nextMine.SetActive(true);
                p1_nextNuke.SetActive(false);
            }
            else if (type == "Nuke Item")
            {
                p1_nextBomb.SetActive(false);
                p1_nextMine.SetActive(false);
                p1_nextNuke.SetActive(true);

            }
            else if (type == "Empty") // No ammo is left
            {
                p1_nextBomb.SetActive(false);
                p1_nextMine.SetActive(false);
                p1_nextNuke.SetActive(false);
            }
        }
    }

    // Show current player health
    public void CurrentHealth(int playerID, int health)
    {
        if(playerID == 0) // PLAYER ONE
        {
            if(health == 3)
            {
                p0_health3.SetActive(true);
                p0_health2.SetActive(false);
                p0_health1.SetActive(false);
            }
            if (health == 2)
            {
                p0_health3.SetActive(false);
                p0_health2.SetActive(true);
                p0_health1.SetActive(false);
            }
            if (health == 1)
            {
                p0_health3.SetActive(false);
                p0_health2.SetActive(false);
                p0_health1.SetActive(true);
            }
        }

        if (playerID == 1) // PLAYER TWO
        {
            if (health == 3)
            {
                p1_health3.SetActive(true);
                p1_health2.SetActive(false);
                p1_health1.SetActive(false);
            }
            if (health == 2)
            {
                p1_health3.SetActive(false);
                p1_health2.SetActive(true);
                p1_health1.SetActive(false);
            }
            if (health == 1)
            {
                p1_health3.SetActive(false);
                p1_health2.SetActive(false);
                p1_health1.SetActive(true);
            }
        }
    }

}
