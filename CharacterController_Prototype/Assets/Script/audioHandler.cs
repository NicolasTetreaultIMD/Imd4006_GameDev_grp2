using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class audioHandler : MonoBehaviour
{
    public CarController carController;
    //public armController armController;

    public AudioSource[] source;

    // Source   [0] - Background Arena Music (Plays automatically on awake)
    //          [1] - Engine Running (vehicle is moving)
    //          [2] - Cart Crashing (When the player hits another player or a wall)
    //          [3] - Item Pickup (collect a projectile)
    //          [4] - Tire Squeal (after releasing from a pole)
    //          [5] - cannon shoot 
    //          [6] - explosion
    //          [7] - drift
    //          [8] - success (player hits a target)
    //          [9] - bump
    //          [10] - Trap
    //          [11] - Mine
    //          [12] - Nuke
    //          [13] - Footsteps
    //          [14] - Ouch (Getting hit)

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
        cartMovement();
    }


    //Play step audio. Random range 
    public void stepAudioEffect()
    {
        source[13].pitch = UnityEngine.Random.Range(0.75f, 1.25f);
        source[13].Play();
    }


    // Vehicle is moving
    public void cartMovement()
    {
        if(carController.speed > 0)
        {
            source[1].pitch = carController.speed / 90;
            if (source[1].pitch < 0.75) source[1].pitch = (float)0.75; // Create a min pitch value 
            if (source[1].isPlaying == false)
            {
                source[1].Play();
            }
        }
        else
        {
            source[1].Stop();
        }
    }

    public void PickupItem()
    {
        if (source[3].isPlaying == false)
        {
            source[3].Play();
        }
    }

    public void cartCrash()
    {
        if (source[2].isPlaying == false)
        {
            source[2].Play();
        }
    }

    // Tire squeal after releasing a pole
    public void poleRelease()
    {
        if (source[4].isPlaying == false)
        {
            source[4].Play();
        }
    }

    public void ShootItem()
    {
        if (source[5].isPlaying == false)
        {
            source[5].Play();
        }
    }

    public void impactExplosion()
    {
        source[6].Play();

    }

    public void carDrift()
    {

        if (source[7].isPlaying == false)
        {
            source[7].Play();
        }
    }

    public void HitTarget()
    {
        if (source[8].isPlaying == false)
        {
            source[8].Play();
        }
    }


    private float currentPitch = 0.8f; 
    private bool increasing = true;   //if the pitch should increase or decrease

    public void carBump()
    {
        if (source[9].isPlaying == false)
        {
            source[9].pitch = currentPitch;

            //change audio pitch
            if (increasing)
            {
                currentPitch += 0.5f; //increase pitch by this value
                if (currentPitch >= 2f) // Check upper bound
                {
                    currentPitch = 2f;
                    increasing = false; //make pitch decrease
                }
            }
            else
            {
                currentPitch -= 0.5f; //decrease pitch
                if (currentPitch <= 0.5f) 
                {
                    currentPitch = 0.5f;
                    increasing = true; //pitch increase
                }
            }

            source[9].Play();
        }
    }

    public void activateTrap()
    {
        // Play bear trap noise
        if (source[10].isPlaying == false)
        {
            source[10].Play();
        }
    }

    public void mineBeep()
    {
        if (source[11].isPlaying == false)
        {
            source[11].Play();
        }
    } 
    public void nukeWhistle()
    {
        if (source[12].isPlaying == false)
        {
            source[12].Play();
        }
    }

    public void hitPlayer()
    {
        if(source[14].isPlaying == false)
        { 
            source[14].Play();
            Debug.Log("Hit!");
        }
    }

}
