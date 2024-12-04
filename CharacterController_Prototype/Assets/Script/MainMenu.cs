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

    public uiHandler uiHandler;

    public void startGame()
    {
        SceneManager.LoadScene(1);
    }

    public void restartGame()
    {
        //Reset position
        //Reset health
        //Reset 
        uiHandler.ifShowMenuScreen = false;

        uiHandler.players[0].GetComponent<CarController>().health = 3;
        uiHandler.players[1].GetComponent<CarController>().health = 3;
        uiHandler.players[2].GetComponent<CarController>().health = 3;
        uiHandler.players[3].GetComponent<CarController>().health = 3;

        uiHandler.playerAliveIndex[0] = true;
        uiHandler.playerAliveIndex[1] = true;
        uiHandler.playerAliveIndex[2] = true;
        uiHandler.playerAliveIndex[3] = true;

        uiHandler.players[0].GetComponent<DamageHandler>().isStunned = false;
        uiHandler.players[1].GetComponent<DamageHandler>().isStunned = false;
        uiHandler.players[2].GetComponent<DamageHandler>().isStunned = false;
        uiHandler.players[3].GetComponent<DamageHandler>().isStunned = false;

        uiHandler.players[0].GetComponent<DamageHandler>().isImmune = false;
        uiHandler.players[1].GetComponent<DamageHandler>().isImmune = false;
        uiHandler.players[2].GetComponent<DamageHandler>().isImmune = false;
        uiHandler.players[3].GetComponent<DamageHandler>().isImmune = false;

        uiHandler.gameOverScreen.SetActive(false);
        uiHandler.p0_winner.SetActive(false);
        uiHandler.p0_loser.SetActive(false);
        uiHandler.p1_winner.SetActive(false);
        uiHandler.p1_loser.SetActive(false);
        uiHandler.p2_winner.SetActive(false);
        uiHandler.p2_loser.SetActive(false);
        uiHandler.p3_winner.SetActive(false);
        uiHandler.p3_loser.SetActive(false);

        uiHandler.players[0].GetComponent<CarController>().characterHider.ShowObjects();
        uiHandler.players[1].GetComponent<CarController>().characterHider.ShowObjects();
        uiHandler.players[2].GetComponent<CarController>().characterHider.ShowObjects();
        uiHandler.players[3].GetComponent<CarController>().characterHider.ShowObjects();

        uiHandler.DetectWinner();
        uiHandler.PlayersReady();

        uiHandler.playerControlsUI[0].SetActive(true);
        uiHandler.playerControlsUI[1].SetActive(true);
        uiHandler.playerControlsUI[2].SetActive(true);
        uiHandler.playerControlsUI[3].SetActive(true);

        uiHandler.p0_ammoFrame.SetActive(true);
        uiHandler.p1_ammoFrame.SetActive(true);
        uiHandler.p2_ammoFrame.SetActive(true);
        uiHandler.p3_ammoFrame.SetActive(true);
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
        if (explosion != null)
        {
            explosion.Rotate(Vector3.forward, -10 * Time.deltaTime);
        }
    }

}
