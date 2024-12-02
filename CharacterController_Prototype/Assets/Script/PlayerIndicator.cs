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
}
