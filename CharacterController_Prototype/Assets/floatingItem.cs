using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatingItem : MonoBehaviour
{

    public float bobbingHeight = 0.5f;  // The height the item will bob up and down
    public float bobbingSpeed = 2f;     // Speed of bobbing motion
    public float rotationSpeed = 100f;  // Speed of rotation

    private Vector3 originalPosition;    // The original positon of the item (before bobbing)

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Bobbing effect using a sine wave (bobbingHeight controls the max height)
        float bobbingOffset = Mathf.Sin(Time.time * bobbingSpeed) * bobbingHeight;

        transform.position = originalPosition + new Vector3(0, bobbingOffset, 0);

        //Spin the item around its Y-Axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
