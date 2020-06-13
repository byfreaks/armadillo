using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject vehiclePrefab;

    public Game gameInstance;
    
    void Start()
    {
        gameInstance = new Game(playerPrefab, vehiclePrefab);
    }
    
}
