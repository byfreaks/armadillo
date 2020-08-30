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
    public bool boardingPosition = false;

    [Header("Driver")]
    public GameObject driver;
    public GameObject driverSeat;

    #region Unity Engine Loop Methods 
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
    #endregion

    #region Methods to operate the entity
    public bool linkAsDriver(GameObject driver)
    {
        if(this.driver == null)
        {
            this.driver = driver;
            return true;
        }
        return false;
    }
    public void unlinkDriver()
    {
        //[TODO]: Validate that the entity which ask for unlink is the current driver
        this.driver = null;
        //[REVIEW]: This isn't the right place to do this operation
        if(passengers.Count > 0)
            passengers[0].GetComponent<EnemyController>().EnemyType = EnemyType.Driver;
    }
    public void linkAsPassenger(GameObject passenger)
    {
        //[TODO] Limit passengers seats
        this.passengers.Add(passenger);
    }
    public void unlinkPassenger(GameObject passenger)
    {
        this.passengers.Remove(passenger);
    }
    public void linkAsWeapon(GameObject weapon)
    {
        this.weapons.Add(weapon);
    }
    public void moveTo(Vector2 direction, float speed)
    {
        rb.velocity = speed * direction;
    }
    #endregion

    #region Methods to query the state of the entity
    public bool hasDriver()
    {
        return (driver != null);
    }
    public int getNumberOfPassangers()
    {
        return this.passengers.Count;
    }
    public int getNumberOfActiveWeapons()
    {
        //[REVIEW]: Find another way to do this query
        int count = 0;
        for (int i=0;i<weapons.Count;i++)
            if(weapons[i].GetComponent<TurretController>().IsActive) count++;
        return count;
    }
    #endregion
}