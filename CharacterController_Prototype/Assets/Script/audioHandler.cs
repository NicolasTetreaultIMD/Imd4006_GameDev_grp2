using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class audioHandler : MonoBehaviour
{
    public CarController carController;
    public armController armController;

    public AudioSource source;
    public AudioClip cartAudio;
    public AudioClip stepAudio;
    public AudioClip wheelAudio;

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

        //CartAudioEffect(); //call cart audio effect function
        //stepAudioEffect(); //call step audio effect function
        //wheelAudioEffect(); //call wheel audio effect function

    }

    //function to turn on cart audio
    //private void CartAudioEffect()
    //{
    //    //Debug.Log("CartAudio enabled: " + cartAudio.enabled);
    //    if (cartAudio.enabled)
    //    {
    //        float fadeSpeed = 1.7f;

    //        if (speed == 0) // Player is not moving, pause the audio
    //        {
    //            if (cartAudio.volume > 0)
    //            {
    //                //fade out the audio but slightly longer by multiplying time by the fadeSpeed constant 
    //                cartAudio.volume -= Time.deltaTime * fadeSpeed;
    //                if (cartAudio.volume < 0)
    //                {
    //                    cartAudio.volume = 0;
    //                    cartAudio.Pause(); //pause the audio when the volume is fully faded out 
    //                }
    //            }
    //        }
    //        else // Player is moving
    //        {
    //            if (!cartAudio.isPlaying)
    //            {
    //                cartAudio.volume = 0;
    //                cartAudio.Play();
    //            }

    //            if (cartAudio.volume < 0.4f)
    //            {
    //                //fade in the audio by adjusting the volume over time
    //                cartAudio.volume += Time.deltaTime;
    //                if (cartAudio.volume > 0.4f)
    //                {
    //                    //play at full volume
    //                    cartAudio.volume = 0.4f;
    //                }
    //            }

    //            //map the pitch of the audio to the speed of the cart
    //            cartAudio.pitch = Mathf.Lerp(1.5f, 4f, speed / 37.5f);
    //        }
    //    }
    //}

    //Play step audio
    public void stepAudioEffect()
    {
        source.pitch = Random.Range(0.25f,0.75f);
        Debug.Log(source.pitch);
        source.clip = stepAudio;
        source.Play();
    }

    private void wheelAudioEffect()
    {
        if (speed >= 20f)
        {

        }
    }

    //function for wheel audio
    //private void wheelAudioEffect()
    //{
    //    //if the player is going fast and turning, play the audio
    //    if (speed >= 20f && (Keyboard.current.aKey.isPressed || Keyboard.current.dKey.isPressed))
    //    {
    //        if (!wheelAudio.isPlaying)
    //        {
    //            wheelAudio.Play();
    //        }
    //    }
    //    else
    //    {
    //        if (wheelAudio.isPlaying)
    //        {
    //            wheelAudio.Pause();
    //        }
    //    }
    //}
}
