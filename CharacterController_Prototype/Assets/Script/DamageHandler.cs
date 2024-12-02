using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    [Header("Screen Shake")]
    public CinemachineVirtualCamera cinemachine;
    public CameraManager camManager;

    public Material testMaterial;
    public Material currentMaterial;

    public CarController carController;
    public int playerId;

    public uiHandler uiHandler;

    public bool isProjectile;
    public bool isImmune;
    public bool isStunned;

    public GameObject shield;
    public Material shieldTerial;
    float lerpDuration;
    float lerpStartTime;


    // Start is called before the first frame update
    void Start()
    {

        uiHandler = GameObject.Find("Canvas").GetComponent<uiHandler>();
        carController = GetComponent<CarController>();
        isStunned = false;

        shield.gameObject.SetActive(false);
        shieldTerial = shield.GetComponent<Renderer>().material;

        lerpDuration = 0.5f; 
        lerpStartTime = Time.time;
    }

 

    // Update is called once per frame
    void Update()
    {
        if (carController != null)
        {
            playerId = carController.playerId;
        }

        if(isStunned)
        {
            carController.speed = 0;

        }

        if (isImmune) // Keep looping the alpha lerp back and forth
        {
            float lerpTime = (Time.time - lerpStartTime) / lerpDuration;
            float alpha = Mathf.Lerp(20f, 120f, Mathf.PingPong(lerpTime, 1));  // PingPong will oscillate between 0 and 1

            Color newColor = shieldTerial.color;
            newColor.a = alpha / 255f;
            shieldTerial.color = newColor;
        }
    }

    public void Hit(int explosivePlayerId)
    {

        if (isImmune == false)
        {
            if (gameObject.tag == "Player" && playerId != explosivePlayerId)
            {
                carController.health--;
                carController.speed = 0;
                carController.haptics.ExplosionHaptics();




                if (carController.health <= 0)
                {
                    isImmune = true;
                    uiHandler.playerAliveIndex[playerId] = false;
                    shield.SetActive(true);
                    isStunned = true;
                }
                else
                {
                    //screenShake
                    cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = camManager.amplitudeChange + camManager.maxAmplitude * 2;
                    cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = camManager.frequencyChange + camManager.maxFrequency * 2;

                    isImmune = true;
                    StartCoroutine(ImmunityTime());
                    shield.SetActive(true);

                    //PLAYER IS HIT - PLAY SFX
                    carController.audioHandler.hitPlayer();

                }
            }
        }
    }

    public void Stun(int explosivePlayerId) 
    {
        if (gameObject.tag == "Player" && playerId != explosivePlayerId)
        {
            //screenShake
            cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = camManager.amplitudeChange + camManager.maxAmplitude;
            cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = camManager.frequencyChange + camManager.maxFrequency;

            isStunned = true;
            StartCoroutine(StunDuration());
        }
    }

    private IEnumerator StunDuration()
    {
        yield return new WaitForSeconds(3f);
        isStunned = false;
    }

    private IEnumerator ImmunityTime()
    {
        yield return new WaitForSeconds(4f);
        isImmune = false;
        shield.SetActive(false);
    }
}
