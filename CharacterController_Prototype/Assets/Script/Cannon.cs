using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
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
    public CarController cart;
    public HapticFeedback haptics;

    [Header("Cannon Movement")]
    public float cannonRotationSpeed;
    public float maxHorizontalTurn;
    public float maxVerticalTurn;
    public float startingX;
    public float startingY;
    bool carLoaded;

    [Header("Projectile")]
    public List<GameObject> projectile; // Current inventory of projectiles
    public Rigidbody projectileRb;
    public GameObject shootingPoint;
    public float shootForce;
    public float maxShootForce;
    public float shootForceIncreaseSpeed;
    Vector3 direction;
    public bool isShooting;
    public bool aritime;
    Vector3 positionAtTime;

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

        shoot.performed += ctx =>
        {
            isShooting = true; 
            shootForce = 20;
        };

        shoot.canceled += ctx =>
        {
            Shoot();
            isShooting = false;
        };

        carLoaded = false;
        shootForceIncreaseSpeed = 2;
        hitMarker.GetComponent<MeshRenderer>().enabled = false;


    }

    // Update is called once per frame
    void Update()
    {
        direction = (shootingPoint.transform.position - transform.position).normalized;


        //Debug.Log(cart.cartState);
        if (cart.cartState != CarController.CartState.Running)
        {
            //Controls whether the Hit Marker is shown or hidden
            if (isShooting && projectile.Count > 0)
            {
                trajectoryLine.enabled = true;
                hitMarker.GetComponent<MeshRenderer>().enabled = true;

                if (shootForce < maxShootForce)
                {
                    shootForce = Mathf.Lerp(shootForce, maxShootForce, shootForceIncreaseSpeed * Time.deltaTime);
                }
                DrawTrajectory();
            }
            else
            {
                trajectoryLine.enabled = false;
                hitMarker.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else
        {
            trajectoryLine.enabled = false;
            hitMarker.GetComponent<MeshRenderer>().enabled = false;
        }

    }

    private void FixedUpdate()
    {
        if (cart.cartState != CarController.CartState.Running)
        {
            //AIMING CANNON
            rightStick = playerInput.Gameplay.CannonAim.ReadValue<Vector2>();

            if (carLoaded)
            {
                projectileRb = projectile[0].GetComponent<Rigidbody>();
            }

            AimCannon();
        }
        else
        {
            MassShift(0);
            gameObject.transform.localEulerAngles = new Vector3(startingX, startingY, gameObject.transform.localEulerAngles.z); // Apply both clamped X and Y rotations to the cannon
        }
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




        //rotationY += cannonRotationSpeed * Time.deltaTime;

        //else if (rightStick.x < -0.5)
        //{
        //    rotationY -= cannonRotationSpeed * Time.deltaTime;
        //}

        //RESET CANNON ROTATION
        /*if (Mathf.Abs(rightStick.y) <= 0.02f)
        {
            rotationY = Mathf.Lerp(rotationY, startingY, Time.deltaTime * 6f);
        }*/

        //rotationY = Mathf.Clamp(rotationY, startingY-maxHorizontalTurn, startingY + maxHorizontalTurn); // Clamp the Y rotation between -45 and 45 degrees

        /*MassShift(rotationY);


        if (rotationY < 0)
        {
            rotationY += 360; // Convert back to 0-360 range if negative
        }*/

        //// VERTICAL CANNON MOVEMENT
        //float rotationX = currentLocalRotation.x;
        //if (rotationX > 180)
        //{
        //    rotationX -= 360; // Normalize to -180 to 180
        //}

        //if (rightStick.y > 0.5)
        //{
        //    rotationX -= (cannonRotationSpeed /2) * Time.deltaTime; // Move upward (toward 0)
        //}
        //else if (rightStick.y < -0.5)
        //{
        //    rotationX += (cannonRotationSpeed/2) * Time.deltaTime; // Move downward (toward -20)
        //}

        //if (Mathf.Abs(rightStick.x) <= 0.02f)
        //{
        //    rotationX = Mathf.Lerp(rotationX, startingX, Time.deltaTime * 6f);
        //}

        //rotationX = Mathf.Clamp(rotationX, startingX + maxVerticalTurn, 0f); // Clamp the X rotation between 0 and -20 degrees

        MassShift(rightStick.x);
        gameObject.transform.localRotation = Quaternion.Lerp(gameObject.transform.localRotation, Quaternion.Euler(startingX, rightStick.x * maxHorizontalTurn, currentLocalRotation.z), Time.deltaTime * cannonRotationSpeed);
        //gameObject.transform.localEulerAngles = new Vector3(startingX, rightStick.x * maxHorizontalTurn, currentLocalRotation.z); // Apply both clamped X and Y rotations to the cannon

        

    }

    private void MassShift(float stickInput)
    {
        //Car tilt
        float massChange = maxCarTiltInfluence * stickInput;
        //Debug.Log(angle);
        centerMassManager.massCenter.x += massChange - prevMassChange;
        prevMassChange = massChange;
    }

    //SHOOTING THE CANNONM
    private void Shoot()
    {
        if (cart.cartState != CarController.CartState.Running)
        {
            if (projectile.Count > 0)
            {
                var bullet = Instantiate(projectile[0], shootingPoint.transform.position, shootingPoint.transform.rotation);
                bullet.GetComponent<Projectile>().applyProperties(shootingPoint.transform, direction, shootForce, haptics);
                bullet.GetComponent<Projectile>().forcesApplied = true;
                projectile.RemoveAt(0);
                //bullet.GetComponent<Rigidbody>().AddForce(direction * shootForce, ForceMode.Impulse);

                // VFX for shooting
                cart.vfxHandler.ShootItem();
                haptics.CannonHaptics();

            }

            if (projectile.Count == 0)
            {
                carLoaded = false;
            }

            //Debug.Log(projectile.Count);
        }

    }

    //LOADING THE CANNON
    public void LoadCannon(GameObject newProjectile)
    {
        projectile.Add(newProjectile);
        carLoaded = true;
        haptics.CrateHaptics();
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
    
    //CALCULATE CURRENT VELOCITY FROM CURRENT POINT ON THE LINE RENDERER
    private Vector3 CalculateNewVelocity(Vector3 velocity, float drag, float increment)
    {
        velocity += Physics.gravity * increment;
        velocity *= Mathf.Clamp01(1f - (drag * increment));
        return velocity;
    }

    //ADD THE NEW POINT TO THE LINE RENDERER
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
