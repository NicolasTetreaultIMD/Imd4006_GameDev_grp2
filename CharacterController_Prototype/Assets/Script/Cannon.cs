using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder;
using UnityEngine.Timeline;

public class Cannon : MonoBehaviour
{
    [Header("Player Input")]
    PlayerInput playerInput;
    private InputAction shoot;
    public Vector2 rightStick;

    [Header("Cannon Movement")]
    public float cannonRotationSpeed;
    public float maxHorizontalTurn;
    public float maxVerticalTurn;

    [Header("Projectile")]
    public GameObject projectile;
    public Rigidbody projectileRb;
    public GameObject shootingPoint;
    public float shootForce;
    Vector3 direction;
    public bool isShooting;

    [Header("Line of Trajectory")]
    public LineRenderer trajectoryLine;
    public Transform hitMarker;
    public int maxPoints;
    public float increment;
    float rayOverlap = 1.1f;


    [Header("Center of Mass")]
    public CenterMassManager centerMassManager;
    public float maxCarTiltInfluence;
    private float prevMassChange;


    // Start is called before the first frame update
    void Start()
    {
        playerInput = new PlayerInput();
        playerInput.Enable();

        shoot = playerInput.Gameplay.CannonShoot;
        shoot.Enable();
        shoot.performed += ctx => isShooting = true;
        shoot.canceled += ctx => isShooting = false;
        shoot.canceled += Shoot;

    }

    // Update is called once per frame
    void Update()
    {
        direction = (shootingPoint.transform.position - transform.position).normalized;

        if (isShooting && projectile != null)
        {
            trajectoryLine.enabled = true;
            hitMarker.GetComponent<MeshRenderer>().enabled = true;
            DrawTrajectory();
        }
        else
        {
            trajectoryLine.enabled = false;
            hitMarker.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void FixedUpdate()
    {
        //AIMING CANNON
        rightStick = playerInput.Gameplay.CannonAim.ReadValue<Vector2>();
        AimCannon();
    }

    private void AimCannon()
    {
        rightStick = playerInput.Gameplay.CannonAim.ReadValue<Vector2>();    
        Vector3 currentLocalRotation = gameObject.transform.localEulerAngles; // Get current local rotation (normalized to -180 to 180 for Y and X adjustments)

        //HORIZONTAL CANNON MOVEMENT
        float rotationY = currentLocalRotation.y;
        if (rotationY > 180)
        {
            rotationY -= 360; // Normalize to -180 to 180
        }
        if (rightStick.x > 0)
        {
            rotationY += cannonRotationSpeed * Time.deltaTime;
        }
        else if (rightStick.x < 0)
        {
            rotationY -= cannonRotationSpeed * Time.deltaTime;
        }
        rotationY = Mathf.Clamp(rotationY, -maxHorizontalTurn, maxHorizontalTurn); // Clamp the Y rotation between -45 and 45 degrees

        MassShift(rotationY);


        if (rotationY < 0)
        {
            rotationY += 360; // Convert back to 0-360 range if negative
        }

        // VERTICAL CANNON MOVEMENT
        float rotationX = currentLocalRotation.x;
        if (rotationX > 180)
        {
            rotationX -= 360; // Normalize to -180 to 180
        }
        if (rightStick.y > 0)
        {
            rotationX -= (cannonRotationSpeed /4) * Time.deltaTime; // Move upward (toward 0)
        }
        else if (rightStick.y < 0)
        {
            rotationX += (cannonRotationSpeed/4) * Time.deltaTime; // Move downward (toward -20)
        }
        rotationX = Mathf.Clamp(rotationX, maxVerticalTurn, 0f); // Clamp the X rotation between 0 and -20 degrees
        gameObject.transform.localEulerAngles = new Vector3(rotationX, rotationY, currentLocalRotation.z); // Apply both clamped X and Y rotations to the cannon
    }

    private void MassShift(float angle)
    {
        //Car tilt
        float massChange = maxCarTiltInfluence * (angle / maxHorizontalTurn);
        //Debug.Log(angle);
        centerMassManager.massCenter.x += massChange - prevMassChange;
        prevMassChange = massChange;
    }

    //SHOOTING THE CANNONM
    private void Shoot(InputAction.CallbackContext context)
    {
        if (projectile != null)
        {
            var bullet = Instantiate(projectile, shootingPoint.transform.position, shootingPoint.transform.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(direction * shootForce, ForceMode.Impulse);
        }

        projectile = null;
    }

    //LOADING THE CANNON
    public void LoadCannon(GameObject newProjectile)
    {
        projectile = newProjectile;
        projectileRb = projectile.GetComponent<Rigidbody>();
    }

    //CREATING THE LINE OF TRAJECTORY FOR THE PROJECTILE
    void DrawTrajectory()
    {
        Vector3 velocity = direction * (shootForce / projectileRb.mass); // Formula for trajectory
        Vector3 position = shootingPoint.transform.position; // Starting position for the first point in the trajectory line
        Vector3 nextPosition; // Next point to generate for the trajectory line
        float overlap;

        UpdateLineRender(maxPoints, (0, position));

        for (int i = 1; i < maxPoints; i++)
        {
            
            velocity = CalculateNewVelocity(velocity, projectileRb.drag, increment); // Estimate velocity and update next predicted position
            nextPosition = position + velocity * increment;

            overlap = Vector3.Distance(position, nextPosition) * rayOverlap; // Overlap the rays by small margin to ensure surface isn't missed

            if (Physics.Raycast(position, velocity.normalized, out RaycastHit hit, overlap)) //When hitting a surface, show the surface marker and stop updating the line
            {
                UpdateLineRender(i, (i - 1, hit.point));
                MoveHitMarker(hit); 
                break;
            }

            hitMarker.gameObject.SetActive(false); //If nothing is hit, continue rendering the arc without a visual marker
            position = nextPosition;
            UpdateLineRender(maxPoints, (i, position));
        }
        
    }
    
    //CALCULATE CURRENT VELOCITY FROM POINT ON THE LINE RENDERED
    private Vector3 CalculateNewVelocity(Vector3 velocity, float drag, float increment)
    {
        velocity += Physics.gravity * increment;
        velocity *= Mathf.Clamp01(1f - (drag * increment));
        return velocity;
    }

    //REDRAW THE LINE RENDERER
    private void UpdateLineRender(int count, (int point, Vector3 pos) pointPos)
    {
        trajectoryLine.positionCount = count;
        trajectoryLine.SetPosition(pointPos.point, pointPos.pos);
    }

    //MOVE THE HITMARKER TO THE FIRST SURFACE FOUND
    private void MoveHitMarker(RaycastHit hit)
    {
        hitMarker.gameObject.SetActive(true);

        float offset = 0.025f; // Offset marker from surface
        hitMarker.position = hit.point + hit.normal * offset;
        hitMarker.rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
    }

    

    

}
