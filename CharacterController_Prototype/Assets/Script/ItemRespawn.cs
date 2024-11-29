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
        int randomTag = Random.Range(1, 5);

        if (randomTag == 1)
        {
            gameObject.tag = "Bomb Box";
        }
        else if (randomTag == 2)
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
        gameObject.tag = "Nuke Box";
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

        Debug.Log("Cooldown");
        int randomValue = Random.Range(20, 31);
        int randomTag = Random.Range(1, 5);

        Debug.Log(randomTag);

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
