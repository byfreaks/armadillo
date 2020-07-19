using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    //public Transform socket;
    [Range(0,4000)]
    public float rotationSpeed;
    private float oldRotationSpeed;
    private Rigidbody2D rb;

    private void Start() {
        //Add rigidbody
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.mass = 10;
        rb.angularDrag = 0;
        rb.gravityScale = 0;

        //Add rotation torque
        rb.AddTorque(-rotationSpeed);
        oldRotationSpeed = rotationSpeed;
    }

    private void Update() {
        if(oldRotationSpeed != rotationSpeed){
            //RotationChanged
            rb.angularVelocity = 0;
            rb.AddTorque(-rotationSpeed);
            oldRotationSpeed = rotationSpeed;
        }
    }
}
