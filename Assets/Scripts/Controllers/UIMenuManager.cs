using UnityEngine.SceneManagement;
using UnityEngine;

public class UIMenuManager : MonoBehaviour
{

    public GameObject optionsPanel;
    public GameObject controlsPanel;
    public string firstScene;

    void Awake()
    {
        this.optionsPanel.SetActive(true);
        this.controlsPanel.SetActive(false);        
    }

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

    private void closeAllPanels(){
        this.optionsPanel.SetActive(false);
        this.controlsPanel.SetActive(false); 
    }
}
