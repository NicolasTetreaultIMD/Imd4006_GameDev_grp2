using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleProximityPrompt : MonoBehaviour
{
    public float proximityThreshold = 20f;  // Distance threshold for proximity

    public CarController carController;
    private GameObject promptText;

    public GameObject[] playerIndicators;       // Array to store all the poles in the scene


    void Start()
    {
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

        // Compare the distances of each player relative to each other
        //foreach (GameObject players in players)
        //{
        //    // Calculate the distance from the player to the pole
        //    //float distance = Vector3.Distance(players[0].transform.position, players[1].transform.position);

        //    if (distance > proximityThreshold)
        //    {
        //        // If the player is close enough to the pole, show the prompt
        //        //ShowPlayerIndicators();
        //        Debug.Log("OUTSIDE OF DISTANCE!");
        //        return; // Exit the loop after finding the first pole within range (optional, can remove if you want to check all poles)
        //    }

        //}
    }
}
