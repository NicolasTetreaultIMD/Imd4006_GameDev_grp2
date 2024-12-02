using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class NewBehaviourScript : MonoBehaviour
{

    public AudioMixer audioMixer;

    public Transform explosion; // Assign your sprite's Transform here in the Inspector
    public float rotateSpeed = 360f; // Degrees per second

    public void startGame()
    {
        SceneManager.LoadScene(1);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void setVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    void Update()
    {
        //rotate explosion
        explosion.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
    }

}
