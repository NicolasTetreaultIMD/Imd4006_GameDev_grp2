using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FootStepVFX : MonoBehaviour
{

    public ParticleSystem left;
    public ParticleSystem right;

    public void CreateFootImpact(string foot)
    {

        if( foot== "Right")
        {
            right.Play();
        }
        else if (foot== "Left")
        {
            left.Play();
        }
    }
}
