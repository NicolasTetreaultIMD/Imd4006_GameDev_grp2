using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleProximityPrompt : MonoBehaviour
{
    public GameObject player;         // Reference to the player GameObject
    public float proximityThreshold = 20f;  // Distance threshold for proximity

    private GameObject[] poles;       // Array to store all the poles in the scene
    private GameObject promptText;

    void Start()
    {
        // Find all the pole objects in the scene by tag
        poles = GameObject.FindGameObjectsWithTag("Pole");
    }

    void Update()
    {
        // Loop through all poles and check the distance to the player
        foreach (GameObject pole in poles)
        {
            // Calculate the distance from the player to the pole
            float distance = Vector3.Distance(player.transform.position, pole.transform.position);

            if (distance < proximityThreshold)
            {
                // If the player is close enough to the pole, show the prompt
                ShowPrompt(pole);
                Debug.Log("CLOSE!");
                return; // Exit the loop after finding the first pole within range (optional, can remove if you want to check all poles)
            }

        }
    }

    void ShowPrompt(GameObject pole)
    {
        // Show the prompt above the pole
        foreach (Transform children in pole.transform.GetComponentsInChildren<Transform>())
        {
            if (children.gameObject.tag == "ButtonPrompt")
            {
                promptText = children.gameObject;
            }
        }
        promptText.gameObject.SetActive(false);
    }
}
