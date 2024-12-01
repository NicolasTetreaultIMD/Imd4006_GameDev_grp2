using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHider : MonoBehaviour
{
    public GameObject[] hide;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideObjects()
    {
        foreach (GameObject go in hide)
        {
            go.SetActive(false);
        }
    }

    public void ShowObjects()
    {
        foreach (GameObject go in hide)
        {
            go.SetActive(true);
        }
    }

}
