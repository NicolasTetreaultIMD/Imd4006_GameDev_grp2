using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        Vector2 armInput = playerControls.Gameplay.ArmMovement.ReadValue<Vector2>();
        //armInput.Normalize();
        armInput.x /= Screen.width;
        armInput.y /= Screen.height;

        armInput -= new Vector2(0.5f,0.5f);

        armInput = new Vector2(Mathf.Max(Mathf.Min(armInput.x, 0.5f), -0.5f), Mathf.Max(Mathf.Min(armInput.y, 0.5f), -0.5f));
        Debug.Log(armInput);
    }
}
