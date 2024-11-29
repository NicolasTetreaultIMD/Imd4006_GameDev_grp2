using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //Mine Mesh
    public GameObject buttonMesh;

    //Trap Mesh
    public GameObject openTrap;
    public GameObject closeTrap;

    //Bomb Mesh
    public GameObject bombMesh;

    //Nuke Mesh
    public GameObject nukeMesh;

    [Header("Projectile Movement")]
    float totalTime = 0f;
    public Vector3 shootingPoint;
    public Vector3 direction;
    public float shootForce;
    public float mass;
    public float projectileSpeed;

    [Header("Explosions")]
    public bool isReady;
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
        isReady = false;

        mass = gameObject.GetComponent<Rigidbody>().mass;

        StartCoroutine(LaunchPeriod());

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
                //Stops Projectile from continuing to fall
                madeContact = true;
                gameObject.GetComponent<Rigidbody>().isKinematic = true;

                //Applier Effect
                explosion.gameObject.SetActive(true);                
                bombMesh.gameObject.SetActive(false);
                explosion.GetComponent<DamageApplier>().playerId = carController.playerId;

                //FX
                carController.audioHandler.impactExplosion();
                haptics.ExplosionHaptics();

                StartCoroutine(FadeOut());
            }

            //Landmine Projectile
            if(gameObject.tag == "Mine")
            {
                //Collision with evironment
                if (collision.gameObject.tag != "Player")
                {
                    //Stops Projectile from continuing to fall
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
             
                //Collision with evironment
                if (nukeTracker.foundPlayer == false)
                {
                    if (collision.gameObject.GetComponent<CarController>() != null)
                    {
                        if (collision.gameObject.GetComponent<CarController>().playerId != carController.playerId)
                        {
                            //Stops Projectile from continuing to fall
                            madeContact = true;
                            gameObject.GetComponent<Rigidbody>().isKinematic = true;

                            //Applier Effect
                            nukeMesh.SetActive(false);
                            explosion.gameObject.SetActive(true);
                            explosion.GetComponent<DamageApplier>().playerId = carController.playerId;

                            //FX
                            carController.audioHandler.impactExplosion();
                            haptics.ExplosionHaptics();

                            StartCoroutine(FadeOut());
                        }
                    }
                    else
                    {
                        //Stops Projectile from continuing to fall
                        madeContact = true;
                        gameObject.GetComponent<Rigidbody>().isKinematic = true;

                        //Applier Effect
                        nukeMesh.SetActive(false);
                        explosion.gameObject.SetActive(true);
                        explosion.GetComponent<DamageApplier>().playerId = carController.playerId;

                        //FX
                        carController.audioHandler.impactExplosion();
                        haptics.ExplosionHaptics();

                        StartCoroutine(FadeOut());
                    }
                }
            }

            //Beartrap Projectile
            if (gameObject.tag == "Trap")
            {
                if (collision.gameObject.tag != "Player")
                {
                    //Stops Projectile from continuing to fall
                    madeContact = true;
                    gameObject.transform.rotation = Quaternion.identity;
                }
            }

        }
    }

    //For non-instant explosion projectiles
    private void OnTriggerEnter(Collider other)
    {
        if (isReady)
        {
            if (gameObject.tag == "Mine")
            {
                if (other.gameObject.tag == "Player" && madeContact == true)
                {
                    explosion.gameObject.SetActive(true);
                    carController.audioHandler.impactExplosion(); // play explosion SFX

                }
            }
        }

        if (isReady)
        {
            if (gameObject.tag == "Trap")
            {
                if (other.gameObject.tag == "Player" && madeContact == true)
                {
                    explosion.gameObject.SetActive(true); //Trap's "explosion" is just the hitbox for when the player get's stunned.

                    openTrap.SetActive(false);
                    closeTrap.SetActive(true);

                    carController.audioHandler.activateTrap();
                }
            }
        }
    }

    private IEnumerator LaunchPeriod()
    {
        yield return new WaitForSeconds(1.2f);
        isReady = true;

        if (gameObject.tag == "Mine")
        {
            carController.audioHandler.mineBeep();
            gameObject.transform.Find("landmineButton_geo").GetComponent<Renderer>().material.color = Color.red;
        }

        if(gameObject.tag == "Trap")
        {
            openTrap.SetActive(true);
            closeTrap.SetActive(false);
        }
    }

        private IEnumerator FadeOut()
    {
        // Wait for 2 seconds before continuing
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
