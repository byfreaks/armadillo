using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class CarController : MonoBehaviour
{
    
    //Components
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private SpriteRenderer sr;

    [Header("Passenger")]
    public List<GameObject> passengers;
    public List<GameObject> weapons;
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
            int i=0;
            for(;i<weapons.Count;i++)
                weapons[i].transform.position = new Vector2(passengerSeat.transform.position.x - (distanceBetweenSeats*i),passengerSeat.transform.position.y);
            for(int j=0;j<passengers.Count;j++,i++)
                passengers[j].transform.position = new Vector2(passengerSeat.transform.position.x - (distanceBetweenSeats*i),passengerSeat.transform.position.y);
        }
        if(driver != null) driver.transform.position = driverSeat.transform.position;
        
        //Behaviour when there isn't driver
        if(driver == null)
            moveTo(Vector2.left,10f);
    }

    //Driver methods
    public void linkAsDriver(GameObject driver){
        this.driver = driver;
    }
    public void unlinkDriver(){
        this.driver = null;
        if(getNumberOfPassangers() > 0)
            passengers[0].GetComponent<EnemyController>().CurrentObjective = EnemyObjective.Drive;
    }
    public void moveTo(Vector2 direction, float speed){
        rb.velocity = speed * direction;
    }
    public bool hasDriver()
    {
        return (driver != null);
    }
    //Passengers methods
    public void linkAsPassenger(GameObject passenger){
        this.passengers.Add(passenger);
    }
    public void unlinkPassenger(GameObject passenger){
        this.passengers.Remove(passenger);
    }
    public int getNumberOfPassangers(){
        return this.passengers.Count;
    }

    //Weapon methods
    public void linkAsWeapon(GameObject weapon){
        this.weapons.Add(weapon);
    }
    public int getNumberOfActiveWeapons()
    {
        int count = 0;
        for (int i=0;i<weapons.Count;i++)
            if(weapons[i].GetComponent<TorretController>().isActive()) count++;
        return count;
    }

}
