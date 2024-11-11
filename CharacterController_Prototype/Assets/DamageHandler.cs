using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    public Material testMaterial;
    public Material currentMaterial;

    public bool isProjectile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit()
    {
        //If object is the projectile, then don't do anything
        if (!isProjectile)
        {
            gameObject.GetComponent<Renderer>().material = testMaterial;
        }
    }

}