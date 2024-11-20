using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleProximityPrompt : MonoBehaviour
{
    public GameObject player;         // Reference to the player GameObject
    public float proximityThreshold = 10f;  // Distance threshold for proximity
    public GameObject promptText;           // The component that shows the prompt

    private GameObject[] poles;       // Array to store all the poles in the scene

    void Start()
    {
        // Hide the prompt at the start
        promptText.gameObject.SetActive(false);

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

        // If no pole is close enough, hide the prompt
        HidePrompt();
    }

    void ShowPrompt(GameObject pole)
    {
        // Show the prompt above the pole
        promptText.gameObject.SetActive(true);

        // Convert the world position of the pole to a screen position
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(pole.transform.position + Vector3.up * 2); // Adjust for height above the pole
        promptText.transform.position = screenPosition;
    }

    void HidePrompt()
    {
        // Hide the prompt when no pole is close enough
        promptText.gameObject.SetActive(false);
    }
}
