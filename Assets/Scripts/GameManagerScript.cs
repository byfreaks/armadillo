using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject vehiclePrefab;
    public Transform groundTransform;
    public GameObject inGameMenu;
    
    [SerializeField]
    public Game gameInstance;

    void Awake() {
        inGameMenu.SetActive(false);
    }
    
    void Start()
    {
        gameInstance = new Game(playerPrefab, vehiclePrefab);
        SetGameState(GameState.RUN_Running);
    }

    public void PlayerIsDead(){
       SetGameState(GameState.GOV_DeadPlayer);
    }

    public void EngineDestroyed(){
        SetGameState(GameState.GOV_VehicleDestroyed);
    }
    
    void Update() {
        if (InputController.Pause(ICActions.keyDown)) {
            pauseMenu();
        }
    }

    void EvaluateState(){
        if(GameState.GAMEOVER.HasFlag(gameInstance.gameState)){
            //game is over
            BackToMenu();
        }
    }

    GameState SetGameState(GameState state, bool evaluate = true){
        gameInstance.gameState = state;
        if(evaluate)
            EvaluateState();
        return gameInstance.gameState;
    }

    void BackToMenu(){
        SceneHelper.LoadScene(GameScenes.main_menu);
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
