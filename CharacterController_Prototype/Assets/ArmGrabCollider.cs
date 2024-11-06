using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ArmGrabCollider : MonoBehaviour
{
    public GrabManager grabManager;

    private void OnTriggerEnter(Collider other)
    {
        //If a pole is collided with send the collided pole to start the pole hold state
        if (other.gameObject.layer == LayerMask.NameToLayer("Pole"))
        {
            grabManager.GrabPole(other.gameObject.transform);
        }
    }
}
