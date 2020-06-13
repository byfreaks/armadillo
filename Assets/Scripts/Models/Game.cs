using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    //Partida
    public GameObject playerObj; //Player Preset
    public GameObject vehicleObj;
    public Camera cameraObj;

    [Header("Temporal settings")]
    //Temporal
    public Sprite testSolidSprite;

    public Game(GameObject player = null, GameObject vehicle = null){
        if(player!=null){
            playerObj = player;
        } else {
            playerObj = new GameObject("Player");
            playerObj.transform.position = new Vector3(0, 0, -10);
            playerObj.AddComponent<PlayerController>();
        }
        
        if(vehicle != null){
            vehicleObj = vehicle;
        } else {
            vehicleObj = new GameObject("TestSolid");
            vehicleObj.transform.position = new Vector3(0, 10, -10);
            vehicleObj.AddComponent<SpriteRenderer>().sprite = testSolidSprite;
            vehicleObj.AddComponent<BoxCollider2D>();
            vehicleObj.AddComponent<Rigidbody2D>().isKinematic = true;
        }
        

    }
}
