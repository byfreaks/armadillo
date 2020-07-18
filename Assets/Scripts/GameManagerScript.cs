using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject vehiclePrefab;
    public Transform groundTransform;

    [SerializeField]
    public Game gameInstance;
    
    void Start()
    {
        gameInstance = new Game(playerPrefab, vehiclePrefab);
        gameInstance.gameState = GameState.RUN_Running;
    }

    public void PlayerIsDead(){
        gameInstance.gameState = GameState.GOV_DeadPlayer;
    }
    
}
