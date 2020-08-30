using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class EnemyController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float contactDistance;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool onGround;
    [SerializeField] private Sprite sprite = null;
    [SerializeField] private DamageTypes damageFrom = DamageTypes.PLAYER_DAMAGE;

    [Header("Components")]
    public Rigidbody2D rb;
    public BoxCollider2D bc;
    public SpriteRenderer sr;
    public Health hc;
    public CorpseController corpse;
    public WeaponController EquipedWeapon = null;

    [Header("AI Properties")]
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private EnemyContext currentContext;
    [SerializeField] private GameObject currentVehicle;
    [SerializeField] private bool blockUpdate = false;
    private EnemyBehaviour currentBehaviour;

    #region Unity Engine Loop Methods 
    void Start()
    {
        //Create and save component references
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = this.sprite;
        rb = gameObject.AddComponent<Rigidbody2D>();
        bc = gameObject.AddComponent<BoxCollider2D>();
        hc = gameObject.AddComponent<Health>();
        //Weapon [REVIEW] 
        EquipedWeapon.wielderTransform = this.transform;
        EquipedWeapon.Set(WeaponCommands.store);

        //Calculate First Behaviour
        calculateNextBehaviour();
    }
    void FixedUpdate()
    {
        getCurrentVehicle();
    }
    void Update()
    {
        if(blockUpdate) return;
        CurrentBehaviour.update();
    }
    #endregion
    
    #region Collisions Methods
    //Damage detection
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.TryGetComponent<Damage>(out var dmg)){
            if(damageFrom.HasFlag(dmg.type)){
                hc.decrementHealthPoints( dmg.damagePoints );
                //[AI TRANSITION]: Dead
                if(!hc.IsAlive && CurrentBehaviour.getBehaviourName() != "Dead"){
                    StartCoroutine(
                        BehaviourTransition(
                            nextBehaviour: new Dead(this, currentBehaviour.cc)
                        )
                    );
                }
            }
        }
    }
    //Current Vehicle detection
    void getCurrentVehicle()
    {
        RaycastHit2D[] hits = new RaycastHit2D[2];
        Physics2D.RaycastNonAlloc(transform.position,Vector2.down,hits,2f);
        if (hits[1].collider != null)
        {
            Debug.DrawLine(transform.position,(Vector2) transform.position + (hits[1].distance * Vector2.down),Color.red);
            currentVehicle = hits[1].collider.gameObject;
            onGround = true;
            if(currentVehicle.name == "Vehicle" && currentContext != EnemyContext.SameCar) CurrentContext = EnemyContext.SameCar; //[HARDCODE]
        }
        else
        {
            currentVehicle = null;
            onGround = false;
        }
    }
    #endregion

    #region AI Methods
    //Method called when currentContext or enemyType are modified
    void calculateNextBehaviour()
    {
        //[AI TRANSITION]: SameCar && Fighter => MoveToTarget
        if(CurrentContext == EnemyContext.SameCar && EnemyType == EnemyType.Fighter) 
            StartCoroutine(
                BehaviourTransition(
                    nextBehaviour: new MoveToTarget(this, GameObject.Find("Player")),
                    secondsBefore: 1f
                )
            );
        //[AI TRANSITION]: OtherCar && Fighter => Passenger
        else if(CurrentContext == EnemyContext.OtherCar && EnemyType == EnemyType.Fighter && currentVehicle!=null) 
            StartCoroutine(
                BehaviourTransition(
                    nextBehaviour: new Passenger(this,currentVehicle.GetComponent<CarController>())
                )
            );
        //[AI TRANSITION]: OtherCar && Driver => DriveInZone
        else if(CurrentContext == EnemyContext.OtherCar && EnemyType == EnemyType.Driver) 
            StartCoroutine(
                BehaviourTransition(
                    nextBehaviour: new DriveInZone(this,currentVehicle.GetComponent<CarController>())
                )
            );
        //[AI TRANSITION]: Default => Idle
        else
            StartCoroutine(
                BehaviourTransition(
                    nextBehaviour: new Idle(this)
                )
            );
    }
    public IEnumerator BehaviourTransition(EnemyBehaviour nextBehaviour, float secondsBefore = 0, float secondsDuring = 0,float secondsAfter = 0)
    {
        blockUpdate = true;
        if(currentBehaviour != null) Debug.Log("Transition from: " + currentBehaviour.getBehaviourName() + " to " + nextBehaviour.getBehaviourName()); //[DEBUG]
        if(secondsBefore > 0) yield return new WaitForSeconds(secondsBefore);
        if(currentBehaviour!=null) currentBehaviour.final(); //END CURRENT BEHAVIOUR
        if(secondsDuring > 0) yield return new WaitForSeconds(secondsDuring);
        currentBehaviour = nextBehaviour; //SET NEW BEHAVIOUR
        currentBehaviour.init(); //INIT BEHAVIOUR
        if(secondsAfter > 0) yield return new WaitForSeconds(secondsAfter);
        blockUpdate = false;
    }
    #endregion
    
    #region Setters&Getters
    public void constructor(Vector3 position, float moveSpeed, EnemyContext context, EnemyType enemyType, GameObject vehicle = null)
    {
        transform.position = position;
        this.MoveSpeed = moveSpeed;
        this.currentContext = context;
        this.enemyType = enemyType;
        this.currentVehicle = vehicle;            
    }
    public float ContactDistance {set { contactDistance = value; } get { return contactDistance; }}
    public float MoveSpeed {set { moveSpeed = value; } get { return moveSpeed; }}
    public bool BlockUpdate {set { blockUpdate = value; } }
    public bool OnGround {get { return onGround; } }
    public EnemyType EnemyType
    {
        set
        {
            enemyType = value;
            calculateNextBehaviour();
        }
        get { return enemyType; }
    }
    public EnemyContext CurrentContext
    {
        set
        {
            currentContext = value;
            calculateNextBehaviour();
        }
        get { return currentContext; }
    }
    public EnemyBehaviour CurrentBehaviour {get { return currentBehaviour; } }
    #endregion
}
