using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class EnemyController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float contactDistance;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Sprite sprite = null;
    [SerializeField] private DamageTypes damageFrom = DamageTypes.PLAYER_DAMAGE;

    [Header("Components")]
    public Rigidbody2D rb;
    public BoxCollider2D bc;
    public SpriteRenderer sr;
    public Health hc;
    public CorpseController corpse;

    [Header("AI Properties")]
    [SerializeField] private EnemyObjective currentObjective;
    [SerializeField] private EnemyContext currentContext;
    [SerializeField] private GameObject currentVehicle;
    private EnemyBehaviour currentBehaviour;
    private bool blockUpdate = false;

    #region Unity Engine Loop Methods 
    // Start is called before the first frame update
    void Start()
    {
        //Create and save component references
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = this.sprite;
        rb = gameObject.AddComponent<Rigidbody2D>();
        bc = gameObject.AddComponent<BoxCollider2D>();
        hc = gameObject.AddComponent<Health>();

        //Calculate First Behaviour
        calculateNextBehaviour();
    }

    //Fixed Update (Physics)
    void FixedUpdate()
    {
        getCurrentVehicle();
    }

    // Update is called once per frame
    void Update()
    {
        if(!blockUpdate)
        { 
            CurrentBehaviour.update();
            if(!hc.IsAlive) CurrentBehaviour = new Dead(this); //[TEST ONLY]
        }
    }
    #endregion
    
    #region Collisions Methods
    //Damage detection
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.TryGetComponent<Damage>(out var dmg)){
            if(damageFrom.HasFlag(dmg.type)){
                hc.decrementHealthPoints( dmg.damagePoints );
                if(!hc.IsAlive && CurrentBehaviour.getBehaviourName() != "Dead"){
                    CurrentBehaviour = new Dead(this);
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
        }
        else
            currentVehicle = null;
    }
    #endregion

    #region AI Methods
    //Method called when currentContext or currentObjective are modified
    void calculateNextBehaviour()
    {
        if(CurrentContext == EnemyContext.SameCar && CurrentObjective == EnemyObjective.MeleeAttack) CurrentBehaviour = new MoveToTarget(this, GameObject.Find("Player"));
        if(CurrentContext == EnemyContext.OtherCar && CurrentObjective == EnemyObjective.Drive) CurrentBehaviour = new Driver(this,currentVehicle.GetComponent<CarController>());
        if(CurrentContext == EnemyContext.OtherCar && CurrentObjective == EnemyObjective.MeleeAttack) CurrentBehaviour = new Passenger(this,currentVehicle.GetComponent<CarController>());
    }
   
    IEnumerator BehaviourTransitionTime()
    {
        blockUpdate = true;
        yield return new WaitForSeconds(1); //[HARDCODE]
        blockUpdate = false;
    }
    #endregion
    
    #region Setters&Getters
    public void enemyConstructor(Vector3 position, float moveSpeed, EnemyContext context, EnemyObjective objective, GameObject vehicle = null)
    {
        transform.position = position;
        this.MoveSpeed = moveSpeed;
        this.currentContext = context;
        this.currentObjective = objective;
        this.currentVehicle = vehicle;
    }
    public float ContactDistance {set { contactDistance = value; } get { return contactDistance; }}
    public float MoveSpeed {set { moveSpeed = value; } get { return moveSpeed; }}
    public bool BlockUpdate {set { blockUpdate = value; } }
    public EnemyObjective CurrentObjective
    {
        set
        {
            currentObjective = value;
            calculateNextBehaviour();
        }
        get { return currentObjective; }
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
    public EnemyBehaviour CurrentBehaviour
    {
        set
        {
            if(currentBehaviour!=null) currentBehaviour.final();
            StartCoroutine(BehaviourTransitionTime());
            currentBehaviour = value;
            currentBehaviour.init();
        }
        get { return currentBehaviour; }
    }
    #endregion
}
