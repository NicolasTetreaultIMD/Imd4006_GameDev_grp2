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
    public GameObject p2_ammoFrame;
    public GameObject p3_ammoFrame;

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

    public GameObject p2_currentBomb;
    public GameObject p2_currentMine;
    public GameObject p2_currentNuke;
    public GameObject p2_currentTrap;

    public GameObject p2_nextBomb;
    public GameObject p2_nextMine;
    public GameObject p2_nextNuke;
    public GameObject p2_nextTrap;

    public GameObject p3_currentBomb;
    public GameObject p3_currentMine;
    public GameObject p3_currentNuke;
    public GameObject p3_currentTrap;

    public GameObject p3_nextBomb;
    public GameObject p3_nextMine;
    public GameObject p3_nextNuke;
    public GameObject p3_nextTrap;

    [Header("Player Health")]
    public GameObject p0_health3;
    public GameObject p0_health2;
    public GameObject p0_health1;
    public GameObject p0_health0;

    public GameObject p1_health3;
    public GameObject p1_health2;
    public GameObject p1_health1;
    public GameObject p1_health0;

    public GameObject p2_health3;
    public GameObject p2_health2;
    public GameObject p2_health1;
    public GameObject p2_health0;

    public GameObject p3_health3;
    public GameObject p3_health2;
    public GameObject p3_health1;
    public GameObject p3_health0;

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

    public GameObject p2_instructions_run;
    public GameObject p2_instructions_aim;
    public GameObject p2_instructions_shoot;
    public GameObject p2_instructions_shoot2;
    public GameObject p2_instructions_bump;

    public GameObject p3_instructions_run;
    public GameObject p3_instructions_aim;
    public GameObject p3_instructions_shoot;
    public GameObject p3_instructions_shoot2;
    public GameObject p3_instructions_bump;

    public GameObject p0_pressAtoJoin;
    public GameObject p1_pressAtoJoin;
    public GameObject p2_pressAtoJoin;
    public GameObject p3_pressAtoJoin;

    [Header("Bump UI")]
    public GameObject[] player_bump_disabled = new GameObject [4];

    [Header("Accelerate Prompt")]
    public GameObject[] player_accelerate = new GameObject [4];

    [Header("PlayerJoin")]
    public int playerCount;

    [Header("PlayersReady")]
    public GameObject awaitingPlayers;
    public GameObject[] countdown3 = new GameObject [4];
    public GameObject[] countdown2 = new GameObject [4];
    public GameObject[] countdown1 = new GameObject[4];
    public GameObject[] countdown_ready = new GameObject[4];


    [Header("WinStates")]
    public bool[] playerAliveIndex = new bool[4];

    public GameObject p0_winner;
    public GameObject p0_loser;

    public GameObject p1_winner;
    public GameObject p1_loser;

    public GameObject p2_winner;
    public GameObject p2_loser;

    public GameObject p3_winner;
    public GameObject p3_loser;

    private bool tutorialFlag;

    private bool countdownStart;




    // Start is called before the first frame update

    void Start()
    {
        // ** CHANGE THESE IF TESTING ONLY TWO PLAYERS (set index 2 and 3 to false)
        playerAliveIndex[0] = true;
        playerAliveIndex[1] = true;
        playerAliveIndex[2] = true;
        playerAliveIndex[3] = true;

        countdown3[0].SetActive(false);
        countdown2[0].SetActive(false);
        countdown1[0].SetActive(false);
        countdown_ready[0].SetActive(false);

        countdown3[1].SetActive(false);
        countdown2[1].SetActive(false);
        countdown1[1].SetActive(false);
        countdown_ready[1].SetActive(false);

        countdown3[2].SetActive(false);
        countdown2[2].SetActive(false);
        countdown1[2].SetActive(false);
        countdown_ready[2].SetActive(false);

        countdown3[3].SetActive(false);
        countdown2[3].SetActive(false);
        countdown1[3].SetActive(false);
        countdown_ready[3].SetActive(false);

        player_bump_disabled[0].SetActive(false);
        player_bump_disabled[1].SetActive(false);
        player_bump_disabled[2].SetActive(false);
        player_bump_disabled[3].SetActive(false);


        countdownStart = false;
        playerCount = 0;
        tutorialFlag = false; // not yet seen tutorial prompts
        p0_winner.SetActive(false);
        p1_winner.SetActive(false);
        p2_winner.SetActive(false);
        p3_winner.SetActive(false);
        awaitingPlayers.SetActive(true);
        ShowJoinPrompt_P1();
        ShowJoinPrompt_P2();
        ShowJoinPrompt_P3();
        ShowJoinPrompt_P4();
        


    }

    // Update is called once per frame
    void Update()
    {
        // SHOW UI dependent on player count

        if (playerCount == 0)
        {
            ShowJoinPrompt_P1();
            ShowJoinPrompt_P2();
            ShowJoinPrompt_P3();
            ShowJoinPrompt_P4();
        }

        if (playerCount == 1) 
        {
            p0_ammoFrame.SetActive(true);
            p0_pressAtoJoin.SetActive(false); // hide press A to join
            
            // SHOW JOIN PROMPTS FOR REMAINING PLAYERS
            ShowJoinPrompt_P2();
            ShowJoinPrompt_P3();
            ShowJoinPrompt_P4();
        }

        if (playerCount == 2)
        {

            p1_ammoFrame.SetActive(true);
            p1_pressAtoJoin.SetActive(false); // hide press A to join
            ShowJoinPrompt_P3();
            ShowJoinPrompt_P4();
        }

        if (playerCount == 3) 
        {
            p2_ammoFrame.SetActive(true);
            p2_pressAtoJoin.SetActive(false); // hide press A to join
            ShowJoinPrompt_P4();
        }

        if (playerCount == 4)
        {
            p3_ammoFrame.SetActive(true);
            p3_pressAtoJoin.SetActive(false); // hide press A to join
            // Show tutorial when all players are in the lobby

            //if (!tutorialFlag)
            //{
            //    StartCoroutine(ShowTutorial());
            //    tutorialFlag = true;
            //}

            if (!countdownStart) // ** MOVE THIS TO 4 PLAYER FOR FINAL **
            {
                PlayersReady(); // REMOVE WAITING SCREEN AND START COUNTDOWN
                countdownStart = true;
            }
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
                    ShowNextAmmoType(1, players[1].GetComponent<CarController>().cannon.projectile[1].name);
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

        if (playerCount >= 3)
        {
            // PLAYER 3 has AMMO
            if (players[2].GetComponent<CarController>().cannon.projectile.Count >= 1)
            {
                ShowCurrentAmmoType(2, players[2].GetComponent<CarController>().cannon.projectile[0].name); // Show current ammo type

                if (players[2].GetComponent<CarController>().cannon.projectile.Count >= 2) // IF the player has reserve amo
                {
                    ShowNextAmmoType(2, players[2].GetComponent<CarController>().cannon.projectile[1].name);
                }
                else // if the player only has 1 ammo set the next ammo type to empty
                {
                    ShowNextAmmoType(2, "Empty"); // Show current ammo type
                }
            }
            else // PLAYER 3 has no ammo
            {
                ShowCurrentAmmoType(2, "Empty");
            }

            CurrentHealth(2, players[2].GetComponent<CarController>().health); // Show current player health

        }

        if (playerCount >= 4)
        {
            // PLAYER 3 has AMMO
            if (players[3].GetComponent<CarController>().cannon.projectile.Count >= 1)
            {
                ShowCurrentAmmoType(3, players[3].GetComponent<CarController>().cannon.projectile[0].name); // Show current ammo type

                if (players[3].GetComponent<CarController>().cannon.projectile.Count >= 2) // IF the player has reserve amo
                {
                    ShowNextAmmoType(3, players[3].GetComponent<CarController>().cannon.projectile[1].name);
                }
                else // if the player only has 1 ammo set the next ammo type to empty
                {
                    ShowNextAmmoType(3, "Empty"); // Show current ammo type
                }
            }
            else // PLAYER 3 has no ammo
            {
                ShowCurrentAmmoType(3, "Empty");
            }

            CurrentHealth(3, players[3].GetComponent<CarController>().health); // Show current player health

        }
        // DETECT WHEN THERE IS A WINNER IF > 2 PLAYERS
        if (playerCount >=2)
        {
            DetectWinner();
        }

        // PLAYER LOOP
        for (int i = 0; i < playerCount; i++)
        {
            //Show prompt to PRESS A to accelerate.Scale it up and down
            if (players[i].GetComponent<CarController>().cartState == CarController.CartState.Running) // If cart is in running state, prompt the player to press A
            {
                // SHOW PROMPT TO TAP A to move
                player_accelerate[i].SetActive(true);
                StartCoroutine(ScalePrompt(player_accelerate[i].transform, i)); // scale it up and down once

            }
            else // Player does not require prompt
            {
                player_accelerate[i].SetActive(false);
            }

            // DISABLE BUMP UI WHEN PLAYER IS IN BUMP STATE
            if (players[i].GetComponent<CarController>().cartState == CarController.CartState.InAir)
            {
                StartCoroutine(DisableBump(i));
            }

        }


    }

    // When the player uses a bump, show a disabled display
    private IEnumerator DisableBump(int playerID)
    {
        player_bump_disabled[playerID].SetActive(true);
        yield return new WaitForSeconds(4);
        player_bump_disabled[playerID].SetActive(false);

    }

    public void PlayersReady()
    {
        awaitingPlayers.SetActive(false);

        StartCoroutine(ShowDisablePrompt());
        Debug.Log("3");

    }

    // Used to display the countdown
    private IEnumerator ShowDisablePrompt()
    {
        countdown3[0].SetActive(true);
        countdown3[1].SetActive(true);
        countdown3[2].SetActive(true);
        countdown3[3].SetActive(true);
        yield return new WaitForSeconds(1);
        countdown3[0].SetActive(false);
        countdown3[1].SetActive(false);
        countdown3[2].SetActive(false);
        countdown3[3].SetActive(false);

        countdown2[0].SetActive(true);
        countdown2[1].SetActive(true);
        countdown2[2].SetActive(true);
        countdown2[3].SetActive(true);
        yield return new WaitForSeconds(1);
        countdown2[0].SetActive(false);
        countdown2[1].SetActive(false);
        countdown2[2].SetActive(false);
        countdown2[3].SetActive(false);

        countdown1[0].SetActive(true);
        countdown1[1].SetActive(true);
        countdown1[2].SetActive(true);
        countdown1[3].SetActive(true);
        yield return new WaitForSeconds(1);
        countdown1[0].SetActive(false);
        countdown1[1].SetActive(false);
        countdown1[2].SetActive(false);
        countdown1[3].SetActive(false);

        countdown_ready[0].SetActive(true);
        countdown_ready[1].SetActive(true);
        countdown_ready[2].SetActive(true);
        countdown_ready[3].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        countdown_ready[0].SetActive(false);
        countdown_ready[1].SetActive(false);
        countdown_ready[2].SetActive(false);
        countdown_ready [3].SetActive(false);

        players[0].GetComponent<CarController>().canMove = true;
        players[1].GetComponent<CarController>().canMove = true;
        players[2].GetComponent<CarController>().canMove = true;
        players[3].GetComponent<CarController>().canMove = true;
    }

    private IEnumerator ScalePrompt(Transform promptTransform, int index)
    {
        float scaleDuration = 0.5f; // Duration of one scale up or down
        float scaleAmount = 1.2f;   // Scale factor for the animation
        Vector3 originalScale = promptTransform.localScale;
        Vector3 targetScale = originalScale * scaleAmount;

        // Scale up
        yield return LerpScale(promptTransform, originalScale, targetScale, scaleDuration / 2);

        // Scale down
        yield return LerpScale(promptTransform, targetScale, originalScale, scaleDuration / 2);

    }

    private IEnumerator LerpScale(Transform target, Vector3 from, Vector3 to, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            target.localScale = Vector3.Lerp(from, to, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.localScale = to;
    }

    // Function toggles element visibility state using the bool type
    public void ShowCurrentAmmoType(int playerID, string type) 
    {
        void SetItemState(GameObject bomb, GameObject mine, GameObject nuke, GameObject trap, string type)
        {
            bomb.SetActive(type == "Bomb Item");
            mine.SetActive(type == "Mine Item");
            nuke.SetActive(type == "Nuke Item");
            trap.SetActive(type == "Trap Item");
        }

        if (playerID == 0) // FIRST PLAYER
        {
            SetItemState(p0_currentBomb, p0_currentMine, p0_currentNuke, p0_currentTrap, type);
        }
        else if (playerID == 1) // SECOND PLAYER
        {
            SetItemState(p1_currentBomb, p1_currentMine, p1_currentNuke, p1_currentTrap, type);
        }
        else if (playerID == 2) // SECOND PLAYER
        {
            SetItemState(p2_currentBomb, p2_currentMine, p2_currentNuke, p2_currentTrap, type);
        }
        else if (playerID == 3) // SECOND PLAYER
        {
            SetItemState(p3_currentBomb, p3_currentMine, p3_currentNuke, p3_currentTrap, type);
        }

    }

    // Function toggles element visibility state using the bool type
    public void ShowNextAmmoType(int playerID, string type)
    {
        void SetItemState(GameObject bomb, GameObject mine, GameObject nuke, GameObject trap, string type)
        {
            bomb.SetActive(type == "Bomb Item");
            mine.SetActive(type == "Mine Item");
            nuke.SetActive(type == "Nuke Item");
            trap.SetActive(type == "Trap Item");
        }

        if (playerID == 0) // FIRST PLAYER
        {
            SetItemState(p0_nextBomb, p0_nextMine, p0_nextNuke, p0_nextTrap, type);
        }
        else if (playerID == 1) // SECOND PLAYER
        {
            SetItemState(p1_nextBomb, p1_nextMine, p1_nextNuke, p1_nextTrap, type);
        }
        else if (playerID == 2) // THIRD PLAYER
        {
            SetItemState(p2_nextBomb, p2_nextMine, p2_nextNuke, p2_nextTrap, type);
        }
        else if (playerID == 4) // SECOND PLAYER
        {
            SetItemState(p3_nextBomb, p3_nextMine, p3_nextNuke, p3_nextTrap, type);
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

        if (playerID == 2) // PLAYER THREE
        {
            if (health == 3)
            {
                p2_health3.SetActive(true);
                p2_health2.SetActive(false);
                p2_health1.SetActive(false);
                p2_health0.SetActive(false);
            }
            if (health == 2)
            {
                p2_health3.SetActive(false);
                p2_health2.SetActive(true);
                p2_health1.SetActive(false);
                p2_health0.SetActive(false);
            }
            if (health == 1)
            {
                p2_health3.SetActive(false);
                p2_health2.SetActive(false);
                p2_health1.SetActive(true);
                p2_health0.SetActive(false);
            }
            if (health == 0)
            {
                p2_health3.SetActive(false);
                p2_health2.SetActive(false);
                p2_health1.SetActive(false);
                p2_health0.SetActive(true);
            }
        }

        if (playerID == 3) // PLAYER FOUR
        {
            if (health == 3)
            {
                p3_health3.SetActive(true);
                p3_health2.SetActive(false);
                p3_health1.SetActive(false);
                p3_health0.SetActive(false);
            }
            if (health == 2)
            {
                p3_health3.SetActive(false);
                p3_health2.SetActive(true);
                p3_health1.SetActive(false);
                p3_health0.SetActive(false);
            }
            if (health == 1)
            {
                p3_health3.SetActive(false);
                p3_health2.SetActive(false);
                p3_health1.SetActive(true);
                p3_health0.SetActive(false);
            }
            if (health == 0)
            {
                p3_health3.SetActive(false);
                p3_health2.SetActive(false);
                p3_health1.SetActive(false);
                p3_health0.SetActive(true);
            }
        }
    }


    // TUTORIAL
    private IEnumerator ShowTutorial()
    {
        // Show run instructions
        p0_instructions_run.SetActive(true);
        p1_instructions_run.SetActive(true);
        p2_instructions_run.SetActive(true);
        p3_instructions_run.SetActive(true);
        yield return new WaitForSeconds(12);
        p0_instructions_run.SetActive(false);
        p1_instructions_run.SetActive(false);
        p2_instructions_run.SetActive(false);
        p3_instructions_run.SetActive(false);

        // AIM
        p0_instructions_aim.SetActive(true);
        p1_instructions_aim.SetActive(true);
        p2_instructions_aim.SetActive(true);
        p3_instructions_aim.SetActive(true);
        yield return new WaitForSeconds(9);
        p0_instructions_aim.SetActive(false);
        p1_instructions_aim.SetActive(false);
        p2_instructions_aim.SetActive(false);
        p3_instructions_aim.SetActive(false);

        // SHOOT
        p0_instructions_shoot.SetActive(true);
        p0_instructions_shoot2.SetActive(true);

        p1_instructions_shoot.SetActive(true);
        p1_instructions_shoot2.SetActive(true);

        p2_instructions_shoot.SetActive(true);
        p2_instructions_shoot2.SetActive(true);

        p3_instructions_shoot.SetActive(true);
        p3_instructions_shoot2.SetActive(true);

        yield return new WaitForSeconds(9);
        p0_instructions_shoot.SetActive(false);
        p0_instructions_shoot2.SetActive(false);

        p1_instructions_shoot.SetActive(false);
        p1_instructions_shoot2.SetActive(false);

        p2_instructions_shoot.SetActive(false);
        p2_instructions_shoot2.SetActive(false);

        p3_instructions_shoot.SetActive(false);
        p3_instructions_shoot2.SetActive(false);

        // BUMP
        p0_instructions_bump.SetActive(true);
        p1_instructions_bump.SetActive(true);
        p2_instructions_bump.SetActive(true);
        p3_instructions_bump.SetActive(true);
        yield return new WaitForSeconds(9);
        p0_instructions_bump.SetActive(false);
        p1_instructions_bump.SetActive(false);
        p2_instructions_bump.SetActive(false);
        p3_instructions_bump.SetActive(false);

    }

    public void AddPlayer()
    {
        playerCount++;        
    }

    public void RemovePlayer()
    {
        playerCount--;
    }

    private void ShowJoinPrompt_P1() // HIDE PLAYER 1 PROMPT
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

    private void ShowJoinPrompt_P2() // HIDE PLAYER 2 UI
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

    private void ShowJoinPrompt_P3() // HIDE PLAYER 2 UI
    {
        p2_pressAtoJoin.SetActive(true); // SHOW JOIN PROMPT

        p2_ammoFrame.SetActive(false);
        p2_currentBomb.SetActive(false);
        p2_currentMine.SetActive(false);
        p2_currentNuke.SetActive(false);
        p2_currentTrap.SetActive(false);
        p2_nextBomb.SetActive(false);
        p2_nextMine.SetActive(false);
        p2_nextNuke.SetActive(false);
        p2_nextTrap.SetActive(false);

        p2_health3.SetActive(false);
        p2_health2.SetActive(false);
        p2_health1.SetActive(false);
        p2_health0.SetActive(false);

        p2_instructions_run.SetActive(false);
        p2_instructions_aim.SetActive(false);
        p2_instructions_shoot.SetActive(false);
        p2_instructions_shoot2.SetActive(false);
        p2_instructions_bump.SetActive(false);
    }

    private void ShowJoinPrompt_P4() // HIDE PLAYER 3 UI
    {
        p3_pressAtoJoin.SetActive(true); // SHOW JOIN PROMPT

        p3_ammoFrame.SetActive(false);
        p3_currentBomb.SetActive(false);
        p3_currentMine.SetActive(false);
        p3_currentNuke.SetActive(false);
        p3_currentTrap.SetActive(false);
        p3_nextBomb.SetActive(false);
        p3_nextMine.SetActive(false);
        p3_nextNuke.SetActive(false);
        p3_nextTrap.SetActive(false);

        p3_health3.SetActive(false);
        p3_health2.SetActive(false);
        p3_health1.SetActive(false);
        p3_health0.SetActive(false);

        p3_instructions_run.SetActive(false);
        p3_instructions_aim.SetActive(false);
        p3_instructions_shoot.SetActive(false);
        p3_instructions_shoot2.SetActive(false);
        p3_instructions_bump.SetActive(false);
    }

    public void DetectWinner() // Function detects whether there is a game winner
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
                        p0_winner.SetActive(true);  // WINNER
                        p1_loser.SetActive(true);
                        p2_loser.SetActive(true);
                        p3_loser.SetActive(true);
                    }
                    else if (i== 1) // PLAYER 2 wins
                    {
                        p0_loser.SetActive(true);
                        p1_winner.SetActive(true);  // WINNER
                        p2_loser.SetActive(true);
                        p3_loser.SetActive(true);
                    }
                    else if (i == 2) // PLAYER 3 wins
                    {
                        p1_loser.SetActive(true);
                        p0_loser.SetActive(true); 
                        p2_winner.SetActive(true);
                        p3_loser.SetActive(true);
                    }
                    else if (i == 3) // PLAYER 4 wins
                    {
                        p1_loser.SetActive(true); 
                        p0_loser.SetActive(true); 
                        p2_loser.SetActive(true);
                        p3_winner.SetActive(true); // WINNER
                    }
                }
            }
        }
    }
}