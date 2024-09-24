using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabColliderManager : MonoBehaviour
{
    public armController armManager;

    private void OnTriggerEnter(Collider other)
    {
        armManager.AddGrabItem(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        armManager.RemoveGrabItem(other.gameObject);
    }
}
