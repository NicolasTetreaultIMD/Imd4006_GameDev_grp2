using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRespawn : MonoBehaviour
{
    public Renderer itemCrate;
    public Collider itemCollider;

    // Start is called before the first frame update
    void Start()
    {
        int randomTag = Random.Range(1, 101);

        if (randomTag > 0 && randomTag <= 27.5f) // 27.5% chance
        {
            gameObject.tag = "Bomb Box";
        }
        else if (randomTag > 27.5f && randomTag <= 55) // 27.5% chance
        {
            gameObject.tag = "Mine Box";
        }
        else if (randomTag > 55 && randomTag <= 82.5f) // 27.5% chance
        {
            gameObject.tag = "Trap Box";
        }
        else if (randomTag > 82.5f && randomTag <= 100) // 17.5% chance
        {
            gameObject.tag = "Nuke Box";
        }
        else
        {
            gameObject.tag = "Nuke Box"; //In case of messed up math
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    public void ItemPickedUp()
    {
        itemCrate.enabled = false;
        itemCollider.enabled = false;
        StartCoroutine(ItemCooldown());
    }

    private IEnumerator ItemCooldown()
    {
        yield return new WaitForSeconds(0.25f);

        int randomValue = Random.Range(20, 31);
        int randomTag = Random.Range(1, 5);


        if (randomTag == 1)
        {
            gameObject.tag = "Bomb Box";
        }
        else if(randomTag == 2)
        {
            gameObject.tag = "Mine Box";
        }
        else if (randomTag == 3)
        {
            gameObject.tag = "Nuke Box";
        }
        else if (randomTag == 4)
        {
            gameObject.tag = "Trap Box";
        }

        yield return new WaitForSeconds(randomValue);

        itemCrate.enabled = true;
        itemCollider.enabled = true;
    }
}
