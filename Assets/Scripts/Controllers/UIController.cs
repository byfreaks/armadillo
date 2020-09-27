using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject inGameMenu;
    public Slider HUDHealthBar;
    private Health playerHealth;
    [HideInInspector]
    public GameObject player;

    private void Start() { 
        inGameMenu.SetActive(false);
        playerHealth = player.GetComponent<Health>();
        initHealthBar();
    }

    private void Update() {
        this.setHealthBar();
        this.resetHUDHealth();
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

    // Valores de inicio para el HUD.
    private void initHealthBar(){
        HUDHealthBar.value = playerHealth.HealthPoints;
        HUDHealthBar.maxValue = playerHealth.MaxHealthPoints;
        HUDHealthBar.minValue = 0;
    }

    private void resetHUDHealth(){
        if(HUDHealthBar.maxValue != playerHealth.MaxHealthPoints){
            HUDHealthBar.maxValue = playerHealth.MaxHealthPoints;
        }
    }

    // Actualización de la barra de vida en base a los HealthPoints.
    private void setHealthBar(){
        if(HUDHealthBar.value != playerHealth.HealthPoints){
            HUDHealthBar.value = playerHealth.HealthPoints;
            this.setColorHealthBar();
        }
    }

    // Cambio de color en base a la cantidad de vida actual y la referencia de los puntos máximos actuales.
    private void setColorHealthBar(){
        Image HUDColor = HUDHealthBar.fillRect.GetComponent<Image>();
        int healthPoints = playerHealth.HealthPoints;
        int MaxHealthPoints = playerHealth.MaxHealthPoints;

        if(healthPoints <= MaxHealthPoints/3){
            HUDColor.color = Color.red;
        }else if(healthPoints > MaxHealthPoints/3 && healthPoints < MaxHealthPoints/2){
            HUDColor.color = Color.yellow;
        }else{
            HUDColor.color = Color.cyan;
        }
    }
}
