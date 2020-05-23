using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public Game gameInstance;
    
    void Start()
    {
        gameInstance = new Game();
    }
    
}
