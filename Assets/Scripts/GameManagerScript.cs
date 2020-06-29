using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject vehiclePrefab;

    public GameObject inGameMenu;

    public Game gameInstance;

    void Awake() {
        inGameMenu.SetActive(false);
    }
    
    void Start()
    {
        gameInstance = new Game(playerPrefab, vehiclePrefab);
    }
    
    void Update() {
        if (InputController.Pause(ICActions.keyDown)) {
            pauseMenu();
        }
    }

    public void pauseMenu(){
        if(Time.timeScale == 1.0f){
            Time.timeScale = 0;
            inGameMenu.SetActive(true);
        }else{
            inGameMenu.SetActive(false);
            Time.timeScale = 1.0F;
        }
    }
}
