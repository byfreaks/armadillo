using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameHelper
{
    public static GameManagerScript GameManager { get => GameObject.Find("GameManager").GetComponent<GameManagerScript>(); }
    public static PlayerController Player {get => GameObject.Find("Player").GetComponent<PlayerController>(); }

}
