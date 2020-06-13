using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    
    //Components
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private SpriteRenderer sr;

    [Header("Passenger")]
    public Rigidbody2D passenger;
    public GameObject passengerSeat;

    [Header("Driver")]
    public Rigidbody2D driver;
    public GameObject driverSeat;

    // Start is called before the first frame update
    void Start()
    {
        //Create and save component references
        //sr = gameObject.AddComponent<SpriteRenderer>();
        rb = gameObject.AddComponent<Rigidbody2D>();
        bc = gameObject.AddComponent<BoxCollider2D>();

        //TEST: These attributes will be set differently
        rb.bodyType = RigidbodyType2D.Kinematic;
        moveTo(1, 2.5f);
        //
    }

    // Update is called once per frame
    void Update()
    {
        //Update entities positions
        if(passenger != null) passenger.position = passengerSeat.transform.position;
        if(driver != null) driver.position = driverSeat.transform.position;
    }

    public void moveTo(int direction, float speed){
        rb.velocity = new Vector2(speed * direction, rb.velocity.y);
    }

    public void linkAsPassenger(Rigidbody2D passenger){
        this.passenger = passenger;
    }

    public void linkAsDriver(Rigidbody2D driver){
        this.driver = driver;
    }

}
