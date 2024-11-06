using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder;

public class Cannon : MonoBehaviour
{
    PlayerInput playerInput;
    private InputAction shoot;
    public Vector2 rightStick;
    public float cannonRotationSpeed;

    public GameObject projectile;
    public GameObject shootingPoint;
    public float shootForce;

    public LineRenderer lineRenderer;
    public int linePoints = 175;
    public float timeIntervalinPoints = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = new PlayerInput();
        playerInput.Enable();

        shoot = playerInput.Gameplay.CannonShoot;
        shoot.Enable();
        shoot.performed += Shoot;
    }

    // Update is called once per frame
    void Update()
    {

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

        Vector3 currentLocalRotation = gameObject.transform.localEulerAngles; // Get the current local rotation of the cannon (relative to the car)
        if (currentLocalRotation.y > 180) // Convert to -180 to 180 range for clamping
        {
            currentLocalRotation.y -= 360; // Convert angle to the range -180 to 180
        }

        if (rightStick.x > 0)
        {
            currentLocalRotation.y += cannonRotationSpeed * Time.deltaTime;
        }
        else if (rightStick.x < 0)
        {
            currentLocalRotation.y -= cannonRotationSpeed * Time.deltaTime;
        }

        currentLocalRotation.y = Mathf.Clamp(currentLocalRotation.y, -45f, 45f); // Clamp the Y rotation between -45 and 45 degrees
        if (currentLocalRotation.y < 0) // Convert back to 0 to 360 range if necessary
        {
            currentLocalRotation.y += 360; // Convert to 0 to 360 range if negative
        }

        // Apply the clamped local rotation to the cannon
        gameObject.transform.localEulerAngles = currentLocalRotation;

        DrawTrajectory();
    }

    void DrawTrajectory()
    {
        Vector3 startVelocity = shootForce * shootingPoint.transform.up;
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if (projectile != null)
        {
            var bullet = Instantiate(projectile, shootingPoint.transform.position, shootingPoint.transform.rotation);
            bullet.GetComponent<Rigidbody>().velocity = shootingPoint.transform.forward * shootForce;
        }

        projectile = null;
    }

    public void LoadCannon(GameObject newProjectile)
    {
        projectile = newProjectile;
    }
}
