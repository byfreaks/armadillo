using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorpseController : MonoBehaviour
{
    public Transform ground; //temporal assignment

    private void Update() {
        if(this.transform.position.y <= ground.transform.position.y){
            if(this.gameObject.TryGetComponent<Rigidbody2D>(out var rb)){
                //Reset
                rb.mass = 1;
                rb.angularVelocity = 0;
                rb.velocity = Vector2.zero;

                //Torque effect
                rb.AddTorque( Random.Range(290,780) );
                
                //this would only offset the speed from the player vehicle
                //For now, it just generates a random number
                var gameVel = Random.Range(-500, -950); 
                var bounce = Random.Range(20, 210);
                rb.AddForce(new Vector2(gameVel, bounce));
            }
        }
    }
}
