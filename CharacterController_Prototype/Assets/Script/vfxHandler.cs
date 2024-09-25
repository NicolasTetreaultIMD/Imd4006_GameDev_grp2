using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class vfxHandler : MonoBehaviour
{

    public CarController cartController;
    public armController armController;

    private float speed;

    // Trails
    public TrailRenderer topLeft_TRL;
    public TrailRenderer topRight_TRL;
    public TrailRenderer botLeft_TRL;
    public TrailRenderer botRight_TRL;

    public VisualEffect leftTireSmoke;
    public VisualEffect rightTireSmoke;

    public ParticleSystem[] smokeEffect;

    public ParticleSystem[] sparks;

    // minTime is used for Trail renderer for wind trails ( VFX )
    private float minTime = 0.0005f;
    private float maxTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0;

        ToggleVolumetricSmoke(true);
        ToggleParticleSmoke(true);
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

            topLeft_TRL.time = time;
            topRight_TRL.time = time;
            botLeft_TRL.time = time;
            botRight_TRL.time = time;

            ToggleTrailEffect(true);

        }
        else // no wind lines when speed < 20
        {
            ToggleTrailEffect(false);
        }


        // SWITCH smokes depending on states
        // If player is running or moving in cart, show volumetric smoke
        if (cartController.cartState == CarController.CartState.Running || cartController.cartState == CarController.CartState.InCart)
        {
            ToggleParticleSmoke(false);
            ToggleVolumetricSmoke(true);
            ToggleSparks(false);
        }
        // Player is grabbing a pole, show particle smoke
        else if(cartController.cartState == CarController.CartState.PoleHolding)
        {
            ToggleParticleSmoke(true);
            ToggleVolumetricSmoke(false);
            ToggleSparks(true);
        }
    }

    // Toggle wind trail effect
    public void ToggleTrailEffect (bool value)
    {
        topLeft_TRL.enabled = value;
        topRight_TRL.enabled = value;
        botLeft_TRL.enabled = value;
        botRight_TRL.enabled = value;
    }

    // Toggle 3D tire smoke
    public void ToggleVolumetricSmoke (bool value)
    {
        leftTireSmoke.enabled = value;
        rightTireSmoke.enabled = value;
    }

    // Toggle particle smoke (in world space) 
    public void ToggleParticleSmoke(bool value)
    {
        if (value) // If player is holding the pole
        {
            for (int i = 0; i < smokeEffect.Length; i++)
            {
                smokeEffect[i].Play(); // Play particle effect
            }
        }
        else // Player released
        {
            for (int i = 0; i < smokeEffect.Length; i++)
            {
                smokeEffect[i].Stop();
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
                sparks[i].Play(); // Play particle effect
            }
        }
        else // Player released
        {
            for (int i = 0; i < sparks.Length; i++)
            {
                sparks[i].Stop();
            }
        }

    }
}
