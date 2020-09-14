using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuManager : MonoBehaviour
{

    public GameObject optionsPanel;
    public GameObject controlsPanel;
    public string firstScene;

    public void moveTo(GameObject panel){
        this.closeAllPanels();
        panel.SetActive(true);
    }

    public void closeGame(){
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void startGame(){
        SceneManager.LoadScene(firstScene);
    }

    public void LoadScene(string sceneName){
        if(GameScenes.TryParse(sceneName, true, out GameScenes scene))
            SceneHelper.LoadScene(scene);
    }

    private void closeAllPanels(){
        this.optionsPanel.SetActive(false);
        this.controlsPanel.SetActive(false); 
    }
}
