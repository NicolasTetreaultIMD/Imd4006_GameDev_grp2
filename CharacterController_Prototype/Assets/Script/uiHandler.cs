using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.XR;
using UnityEngine.VFX;
using System.Runtime.InteropServices.WindowsRuntime;


public class uiHandler : MonoBehaviour
{
    public GameObject[] players = new GameObject[4];
    [Header("Frames")]
    public GameObject p0_ammoFrame;
    public GameObject p1_ammoFrame;
    public GameObject p2_ammoFrame;
    public GameObject p3_ammoFrame;

    [Header("Waiting Room UI")]
    public List<GameObject> waitingPlayersUI = new List<GameObject>();
    public List<GameObject> readyPlayersUI = new List<GameObject>();

    [Header("controls UI")]
    public List<GameObject> playerControlsUI = new List<GameObject>();

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

    public GameObject p0_pressAtoJoin;
    public GameObject p1_pressAtoJoin;
    public GameObject p2_pressAtoJoin;
    public GameObject p3_pressAtoJoin;

    [Header("Bump UI")]
    public GameObject[] player_bump_disabled = new GameObject [4];

    [Header("Accelerate Prompt")]
    public GameObject[] player_accelerate = new GameObject [4];
    Vector3 originalScaleAccUI;

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
    private bool gameStart;
    public GameObject gameOverScreen;
    public int playersAliveCount;

    public bool ifShowMenuScreen;



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

        originalScaleAccUI = player_accelerate[0].transform.localScale;

        StartCoroutine(ScalePrompt(player_accelerate[0].transform, 0));
        StartCoroutine(ScalePrompt(player_accelerate[1].transform, 1));
        StartCoroutine(ScalePrompt(player_accelerate[2].transform, 2));
        StartCoroutine(ScalePrompt(player_accelerate[3].transform, 3));

        countdownStart = false;
        playerCount = 0;
        tutorialFlag = false; // not yet seen tutorial prompts
        p0_winner.SetActive(false);
        p1_winner.SetActive(false);
        p2_winner.SetActive(false);
        p3_winner.SetActive(false);
        awaitingPlayers.SetActive(true);
        ShowJoinPrompt_P1(true);
        ShowJoinPrompt_P2(true);
        ShowJoinPrompt_P3(true);
        ShowJoinPrompt_P4(true);


        gameOverScreen.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        // SHOW UI dependent on player count
        if (playerCount == 0)
        {
            ShowJoinPrompt_P1(true);
            ShowJoinPrompt_P2(true);
            ShowJoinPrompt_P3(true);
            ShowJoinPrompt_P4(true);
        }

        if (playerCount == 1)
        {
            p0_ammoFrame.SetActive(true);
            p0_pressAtoJoin.SetActive(false); // hide press A to join

            // SHOW JOIN PROMPTS FOR REMAINING PLAYERS
            ShowJoinPrompt_P2(true);
            ShowJoinPrompt_P3(true);
            ShowJoinPrompt_P4(true);

            HandleReadyUI(1);

            players[0].gameObject.GetComponent<CarController>().health = 3; // Players will always have max health
        }

        if (playerCount == 2)
        {

            p1_ammoFrame.SetActive(true);
            p1_pressAtoJoin.SetActive(false); // hide press A to join
            ShowJoinPrompt_P3(true);
            ShowJoinPrompt_P4(true);

            HandleReadyUI(2);

            players[0].gameObject.GetComponent<CarController>().health = 3; // Players will always have max health while waiting for players to join
            players[1].gameObject.GetComponent<CarController>().health = 3; // Players will always have max health while waiting for players to join
        }

        if (playerCount == 3)
        {
            p2_ammoFrame.SetActive(true);
            p2_pressAtoJoin.SetActive(false); // hide press A to join
            ShowJoinPrompt_P4(true);

            HandleReadyUI(3);

            players[0].gameObject.GetComponent<CarController>().health = 3; // Players will always have max health while waiting for players to join
            players[1].gameObject.GetComponent<CarController>().health = 3; // Players will always have max health while waiting for players to join
            players[2].gameObject.GetComponent<CarController>().health = 3; // Players will always have max health while waiting for players to join
        }

