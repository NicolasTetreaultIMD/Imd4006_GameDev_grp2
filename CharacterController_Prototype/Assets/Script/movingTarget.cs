using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingTarget : MonoBehaviour
{
    // Speed at which the target moves (units per second)
    private float moveSpeed = 20f;

    // The left and right boundaries for movement
    private float leftLimit = -20;
    private float rightLimit = 20;

    // Direction of movement: 1 for right, -1 for left
    private int moveDirection = 1;

    // Update is called once per frame
    void Update()
    {
        // Move the target horizontally based on the move direction
        transform.Translate(Vector2.right * moveSpeed * moveDirection * Time.deltaTime);

        // Reverse direction if we hit the left or right boundaries
        if (transform.position.x <= leftLimit)
        {
            moveDirection = 1;  // Move to the right
        }
        else if (transform.position.x >= rightLimit)
        {
            moveDirection = -1; // Move to the left
        }
    }
}


