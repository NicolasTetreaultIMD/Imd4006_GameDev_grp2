using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ArmGrabCollider : MonoBehaviour
{
    public GrabManager grabManager;

    //0 = left; 1 = right
    public int armType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pole"))
        {
            grabManager.GrabPole(other.gameObject.transform);
        }
    }
}
