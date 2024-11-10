using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class vfxHandler : MonoBehaviour
{
    public CenterMassManager CenterMassManager;
    public CarController cartController; // This is the master car obj

    public TrailRenderer[] windTrails; // Wind Trails
    public TrailRenderer[] tireTrails; // Tire trails on the ground
    public VisualEffect leftTireSmoke;
    public VisualEffect rightTireSmoke;
    public ParticleSystem[] particleSmokeEffect; //particle smoke when spinning around a pole
    public ParticleSystem[] sparks; // sparks while spinning around pole
    public ParticleSystem[] fireTrail; // fire trail when releasing from pole
    public ParticleSystem grabEffect;


    private float speed;
    private float minTime = 0.0005f; // min/maxTime is used for Trail renderer for wind trails ( VFX )
    private float maxTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0;
        ToggleSparks(false);
        ToggleVolumetricSmoke(3); // both trails off
        ToggleParticleSmoke(false);
        ToggleTireTrails(0); // Both trails on
    }

    // Update is called once per frame
    void Update()
    {
        speed = cartController.speed;
        

        // SHOW wind lines when player speed is greater than 20
        if (speed >= 20)
        {
            // LERP the TrailRenderer.Time to ease the trail in when it appears
            float time = Mathf.Lerp(minTime, maxTime, Mathf.InverseLerp(30.0f, 40.0f, speed));
            for (int i = 0; i < windTrails.Length; i++)
            {
                windTrails[i].time = time;
            }

            ToggleTrailEffect(true); // show wind trails

        }
        else // Hide wind lines when speed < 20
        {
            ToggleTrailEffect(false);
        }

       
        if (cartController.speed > 0) // IF cart is MOVING
        {
            // Check TILT angle and toggle tire smokes/trails
            if (CenterMassManager.massCenter.x > 0) // RIGHT wheels are ON the ground
            {
                ToggleVolumetricSmoke(2); // Show RIGHT tire smoke
                ToggleTireTrails(2); // Show RIGHT tire trails
            }

            else if (CenterMassManager.massCenter.x < 0)
            {
                ToggleVolumetricSmoke(1); // Show RIGHT tire smoke
                ToggleTireTrails(1); // Show RIGHT tire trails
            }
            else // BOTH wheels are touching the ground
            {
                ToggleVolumetricSmoke(0); // Show BOTH tire smokes
                ToggleTireTrails(0); // Show BOTH tire trails
            }
        }
        else
        {
            ToggleVolumetricSmoke(3); // none
        }

        // SWITCH smokes depending on states
        // Volumetric smoke if - Player is running / sitting in cart
        if (cartController.cartState == CarController.CartState.Running || cartController.cartState == CarController.CartState.InCart)
        {
            ToggleSparks(false);
            ToggleParticleSmoke(false);
        }
        // Particle smoke if - Player is grabing pole
        else if(cartController.cartState == CarController.CartState.PoleHolding)
        {
            ToggleSparks(true);
            ToggleParticleSmoke(true);
            ToggleVolumetricSmoke(3); // none
        }
    }

    // Wind Trail Effect
    public void ToggleTrailEffect (bool value)
    {
        for (int i = 0;i < windTrails.Length;i++) { windTrails[i].enabled = value; }
    }

    // Toggle 3D tire smoke
    public void ToggleVolumetricSmoke (int value)
    {
        // VALUES for VOLUMETRIC SMOKE
        // ----------------------------
        // 0 - Toggle BOTH on
        // 1 - LEFT only
        // 2 - RIGHT only
        // 3 - none
        // ----------------------------

        switch (value)
        {
            case 0: // Toggle BOTH on
                leftTireSmoke.enabled = true;
                rightTireSmoke.enabled = true;
                break;

            case 1: // Show LEFT only
                leftTireSmoke.enabled = true;
                rightTireSmoke.enabled = false;
                break;

            case 2: // Show RIGHT only
                leftTireSmoke.enabled = false;
                rightTireSmoke.enabled = true;
                break; 

            case 3: // Show NONE
                leftTireSmoke.enabled = false;
                rightTireSmoke.enabled = false;
                break;

            default:
                // Nothing
                leftTireSmoke.enabled = false;
                rightTireSmoke.enabled = false;
                break;
        }
        
    }

    // Toggle particle smoke (in world space) 
    public void ToggleParticleSmoke(bool value)
    {
        if (value) // If player is holding the pole
        {
            for (int i = 0; i < particleSmokeEffect.Length; i++)
            {
                particleSmokeEffect[i].Play(); // Play particle effect
            }
        }
        else // Player released
        {
            for (int i = 0; i < particleSmokeEffect.Length; i++)
            {
                particleSmokeEffect[i].Stop();
            }
        }

    }

    // Toggle Sparks (in world space) 
    public void ToggleSparks(bool value)
    {
        if (value) // If player is holding the pole
        {
            for (int i = 0; i < sparks.Length; i++)
            {
                sparks[i].gameObject.SetActive(true); // Play particle effect
            }
        }
        else // Player released
        {
            for (int i = 0; i < sparks.Length; i++)
            {
                sparks[i].gameObject.SetActive(false);
            }
        }

    }

    // Toggle Fire Trail
    public void PlayFireTrail()
    {
        for (int i = 0; i < fireTrail.Length; i++)
        {
            fireTrail[i].Play(); // Play fire trail effect
        }

    }

    // Toggle trails left behind each wheel of the car
    public void ToggleTireTrails(int value)
    {
        switch (value)
        {
            case 0: // Both wheels are touching - show ALL trails
                for (int i = 0; i < tireTrails.Length; i++) { tireTrails[i].enabled = true; }
                break;

            case 1: // LEFT side is touching the ground
                tireTrails[0].enabled = true;
                tireTrails[1].enabled = true;
                tireTrails[2].enabled = false;
                tireTrails[3].enabled = false;
                break;

            case 2: // RIGHT side is touching the ground
                tireTrails[0].enabled = false;
                tireTrails[1].enabled = false;
                tireTrails[2].enabled = true;
                tireTrails[3].enabled = true;
                break;

            default:
                break;
        }
    }

    // GRAB effect
    public void PlayGrabEffect()
    {   
       // if(!grabEffect.isPlaying)
        //{
        grabEffect.Play();
        Debug.Log("GRABBED");

        // }
    }
}
