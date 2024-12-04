using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerIndicators : MonoBehaviour
{
    public float proximityThreshold = 20f;  // Distance threshold for proximity

    public CarController carController;
    private GameObject promptText;

    public GameObject[] playerIndicators;       // Array to store all the indicators

    public GameObject[] playerPlaneIndicators;  // Array store the plane indicators showing player's direction

    public List<PlayerInput> players;

    public bool showPlayer1;
    public bool showPlayer2;
    public bool showPlayer3;
    public bool showPlayer4;


    void Start()
    {
        players = GameObject.Find("MultiplayerManager").GetComponent<MultiplayerManager>().getPlayers();

        // ARROW INDICATORS ABOVE THE PLAYERS
        if (carController.playerId == 0)
        {
            playerIndicators[0].SetActive(true);
            playerIndicators[1].SetActive(false);
            playerIndicators[2].SetActive(false);
            playerIndicators[3].SetActive(false);
        }

        if (carController.playerId == 1)
        {
            playerIndicators[0].SetActive(false);
            playerIndicators[1].SetActive(true);
            playerIndicators[2].SetActive(false);
            playerIndicators[3].SetActive(false);
        }

        if (carController.playerId == 2)
        {
            playerIndicators[0].SetActive(false);
            playerIndicators[1].SetActive(false);
            playerIndicators[2].SetActive(true);
            playerIndicators[3].SetActive(false);
        }

        if (carController.playerId == 3)
        {
            playerIndicators[0].SetActive(false);
            playerIndicators[1].SetActive(false);
            playerIndicators[2].SetActive(false);
            playerIndicators[3].SetActive(true);
        }

    }

    private void Update()
    {
        if (players.Count < 4)
        {
            players = GameObject.Find("MultiplayerManager").GetComponent<MultiplayerManager>().getPlayers();
        }

        // Track the position of each player relative to one another
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].playerIndex != carController.playerId)
            {
                if (playerPlaneIndicators[i].activeSelf == false)
                {
                    playerPlaneIndicators[i].SetActive(true);
                }
                playerPlaneIndicators[i].transform.LookAt(players[i].gameObject.transform);
            }

            // HIDE player indicators for all dead players
            if (players[i].GetComponent<CarController>().health == 0)
            {
                playerPlaneIndicators[i].SetActive(false);
                // players[i].gameObject.transform.root.Find("PlayerPlaneIndicatorParent").gameObject.SetActive(false);
            }

        }
    }
}
