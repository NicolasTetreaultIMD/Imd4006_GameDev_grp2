using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject[] players = new GameObject[2];
    public bool GameOver;

    // Start is called before the first frame update
    void Start()
    {
        GameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndGame()
    {
        GameOver = true;
    }
}
