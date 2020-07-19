using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameHelper
{
    public static GameManagerScript GameManager { get => GetGameManager(); }
    public static Game GameInstance { get => GetGameManager().gameInstance; }
    
    private static GameManagerScript GetGameManager(){
        return GameObject.Find("GameManager").GetComponent<GameManagerScript>();
    }
}
