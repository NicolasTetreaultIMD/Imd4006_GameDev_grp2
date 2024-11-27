using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.XR;
using UnityEngine.VFX;


public class uiHandler : MonoBehaviour
{
    public GameObject[] players = new GameObject[4];
    [Header("Frames")]
    public GameObject p0_ammoFrame;
    public GameObject p1_ammoFrame;

    [Header("Ammo Types")]
    public GameObject p0_currentBomb;
    public GameObject p0_currentMine;
    public GameObject p0_currentNuke;
    public GameObject p0_currentTrap;

    public GameObject p0_nextBomb;
    public GameObject p0_nextMine;
    public GameObject p0_nextNuke;
    public GameObject p0_nextTrap;

    public GameObject p1_currentBomb;
    public GameObject p1_currentMine;
    public GameObject p1_currentNuke;
    public GameObject p1_currentTrap;

    public GameObject p1_nextBomb;
    public GameObject p1_nextMine;
    public GameObject p1_nextNuke;
    public GameObject p1_nextTrap;

    [Header("Player Health")]
    public GameObject p0_health3;
    public GameObject p0_health2;
    public GameObject p0_health1;
    public GameObject p0_health0;

    public GameObject p1_health3;
    public GameObject p1_health2;
    public GameObject p1_health1;
    public GameObject p1_health0;

    [Header("Instructions")]
    public GameObject p0_instructions_run;
    public GameObject p0_instructions_aim;
    public GameObject p0_instructions_shoot;
    public GameObject p0_instructions_shoot2;
    public GameObject p0_instructions_bump;

    public GameObject p1_instructions_run;
    public GameObject p1_instructions_aim;
    public GameObject p1_instructions_shoot;
    public GameObject p1_instructions_shoot2;
    public GameObject p1_instructions_bump;

    public GameObject p0_pressAtoJoin;
    public GameObject p1_pressAtoJoin;

    [Header("PlayerJoin")]
    public int playerCount;

    [Header("WinStates")]
    public bool[] playerAliveIndex = new bool[4];

    public GameObject p0_winner;
    public GameObject p0_loser;

    public GameObject p1_winner;
    public GameObject p1_loser;

    private bool tutorialFlag;




    // Start is called before the first frame update

    void Start()
    {
        playerAliveIndex[0] = true;
        playerAliveIndex[1] = true;
        playerAliveIndex[2] = false;
        playerAliveIndex[3] = false;

        playerCount = 0;
        tutorialFlag = false; // not yet seen tutorial prompts
        p0_winner.SetActive(false);
        p1_winner.SetActive(false);
        HideP0UI();
        HideP1UI();
        


    }

