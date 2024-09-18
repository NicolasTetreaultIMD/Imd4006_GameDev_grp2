using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armController : MonoBehaviour
{
    PlayerInput playerControls;
    void Start()
    {
        playerControls = new PlayerInput();
        playerControls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(playerControls.Gameplay.ArmMovement.ReadValue<Vector2>());
    }
}
