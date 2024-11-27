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
    public NukeTracker nukeTracker;

    public CarController carController;


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

            if (gameObject.tag != "Nuke")
            {
                transform.Rotate(Vector3.up, 300 * Time.deltaTime, Space.Self);
                transform.Rotate(Vector3.right, 300 * Time.deltaTime, Space.Self);
            }

            transform.position = positionAtTime;
        }

        if (carController != null)
        {
            //if (gameObject.tag == "Trap")
            //{
            //    explosion.GetComponent<DamageApplier>().playerId = carController.playerId;
            //}
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

        if (gameObject.tag == "Mine")
        {
            carController.audioHandler.mineBeep();
        }

        if (gameObject.tag == "Nuke")
        {
            carController.audioHandler.nukeWhistle();
        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        //If Projectile collides with either a default layer or an obstacle layer, then stop moving and create explosion.
        if (collision.gameObject.layer == 0 || collision.gameObject.layer == 7)
        {
            //Normal Bomb Projectile
            if (gameObject.tag == "Bomb")
            {
                madeContact = true;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                explosion.gameObject.SetActive(true);
                explosion.GetComponent<DamageApplier>().playerId = carController.playerId;
                bombMesh.enabled = false;

                carController.audioHandler.impactExplosion(); // PLAY Audio
                haptics.ExplosionHaptics();

                StartCoroutine(FadeOut());
            }

            //Landmine Projectile
            if(gameObject.tag == "Mine")
            {
                if (collision.gameObject.tag != "Player")
                {
                    madeContact = true;
                    gameObject.transform.rotation = Quaternion.identity;
                    explosion.GetComponent<DamageApplier>().playerId = carController.playerId;
                }
            }

            //Nuke Projectile
            if (gameObject.tag == "Nuke")
            {
                gameObject.GetComponent<NukeTracker>().playerId = carController.playerId;
                explosion.GetComponent<DamageApplier>().playerId = carController.playerId;

                if (nukeTracker.foundPlayer == false)
                {
                    gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    explosion.gameObject.SetActive(true);
                    explosion.GetComponent<DamageApplier>().playerId = carController.playerId;

                    carController.audioHandler.impactExplosion();
                    haptics.ExplosionHaptics();

                    StartCoroutine(FadeOut());
                }
            }

            //Beartrap Projectile
            if (gameObject.tag == "Trap")
            {
                if (collision.gameObject.tag != "Player")
                {
                    madeContact = true;
                    gameObject.transform.rotation = Quaternion.identity;
                }
            }

        }
    }

    //For non-instant explosion projectiles
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "Mine")
        {
            if (other.gameObject.tag == "Player" && madeContact == true)
            {
                explosion.gameObject.SetActive(true);
                carController.audioHandler.impactExplosion(); // play explosion SFX

            }
        }

        if (gameObject.tag == "Trap")
        {
            if (other.gameObject.tag == "Player" && madeContact == true)
            {
                explosion.gameObject.SetActive(true);
                carController.audioHandler.activateTrap();
            }
        }
    }


    private IEnumerator FadeOut()
    {
        // Wait for 2 seconds before continuing
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
