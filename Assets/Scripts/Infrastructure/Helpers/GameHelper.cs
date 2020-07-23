using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameHelper
{
    private static GameManagerScript gameManager;
    public static GameManagerScript GameManager { get => gameManager ?? GetGameManager(); set => gameManager = value; }
    
    private static PlayerController player;
    public static PlayerController Player { get => player ?? GetPlayerController(); set => player = value; }

    private static GameManagerScript GetGameManager(){
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        return gameManager;
    } 

    private static PlayerController GetPlayerController(){
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        return player;
    }

    public static void ClearCache(){
        gameManager = null;
        player = null;
    }
}
