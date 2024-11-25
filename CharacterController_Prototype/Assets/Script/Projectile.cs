using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public MeshRenderer bombMesh;

    [Header("Projectile Movement")]
    float totalTime = 0f;
    public Vector3 shootingPoint;
    public Vector3 direction;
    public float shootForce;
    public float mass;
    public float projectileSpeed;

    [Header("Explosions")]
    public bool forcesApplied;
    public bool madeContact;
    public GameObject explosion;
    public HapticFeedback haptics;

    private CarController carController;
    

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Awake()
    {
        projectileSpeed = 2;
        forcesApplied = false;
        madeContact = false;
        mass = gameObject.GetComponent<Rigidbody>().mass;

         
    }

    // Update is called once per frame
    void Update()
    {
        //Make the Projectile follow the path designated by the Cannon
        if (forcesApplied == true && madeContact == false)
        {
            totalTime += Time.deltaTime * projectileSpeed; // Accumulate time each frame
            Vector3 positionAtTime = shootingPoint
                                   + direction * (shootForce / mass) * totalTime
                                   + 0.5f * Physics.gravity * totalTime * totalTime;

            transform.Rotate(Vector3.up, 300 * Time.deltaTime, Space.Self);
            transform.Rotate(Vector3.right, 300 * Time.deltaTime, Space.Self);

            transform.position = positionAtTime;
        }
    }

    //Applies the properties from the Cannon to shoot the Projectile accordingly
    public void applyProperties(Transform newShootingPoint, Vector3 newDirection, float newShootForce, HapticFeedback newHaptics, CarController newCart)
    {
        shootingPoint = newShootingPoint.position;
        direction = newDirection;
        shootForce = newShootForce;
        haptics = newHaptics;
        carController = newCart;
        forcesApplied = true;

    }


    private void OnCollisionEnter(Collision collision)
    {
        //If Projectile collides with either a default layer or an obstacle layer, then stop moving and create explosion.
        if (collision.gameObject.layer == 0 || collision.gameObject.layer == 7)
        {
            madeContact = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            explosion.gameObject.SetActive(true);
            explosion.GetComponent<DamageApplier>().playerId = carController.playerId;
            bombMesh.enabled = false;

            carController.audioHandler.impactExplosion();
            haptics.ExplosionHaptics();

            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        // Wait for 2 seconds before continuing
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
