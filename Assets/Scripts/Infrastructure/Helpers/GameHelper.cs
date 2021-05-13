using System.Collections;
using System.Collections.Generic;
using Armadillo.Game.GameInstance.Components;
using UnityEngine;

public static class GameHelper
{
    private static GameManagerScript gameManager;
    public static GameManagerScript GameManager { get => gameManager ?? GetGameManager(); set => gameManager = value; }
    private static GameManagerScript GetGameManager(){ return gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>(); }
    
    private static PlayerController player;
    public static PlayerController Player { get => player ?? GetPlayerController(); set => player = value; }
    private static PlayerController GetPlayerController(){ return player = GameObject.Find("Player").GetComponent<PlayerController>(); }

    public static void ClearCache(){
        gameManager = null;
        player = null;
    }
}
