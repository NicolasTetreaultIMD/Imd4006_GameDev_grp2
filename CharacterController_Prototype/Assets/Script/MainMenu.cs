using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class NewBehaviourScript : MonoBehaviour
{

    public AudioMixer audioMixer;
    
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

}