        if (playerCount == 4)
        {
            p3_pressAtoJoin.SetActive(false); // hide press A to join
            // Show tutorial when all players are in the lobby

            HandleReadyUI(4);

            if (!gameStart)
            {
                p3_ammoFrame.SetActive(true);
                players[0].gameObject.GetComponent<CarController>().health = 3; // Players will always have max health while waiting for players to join
                players[1].gameObject.GetComponent<CarController>().health = 3; // Players will always have max health while waiting for players to join
                players[2].gameObject.GetComponent<CarController>().health = 3; // Players will always have max health while waiting for players to join
                players[3].gameObject.GetComponent<CarController>().health = 3; // Players will always have max health while waiting for players to join
            }

            if (!countdownStart)
            {
                if (players[0].gameObject.GetComponent<CarController>().playerIsReady &&
                    players[1].gameObject.GetComponent<CarController>().playerIsReady &&
                    players[2].gameObject.GetComponent<CarController>().playerIsReady &&
                    players[3].gameObject.GetComponent<CarController>().playerIsReady)
                {
                    PlayersReady(); // REMOVE WAITING SCREEN AND START COUNTDOWN
                    countdownStart = true;
                    gameStart = true;
                }
            }

            /*if (!countdownStart)
            {
                PlayersReady(); // REMOVE WAITING SCREEN AND START COUNTDOWN
                countdownStart = true;
            }*/
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
        if (playerCount == 4)
        {
            DetectWinner();
        }

        // PLAYER LOOP
        for (int i = 0; i < playerCount; i++)
        {
            //Show prompt to PRESS A to accelerate.Scale it up and down
            if (players[i].GetComponent<CarController>().cartState == CarController.CartState.Running && players[i].GetComponent<CarController>().health > 0) // If cart is in running state, prompt the player to press A
            {
                // SHOW PROMPT TO TAP A to move
                player_accelerate[i].SetActive(true);
                //StartCoroutine(ScalePrompt(player_accelerate[i].transform, i)); // scale it up and down once

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

    private void HandleReadyUI(int playerCount)
    {
        for (int i = 0; i < playerCount; i++)
        {
            if (players[i].GetComponent<CarController>().playerIsReady)
            {
                waitingPlayersUI[i].SetActive(false);
                readyPlayersUI[i].SetActive(true);
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
        GameObject[] itemClear1 = GameObject.FindGameObjectsWithTag("Bomb");
        GameObject[] itemClear2 = GameObject.FindGameObjectsWithTag("Mine");
        GameObject[] itemClear3 = GameObject.FindGameObjectsWithTag("Nuke");
        GameObject[] itemClear4 = GameObject.FindGameObjectsWithTag("Trap");

        foreach (GameObject bombs in itemClear1)
        {
            Destroy(bombs);
        }

        foreach (GameObject mines in itemClear2)
        {
            Destroy(mines);
        }

        foreach (GameObject nukes in itemClear3)
        {
            Destroy(nukes);
        }

        foreach (GameObject traps in itemClear4)
        {
            Destroy(traps);
        }

        players[0].GetComponentInChildren<Cannon>().projectile.Clear();
        players[1].GetComponentInChildren<Cannon>().projectile.Clear();
        players[2].GetComponentInChildren<Cannon>().projectile.Clear();
        players[3].GetComponentInChildren<Cannon>().projectile.Clear();

        // Reset items from the UI
        ShowCurrentAmmoType(0, "Empty");
        ShowCurrentAmmoType(1, "Empty");
        ShowCurrentAmmoType(2, "Empty");
        ShowCurrentAmmoType(3, "Empty");

        ShowNextAmmoType(0, "Empty");
        ShowNextAmmoType(1, "Empty");
        ShowNextAmmoType(2, "Empty");
        ShowNextAmmoType(3, "Empty");

        players[0].GetComponent<CarController>().canMove = false;
        players[1].GetComponent<CarController>().canMove = false;
        players[2].GetComponent<CarController>().canMove = false;
        players[3].GetComponent<CarController>().canMove = false;

        players[0].GetComponent<CarController>().speed = 0;
        players[1].GetComponent<CarController>().speed = 0;
        players[2].GetComponent<CarController>().speed = 0;
        players[3].GetComponent<CarController>().speed = 0;

        players[0].gameObject.transform.position = GameObject.Find("Player1Spawn").transform.position;
        players[1].gameObject.transform.position = GameObject.Find("Player2Spawn").transform.position;
        players[2].gameObject.transform.position = GameObject.Find("Player3Spawn").transform.position;
        players[3].gameObject.transform.position = GameObject.Find("Player4Spawn").transform.position;


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
        promptTransform.localScale = originalScaleAccUI;
        Vector3 curScale = promptTransform.localScale;
        Vector3 targetScale = originalScaleAccUI * scaleAmount;

        // Scale up
        yield return LerpScale(promptTransform, curScale, targetScale, scaleDuration / 2);

        // Scale down
        yield return LerpScale(promptTransform, targetScale, curScale, scaleDuration / 2);

        StartCoroutine(ScalePrompt(promptTransform, index));
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
            nuke.SetActive(type == "Nuke Item V2");
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
            nuke.SetActive(type == "Nuke Item V2");
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
        else if (playerID == 3) // SECOND PLAYER
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
                p0_loser.SetActive(true);
                ShowJoinPrompt_P1(false);
                playerControlsUI[0].SetActive(false);
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
                p1_loser.SetActive(true);
                ShowJoinPrompt_P2(false);
                playerControlsUI[1].SetActive(false);

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
                p2_loser.SetActive(true);
                ShowJoinPrompt_P3(false);
                playerControlsUI[2].SetActive(false);

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
                p3_loser.SetActive(true);
                ShowJoinPrompt_P4(false);
                playerControlsUI[3].SetActive(false);

            }
        }
    }

    public void AddPlayer()
    {
        playerCount++;        
    }

    public void RemovePlayer()
    {
        playerCount--;
    }

    private void ShowJoinPrompt_P1(bool pressToJoin) // HIDE PLAYER 1 PROMPT
    {
        p0_pressAtoJoin.SetActive(pressToJoin); // SHOW JOIN PROMPT

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


    }

    private void ShowJoinPrompt_P2(bool pressToJoin) // HIDE PLAYER 2 UI
    {
        p1_pressAtoJoin.SetActive(pressToJoin); // SHOW JOIN PROMPT

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
    }

    private void ShowJoinPrompt_P3(bool pressToJoin) // HIDE PLAYER 2 UI
    {
        p2_pressAtoJoin.SetActive(pressToJoin); // SHOW JOIN PROMPT

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
    }

    private void ShowJoinPrompt_P4(bool pressToJoin) // HIDE PLAYER 3 UI
    {
        p3_pressAtoJoin.SetActive(pressToJoin); // SHOW JOIN PROMPT

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
    }

    public void DetectWinner() // Function detects whether there is a game winner
    {
        playersAliveCount = 0;
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
            if (!ifShowMenuScreen)
            {
                StartCoroutine(GameOverScreen());
            }

        }
    }

    private IEnumerator GameOverScreen()
    {
        ifShowMenuScreen = true;
        yield return new WaitForSeconds(1.5f);
        gameOverScreen.SetActive(true); //turn on game over UIs
    }
}