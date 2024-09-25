using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class audioHandler : MonoBehaviour
{
    public CarController carController;
    public armController armController;

    public AudioSource[] source;

    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        speed = carController.speed;
        wheelAudioEffect();
        cartMovement();
    }


    //Play step audio. Random range 
    public void stepAudioEffect()
    {
        source[1].pitch = UnityEngine.Random.Range(0.75f, 1.25f);
        source[1].Play();
    }

    public void wheelAudioEffect()
    {
        if (speed >= 20f)
        {
            if (Math.Abs(carController.leftStick.x) > 0.05f)
            {
                source[3].pitch = 1;
                if (source[3].isPlaying == false)
                {
                    source[3].Play();
                }
            }
            else
            {
                source[3].Stop();
            }
        }
    }

    public void cartMovement()
    {
        if(carController.speed > 0)
        {
            source[0].pitch = carController.speed/90;
            if (source[0].isPlaying == false)
            {
                source[0].Play();
            }
        }
        else
        {
            source[0].Stop();
        }
    }

    public void grabItem()
    {
        if (source[2].isPlaying == false)
        {
            source[2].Play();
        }
    }

    public void cartCrash()
    {
        if (source[4].isPlaying == false)
        {
            source[4].Play();
        }
    }
    
}
