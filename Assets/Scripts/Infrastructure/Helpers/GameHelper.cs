using System.Collections;
using System.Collections.Generic;
using Armadillo.Game.GameInstance.Components;
using UnityEngine;

public static class GameHelper
{
    private static GameManager gameManager;
    public static GameManager GameManager { get => gameManager ?? GetGameManager(); set => gameManager = value; }
    private static GameManager GetGameManager(){ return gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); }
    
    private static PlayerController player;
    public static PlayerController Player { get => player ?? GetPlayerController(); set => player = value; }
    private static PlayerController GetPlayerController(){ return player = GameObject.Find("Player").GetComponent<PlayerController>(); }

    public static void ClearCache(){
        gameManager = null;
        player = null;
    }
}
