using Cinemachine;
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
    public PlayerInput playerInput;
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
    public float startingShootforce;
    public float maxShootForce;
    public float shootForceIncreaseSpeed;
    Vector3 direction;
    public bool isShooting;
    public bool aritime;
    Vector3 positionAtTime;

    [Header("Rate of Fire")]
    public float shootCooldown;
    public float lastShootTime;
    public bool canShoot;

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

    [Header("Screen Shake")]
    public CinemachineVirtualCamera cinemachine;
    public CameraManager camManager;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();

        shoot = playerInput.actions["CannonPower"];
        shoot.Enable();

        shoot.performed += ctx =>
        {
            isShooting = true; 
        };

        shoot.canceled += ctx =>
        {
            shootForce = startingShootforce;
            isShooting = false;
        };

        carLoaded = false;
        shootForceIncreaseSpeed = 3.5f;
        hitMarker.GetComponent<MeshRenderer>().enabled = false;

        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        direction = (shootingPoint.transform.position - transform.position).normalized;

        if(Time.time > lastShootTime)
        {
            canShoot = true;
        }
        else
        {
            canShoot = false;
        }


        //Debug.Log(cart.cartState);
        if (cart.cartState != CarController.CartState.Running)
        {
            //Controls whether the Hit Marker is shown or hidden
            if (isShooting && projectile.Count > 0)
            {
                if (canShoot == true)
                {
                    trajectoryLine.enabled = true;
                    hitMarker.GetComponent<MeshRenderer>().enabled = true;

                    if (shootForce < maxShootForce)
                    {
                        shootForce = Mathf.Lerp(shootForce, maxShootForce * playerInput.actions["CannonPower"].ReadValue<float>(), shootForceIncreaseSpeed * Time.deltaTime);
                    }
                    DrawTrajectory();

                    if (playerInput.actions["CannonShoot"].ReadValue<float>() > 0.4)
                    {
                        Shoot();
                    }
                }
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

        if(canShoot == false)
        {
            trajectoryLine.enabled = false;
            hitMarker.GetComponent<MeshRenderer>().enabled = false;
        }

    }

    private void FixedUpdate()
    {
        if (cart.cartState != CarController.CartState.Running && cart.cartState != CarController.CartState.PoleHolding)
        {
            if (canShoot == true)
            {
                //AIMING CANNON
                rightStick = playerInput.actions["CannonAim"].ReadValue<Vector2>();

                AimCannon();
            }
        }
        else
        {
            MassShift(0);
            gameObject.transform.localEulerAngles = new Vector3(startingX, startingY, gameObject.transform.localEulerAngles.z); // Apply both clamped X and Y rotations to the cannon
        }
    }

    private void AimCannon()
    {
        rightStick = playerInput.actions["CannonAim"].ReadValue<Vector2>();
        Vector3 currentLocalRotation = gameObject.transform.localEulerAngles; // Get current local rotation (normalized to -180 to 180 for Y and X adjustments)

        //HORIZONTAL CANNON MOVEMENT
        float rotationY = currentLocalRotation.y;
        if (rotationY > 180)
        {
            rotationY -= 360; // Normalize to -180 to 180
        }

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
                if (canShoot == true)
                {
                    var bullet = Instantiate(projectile[0], shootingPoint.transform.position, shootingPoint.transform.rotation);
                    bullet.GetComponent<Projectile>().applyProperties(shootingPoint.transform, direction, shootForce, haptics, cart);
                    bullet.GetComponent<Projectile>().forcesApplied = true;

                    projectile.RemoveAt(0);

                    // VFX for shooting
                    cart.vfxHandler.ShootItem();
                    cart.audioHandler.ShootItem();

                    haptics.CannonHaptics();
                    shootForce = startingShootforce;

                    //screenShake
                    cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = camManager.amplitudeChange + camManager.maxAmplitude;
                    cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = camManager.frequencyChange + camManager.maxFrequency;

                    lastShootTime = Time.time + shootCooldown;
                }
            }

            //Debug.Log(projectile.Count);
        }

    }

    //LOADING THE CANNON
    public void LoadCannon(GameObject newProjectile)
    {
        projectile.Add(newProjectile);
        projectileRb = newProjectile.GetComponent<Rigidbody>();
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
