using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CenterMassManager : MonoBehaviour
{
    //Center of mass
    //x = rotation; y = up down; z = ???
    public Vector3 massCenter;

    public Transform carBody;       //Main body of the car (seperate from wheels)
    public Transform leftRotation;  //Left tilt
    public Transform rightRotation; //right tilt

    public float rotationDeadZone;  //Minimum value until the car starts tilting
    public float maxRotationAngle;  //Max tilt the car can have
    public float maxRotationInput;  //Max center of mass value
    public float rotationSpeed;     //Speed for the rotation lerp

    public float maxHeight;         //Max height of body
    public float minHeight;         //Minimum height of body
    public float sinkSpeed;         //Speed for the sink lerp

    private Vector3 origBodyPos;

    // Start is called before the first frame update
    void Start()
    {
        origBodyPos = carBody.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        carTilt();
        carSink();
    }

    //CAUSES ISSUES WITH CAR SHAKE CURRENTLY
    //IN THE FUTUR INTEGRATE CAR SHAKE USING THE CENTER OF MASS
    private void carSink()
    {
        /*massCenter.y = Mathf.Min(Mathf.Max(massCenter.y, minHeight), maxHeight);

        carBody.localPosition = Vector3.Lerp(carBody.localPosition, new Vector3(origBodyPos.x, origBodyPos.y + massCenter.y, origBodyPos.z), Time.deltaTime * sinkSpeed);*/
    }

    //Tilts the body of the car depending on the position of the x component of the center of mass
    private void carTilt()
    {
        if (Mathf.Abs(massCenter.x) > rotationDeadZone)
        {
            massCenter.x = Mathf.Min(Mathf.Max(massCenter.x, -maxRotationInput), maxRotationInput);

            //Right Tilt
            if (massCenter.x > 0)
            {
                float cartRotation = Map(rotationDeadZone, maxRotationInput, 0, -maxRotationAngle, massCenter.x);

                leftRotation.localRotation = Quaternion.Lerp(leftRotation.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * rotationSpeed);
                rightRotation.localRotation = Quaternion.Lerp(rightRotation.localRotation, Quaternion.Euler(0, 0, cartRotation), Time.deltaTime * rotationSpeed);
            }
            //Left Tilt
            else if (massCenter.x < 0)
            {
                float cartRotation = Map(-maxRotationInput, -rotationDeadZone, maxRotationAngle, 0, massCenter.x);

                leftRotation.localRotation = Quaternion.Lerp(leftRotation.localRotation, Quaternion.Euler(0, 0, cartRotation), Time.deltaTime * rotationSpeed);
                rightRotation.localRotation = Quaternion.Lerp(rightRotation.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * rotationSpeed);
            }

        }
        else
        {
            leftRotation.localRotation = Quaternion.Lerp(leftRotation.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * rotationSpeed);
            rightRotation.localRotation = Quaternion.Lerp(rightRotation.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * rotationSpeed);
        }
    }

    public static float Map(float a, float b, float c, float d, float x)
    {
        return c + (x - a) * (d - c) / (b - a);
    }
}
