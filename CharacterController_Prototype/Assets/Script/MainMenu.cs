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
    public Transform explosion;

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
        //set volume slider as first button
        EventSystem.current.SetSelectedGameObject(volumeSlider);
    }

    public void playButtonSelect()
    {
        //set first button to playButton
        EventSystem.current.SetSelectedGameObject(playButton);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
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
