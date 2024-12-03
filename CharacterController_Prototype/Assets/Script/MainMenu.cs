using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class NewBehaviourScript : MonoBehaviour
{

    public AudioMixer audioMixer;

    public GameObject volumeSlider;
    public GameObject playButton;
    public Transform explosion; // Assign your sprite's Transform here in the Inspector

    public void startGame()
    {
        SceneManager.LoadScene(1);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void optionsMenu()
    {
        // Set the first selectable UI element (the slider) in the options menu
        EventSystem.current.SetSelectedGameObject(volumeSlider);
    }

    public void mainMenu()
    {
        // Set the first selectable UI element (the slider) in the options menu
        EventSystem.current.SetSelectedGameObject(playButton);
    }

    public void setVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }


    void Update()
    {
        //rotate explosion
        explosion.Rotate(Vector3.forward, -10 * Time.deltaTime);
    }

}
