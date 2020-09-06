using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class CarController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private int numberOfPassengerSeats;
    [SerializeField] private int numberOfWeaponSeats;
    [SerializeField] private bool inBoardingPosition = false;
    [SerializeField] private GameObject driverSeat = null;
    [SerializeField] private GameObject passengerSeat = null;
    [SerializeField] private float distanceBetweenSeats = 0;
    
    //Components
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private SpriteRenderer sr;

    [Header("Entities related")]
    [SerializeField] private GameObject driver;
    [SerializeField] private List<GameObject> weapons = null;
    [SerializeField] private List<GameObject> passengers = null;
    
    #region Unity Engine Loop Methods 
    void Start()
    {
        //Create and save component references
        //sr = gameObject.AddComponent<SpriteRenderer>(); //[TODO] Replace Prefab SpriteRenderer with this SpriteRenderer
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        bc = gameObject.AddComponent<BoxCollider2D>();
    }
    void Update()
    {
        //Update entities positions
        if(NumberOfWeapons > 0 || NumberOfCurrentPassengers > 0)
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

    #region Car Usage Methods
    public bool linkAsDriver(GameObject driver)
    {
        if(!HasDriver)
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
    }
    public bool linkAsPassenger(GameObject passenger)
    {
        if(NumberOfCurrentPassengers < numberOfPassengerSeats)
        {
            passengers.Add(passenger);
            return true;
        }
        return false;
    }
    public void unlinkPassenger(GameObject passenger)
    {
        this.passengers.Remove(passenger);
    }
    public bool linkAsWeapon(GameObject weapon)
    {
        if(NumberOfWeapons < numberOfWeaponSeats)
        {
            weapons.Add(weapon);
            return true;
        }
        return false;
    }
    public void moveTo(Vector2 direction, float speed)
    {
        rb.velocity = speed * direction;
    }
    #endregion

    #region Setters&Getters
    public void constructor(Vector3 position, int numberOfPassengerSeats, int numberOfWeaponSeats)
    {
        transform.position = position;
        this.numberOfPassengerSeats = numberOfPassengerSeats;
        this.numberOfWeaponSeats = numberOfWeaponSeats;
    }
    public bool HasDriver { get { return (driver != null); } }
    public bool InBoardingPosition { set { inBoardingPosition = value; } get { return inBoardingPosition; } }
    public int NumberOfCurrentPassengers { get {return passengers.Count; } }
    public int NumberOfWeapons { get {return weapons.Count; } }
    public int NumberOfActiveWeapons
    {
        get
        {
            int count = 0;
            for (int i=0;i<weapons.Count;i++)
                if(weapons[i].GetComponent<TurretController>().IsActive) count++;
            return count;
        }
    }
    public TurretController NextAvailableWeaponController
    {
        get
        {
            TurretController nextTurret;
            for (int i=0;i<weapons.Count;i++)
            {
                nextTurret = weapons[i].GetComponent<TurretController>();
                if(!nextTurret.IsActive) return nextTurret;
            }
            return null;
        }
    }
    #endregion
}