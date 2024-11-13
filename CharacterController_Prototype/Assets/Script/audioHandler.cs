using System;
using System.Collections;
using System.Collections.Generic;
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
        //source[1].pitch = UnityEngine.Random.Range(0.75f, 1.25f);
        //source[1].Play();
    }

    public void wheelAudioEffect()
    {
        if (speed >= 20f)
        {
            //if (Math.Abs(carController.leftStick.x) > 0.05f)
            //{
            //    source[3].pitch = 1;
            //    if (source[3].isPlaying == false)
            //    {
            //        source[3].Play();
            //    }
            //}
            //else
            //{
            //    source[3].Stop();
            //}
        }
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


}
