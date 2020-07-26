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
    public List<GameObject> passengers;
    public GameObject passengerSeat;
    public float distanceBetweenSeats;

    [Header("Driver")]
    public GameObject driver;
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
        //
    }

    // Update is called once per frame
    void Update()
    {
        //Update entities positions
        if(passengers.Count > 0)
        {
            for(int i=0; i<passengers.Count;i++)
                passengers[i].transform.position = new Vector2(passengerSeat.transform.position.x - (distanceBetweenSeats*i),passengerSeat.transform.position.y);
        }
        if(driver != null) driver.transform.position = driverSeat.transform.position;
    }

    public void moveTo(int direction, float speed){
        rb.velocity = new Vector2(speed * direction, rb.velocity.y);
    }

    public void linkAsPassenger(GameObject passenger){
        this.passengers.Add(passenger);
    }

    public void linkAsDriver(GameObject driver){
        this.driver = driver;
    }

    public void unlinkPassenger(GameObject passenger){
        this.passengers.Remove(passenger);
    }

    public void unlinkDriver(){
        this.driver = null;
    }

}
