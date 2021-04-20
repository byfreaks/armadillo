using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Events;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    [HideInInspector]
    public GameObject playerPrefab;
    public GameObject vehiclePrefab;
    public Transform groundTransform;
    public UIController uiController;
    public GameObject gameOverPanelPrefab;

    private GameObject canvas;
    private GameObject gameOverPanel;
    
    [SerializeField]
    public Game gameInstance;
    
    void Start()
    {
        playerPrefab = GameHelper.Player.gameObject;

        if(uiController)
            uiController.player = playerPrefab;

        gameInstance = new Game(playerPrefab, vehiclePrefab);
        SetGameState(GameState.RUN_Running);

        canvas = GameObject.Find("Canvas");

        if(gameOverPanelPrefab != null){
            gameOverPanel = Instantiate(gameOverPanelPrefab, canvas.transform.position, Quaternion.identity, canvas.transform);
            gameOverPanel.GetComponentInChildren<Button>().onClick.AddListener( () => this.BackToMenu() );
            gameOverPanel.SetActive(false);
        }
    }

    public void PlayerIsDead(){
       SetGameState(GameState.GOV_DeadPlayer);
    }

    public void EngineDestroyed(){
        SetGameState(GameState.GOV_VehicleDestroyed);
    }
    
    void Update() {
        if (InputController.Pause(ICActions.keyDown) && uiController) {
            uiController.pauseMenu();
        }
    }

    void EvaluateState(){
        if(GameState.GAMEOVER.HasFlag(gameInstance.gameState)){
            if(gameOverPanel != null)
                gameOverPanel.SetActive(true);
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

    private void OnDestroy() {
        GameHelper.ClearCache();    
    }
}
