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


        // Show speed lines when the player exceeds a certain speed
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
        else
        {
            ToggleTrailEffect(false);
        }


        if (cartController.cartState == CarController.CartState.Running || cartController.cartState == CarController.CartState.InCart)
        {
            ToggleParticleSmoke(false);
            ToggleVolumetricSmoke(true);
        }
        else if(cartController.cartState == CarController.CartState.PoleHolding)
        {
            ToggleParticleSmoke(true);
            ToggleVolumetricSmoke(false);
        }


    }

    public void ToggleTrailEffect (bool value)
    {
        topLeft_TRL.enabled = value;
        topRight_TRL.enabled = value;
        botLeft_TRL.enabled = value;
        botRight_TRL.enabled = value;
    }

    public void ToggleVolumetricSmoke (bool value)
    {
        leftTireSmoke.enabled = value;
        rightTireSmoke.enabled = value;
    }

    // TOGGLE SMOKE EFFECT
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
}