    // Update is called once per frame
    void Update()
    {
        // SHOW UI dependent on player count

        if (playerCount == 0)
        {
            HideP0UI();
            HideP1UI();
        }

        if (playerCount == 1) 
        {
            p0_ammoFrame.SetActive(true);
            p0_pressAtoJoin.SetActive(false); // hide press A to join
            HideP1UI();
        }

        if (playerCount == 2)
        {
            p1_ammoFrame.SetActive(true);
            p1_pressAtoJoin.SetActive(false); // hide press A to join

            // Show tutorial if not yet seen 
            if (!tutorialFlag)
            {
                StartCoroutine(ShowTutorial());
                tutorialFlag=true;
            }
        }

        if (playerCount == 3) 
        {

        }

        if (playerCount == 4)
        {

        }

        // Update the UI text to show the projectile count
        if (playerCount >= 1)
        {

            // PLAYER 1 has AMMO
            if (players[0].GetComponent<CarController>().cannon.projectile.Count >= 1) 
            {
                ShowCurrentAmmoType(0, players[0].GetComponent<CarController>().cannon.projectile[0].name); // Show current ammo type

                if (players[0].GetComponent<CarController>().cannon.projectile.Count >= 2)
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

        if (playerCount >= 2) 
        {
            // PLAYER 2 has AMMO
            if (players[1].GetComponent<CarController>().cannon.projectile.Count >= 1)
            {
                ShowCurrentAmmoType(1, players[1].GetComponent<CarController>().cannon.projectile[0].name); // Show current ammo type

                if (players[1].GetComponent<CarController>().cannon.projectile.Count >= 2)
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
        // DETECT WHEN THERE IS A WINNER IF > 2 PLAYERS
        if(playerCount >=2)
        {
            DetectWinner();
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
                p0_currentTrap.SetActive(false);

            }
            else if (type == "Mine Item")
            {
                p0_currentBomb.SetActive(false);
                p0_currentMine.SetActive(true);
                p0_currentNuke.SetActive(false);
                p0_currentTrap.SetActive(false);

            }
            else if (type == "Nuke Item")
            {
                p0_currentBomb.SetActive(false);
                p0_currentMine.SetActive(false);
                p0_currentNuke.SetActive(true);
                p0_currentTrap.SetActive(false);

            }
            else if (type == "Trap Item")
            {
                p0_currentBomb.SetActive(false);
                p0_currentMine.SetActive(false);
                p0_currentNuke.SetActive(false);
                p0_currentTrap.SetActive(true);
            }

            else if (type == "Empty") // No ammo is left
            {
                p0_currentBomb.SetActive(false);
                p0_currentMine.SetActive(false);
                p0_currentNuke.SetActive(false);
                p0_currentTrap.SetActive(false);
            }
        }
        else if (playerID == 1) // SECOND PLAYER
        {
            if (type == "Bomb Item")
            {
                p1_currentBomb.SetActive(true);
                p1_currentMine.SetActive(false);
                p1_currentNuke.SetActive(false);
                p1_currentTrap.SetActive(false);
            }
            else if (type == "Mine Item")
            {
                p1_currentBomb.SetActive(false);
                p1_currentMine.SetActive(true);
                p1_currentNuke.SetActive(false);
                p1_currentTrap.SetActive(false);
            }
            else if (type == "Nuke Item")
            {
                p1_currentBomb.SetActive(false);
                p1_currentMine.SetActive(false);
                p1_currentNuke.SetActive(true);
                p1_currentTrap.SetActive(false);


            }
            else if (type == "Trap Item")
            {
                p1_currentBomb.SetActive(false);
                p1_currentMine.SetActive(false);
                p1_currentNuke.SetActive(false);
                p1_currentTrap.SetActive(true);
            }
            else if (type == "Empty") // No ammo is left
            {
                p1_currentBomb.SetActive(false);
                p1_currentMine.SetActive(false);
                p1_currentNuke.SetActive(false);
                p1_currentTrap.SetActive(false);

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
                p0_nextTrap.SetActive(false);
            }
            else if (type == "Mine Item")
            {
                p0_nextBomb.SetActive(false);
                p0_nextMine.SetActive(true);
                p0_nextNuke.SetActive(false);
                p0_nextTrap.SetActive(false);
            }
            else if (type == "Nuke Item")
            {
                p0_nextBomb.SetActive(false);
                p0_nextMine.SetActive(false);
                p0_nextNuke.SetActive(true);
                p0_nextTrap.SetActive(false);

            }
            else if (type == "Trap Item")
            {
                p0_nextBomb.SetActive(false);
                p0_nextMine.SetActive(false);
                p0_nextNuke.SetActive(false);
                p0_nextTrap.SetActive(true);
            }
            else if (type == "Empty") // No ammo is left
            {
                p0_nextBomb.SetActive(false);
                p0_nextMine.SetActive(false);
                p0_nextNuke.SetActive(false);
                p0_nextTrap.SetActive(false);
            }
        }

        if (playerID == 1) // PLAYER TWO
        {
            if (type == "Bomb Item")
            {
                p1_nextBomb.SetActive(true);
                p1_nextMine.SetActive(false);
                p1_nextNuke.SetActive(false);
                p1_nextTrap.SetActive(false);
            }
            else if (type == "Mine Item")
            {
                p1_nextBomb.SetActive(false);
                p1_nextMine.SetActive(true);
                p1_nextNuke.SetActive(false);
                p1_nextTrap.SetActive(false);
            }
            else if (type == "Nuke Item")
            {
                p1_nextBomb.SetActive(false);
                p1_nextMine.SetActive(false);
                p1_nextNuke.SetActive(true);
                p1_nextTrap.SetActive(false);

            }
            else if (type == "Trap Item")
            {
                p1_nextBomb.SetActive(false);
                p1_nextMine.SetActive(false);
                p1_nextNuke.SetActive(false);
                p1_nextTrap.SetActive(true);
            }
            else if (type == "Empty") // No ammo is left
            {
                p1_nextBomb.SetActive(false);
                p1_nextMine.SetActive(false);
                p1_nextNuke.SetActive(false);
                p1_nextTrap.SetActive(false);
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
                p0_health0.SetActive(false);
            }
            if (health == 2)
            {
                p0_health3.SetActive(false);
                p0_health2.SetActive(true);
                p0_health1.SetActive(false);
                p0_health0.SetActive(false);
            }
            if (health == 1)
            {
                p0_health3.SetActive(false);
                p0_health2.SetActive(false);
                p0_health1.SetActive(true);
                p0_health0.SetActive(false);

            }
            if (health == 0)
            {
                p0_health3.SetActive(false);
                p0_health2.SetActive(false);
                p0_health1.SetActive(false);
                p0_health0.SetActive(true);
            }
        }

        if (playerID == 1) // PLAYER TWO
        {
            if (health == 3)
            {
                p1_health3.SetActive(true);
                p1_health2.SetActive(false);
                p1_health1.SetActive(false);
                p1_health0.SetActive(false);
            }
            if (health == 2)
            {
                p1_health3.SetActive(false);
                p1_health2.SetActive(true);
                p1_health1.SetActive(false);
                p1_health0.SetActive(false);
            }
            if (health == 1)
            {
                p1_health3.SetActive(false);
                p1_health2.SetActive(false);
                p1_health1.SetActive(true);
                p1_health0.SetActive(false);
            }
            if (health == 0)
            {
                p1_health3.SetActive(false);
                p1_health2.SetActive(false);
                p1_health1.SetActive(false);
                p1_health0.SetActive(true);
            }
        }
    }


    // TUTORIAL
    private IEnumerator ShowTutorial()
    {
        // Show run instructions
        p0_instructions_run.SetActive(true);
        p1_instructions_run.SetActive(true);
        yield return new WaitForSeconds(12);
        p0_instructions_run.SetActive(false);
        p1_instructions_run.SetActive(false);

        // AIM
        p0_instructions_aim.SetActive(true);
        p1_instructions_aim.SetActive(true);
        yield return new WaitForSeconds(9);
        p0_instructions_aim.SetActive(false);
        p1_instructions_aim.SetActive(false);

        // SHOOT
        p0_instructions_shoot.SetActive(true);
        p0_instructions_shoot2.SetActive(true);
        p1_instructions_shoot2.SetActive(true);
        p1_instructions_shoot2.SetActive(true);
        yield return new WaitForSeconds(9);
        p0_instructions_shoot.SetActive(false);
        p0_instructions_shoot2.SetActive(false);
        p1_instructions_shoot2.SetActive(false);
        p1_instructions_shoot2.SetActive(false);

        // BUMP
        p0_instructions_bump.SetActive(true);
        p1_instructions_bump.SetActive(true);
        yield return new WaitForSeconds(9);
        p0_instructions_bump.SetActive(false);
        p1_instructions_bump.SetActive(false);

    }

    public void AddPlayer()
    {
        playerCount++;        
    }

    public void RemovePlayer()
    {
        playerCount--;
    }

    private void HideP0UI() // HIDE PLAYER 1 PROMPT
    {
        p0_pressAtoJoin.SetActive(true); // SHOW JOIN PROMPT

        // hide UI's by default
        p0_ammoFrame.SetActive(false);
        p0_currentBomb.SetActive(false);
        p0_currentMine.SetActive(false);
        p0_currentNuke.SetActive(false);
        p0_currentTrap.SetActive(false);
        p0_nextBomb.SetActive(false);
        p0_nextMine.SetActive(false);
        p0_nextNuke.SetActive(false);
        p0_nextTrap.SetActive(false);
        
        p0_health3.SetActive(false);
        p0_health2.SetActive(false);
        p0_health1.SetActive(false);
        p0_health0.SetActive(false);

       
        p0_instructions_run.SetActive(false);
        p0_instructions_aim.SetActive(false);
        p0_instructions_shoot.SetActive(false);
        p0_instructions_shoot2.SetActive(false);
        p0_instructions_bump.SetActive(false);

       
    }

    private void HideP1UI() // HIDE PLAYER 2 UI
    {
        p1_pressAtoJoin.SetActive(true); // SHOW JOIN PROMPT

        p1_ammoFrame.SetActive(false);
        p1_currentBomb.SetActive(false);
        p1_currentMine.SetActive(false);
        p1_currentNuke.SetActive(false);
        p1_currentTrap.SetActive(false);
        p1_nextBomb.SetActive(false);
        p1_nextMine.SetActive(false);
        p1_nextNuke.SetActive(false);
        p1_nextTrap.SetActive(false);

        p1_health3.SetActive(false);
        p1_health2.SetActive(false);
        p1_health1.SetActive(false);
        p1_health0.SetActive(false);

        p1_instructions_run.SetActive(false);
        p1_instructions_aim.SetActive(false);
        p1_instructions_shoot.SetActive(false);
        p1_instructions_shoot2.SetActive(false);
        p1_instructions_bump.SetActive(false);
    }

    public void DetectWinner()
    {
        int playersAliveCount = 0;

        for(int i = 0; i < playerAliveIndex.Length; i++)
        {
            if (playerAliveIndex[i])
            {
                playersAliveCount++;
            }
        }

        if(playersAliveCount == 1)
        {
            for (int i = 0; i < playerAliveIndex.Length; i++)
            {
                if (playerAliveIndex[i] == true)
                {
                    if(i == 0) // PLAYER 1 wins
                    {
                        p0_winner.SetActive(true); // Show player 0 as the winner
                        p1_loser.SetActive(true); // Show player 1 as the loser
                    }
                    else if (i== 1) // PLAYER 2 wins
                    {
                        p1_winner.SetActive(true); // Show player 1 as the winner
                        p0_loser.SetActive(true); // Show player 0 as the loser
                    }
                    else if (i == 2) // PLAYER 3 wins
                    {
                        p1_loser.SetActive(true); // Show player 1 as the loser
                        p0_loser.SetActive(true); // Show player 0 as the loser
                    }
                    else if (i == 3) // PLAYER 4 wins
                    {
                        p1_loser.SetActive(true); // Show player 1 as the loser
                        p0_loser.SetActive(true); // Show player 0 as the loser
                    }
                }
            }
        }
        
    }
}