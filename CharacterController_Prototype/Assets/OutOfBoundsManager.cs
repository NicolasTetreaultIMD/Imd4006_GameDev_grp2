using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class OutOfBoundsManager : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<DamageHandler>().isImmune = true;
            StartCoroutine(other.GetComponent<DamageHandler>().ImmunityTime());
            other.GetComponent<DamageHandler>().shield.SetActive(true);

            other.transform.position = spawnPoints[other.GetComponent<CarController>().playerId].position;
            other.transform.rotation = spawnPoints[other.GetComponent<CarController>().playerId].rotation;
        }
    }
}
