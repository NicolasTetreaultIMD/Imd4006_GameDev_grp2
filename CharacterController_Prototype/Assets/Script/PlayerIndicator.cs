using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PoleProximityPrompt : MonoBehaviour
{
    public float proximityThreshold = 20f;  // Distance threshold for proximity

    public CarController carController;
    private GameObject promptText;

    public GameObject[] playerIndicators;       // Array to store all the indicators
    public List<PlayerInput> players;

    public bool showPlayer1;
    public bool showPlayer2;
    public bool showPlayer3;
    public bool showPlayer4;


    void Start()
    {
        players = GameObject.Find("MultiplayerManager").GetComponent<MultiplayerManager>().getPlayers();

        // Find all the pole objects in the scene by tag
        if(carController.playerId == 0)
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

    void Update()
    {
        //foreach (var player in players)
        //{
        //    if (carController.playerId != player.playerIndex)
        //    {
        //        float distance = Vector3.Distance();
        //    }
        //}

        //float distance = Vector3.Distance();

        //if (distance > proximityThreshold)
        //{
        //    If the player is close enough to the pole, show the prompt
        //    ShowPlayerIndicators();
        //    Debug.Log("OUTSIDE OF DISTANCE!");
        //    return; // Exit the loop after finding the first pole within range (optional, can remove if you want to check all poles)
        //}
    }
}
