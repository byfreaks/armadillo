using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public GameObject player;
    public Slider HUDHealthBar;
    private Health playerHealth;

    public void Awake() {
        this.playerHealth = player.GetComponent<Health>();
        this.initHealthBar();
    }

    private void initHealthBar(){
        HUDHealthBar.value = playerHealth.HealthPoints;
        HUDHealthBar.maxValue = playerHealth.MaxHealthPoints;
        HUDHealthBar.minValue = 0;
    }

    private void setHealthBar(){
        if(HUDHealthBar.value != playerHealth.HealthPoints){
            HUDHealthBar.value = playerHealth.HealthPoints;
            this.setColorHealthBar();
        }
    }

    private void Update() {
        this.setHealthBar();
    }

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
