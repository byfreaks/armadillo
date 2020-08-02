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
    [SerializeField] private bool blockUpdate = false;

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
        if(blockUpdate) return;
        CurrentBehaviour.update();
        if(!hc.IsAlive && CurrentBehaviour.getBehaviourName() != "Dead") 
            StartCoroutine(
                BehaviourTransition(
                    nextBehaviour: new Dead(this)
                )
            );
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
                    StartCoroutine(
                        BehaviourTransition(
                            nextBehaviour: new Dead(this)
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
        }
        else
            currentVehicle = null;
    }
    #endregion

    #region AI Methods
    //Method called when currentContext or currentObjective are modified
    void calculateNextBehaviour()
    {
        if(CurrentContext == EnemyContext.SameCar && CurrentObjective == EnemyObjective.MeleeAttack) 
            StartCoroutine(
                BehaviourTransition(
                    nextBehaviour: new MoveToTarget(this, GameObject.Find("Player")),
                    secondsBefore: 1f
                )
            );
        if(CurrentContext == EnemyContext.OtherCar && CurrentObjective == EnemyObjective.MeleeAttack && currentVehicle!= null) 
            StartCoroutine(
                BehaviourTransition(
                    nextBehaviour: new Passenger(this,currentVehicle.GetComponent<CarController>()),
                    secondsBefore: 1f
                )
            );
        if(CurrentContext == EnemyContext.OtherCar && CurrentObjective == EnemyObjective.Drive) 
            StartCoroutine(
                BehaviourTransition(
                    nextBehaviour: new Driver(this,currentVehicle.GetComponent<CarController>()),
                    secondsBefore: 1f
                )
            );
        else
            StartCoroutine(
                BehaviourTransition(
                    nextBehaviour: new Idle(this),
                    secondsBefore: 1f
                )
            );
    }
   
    public IEnumerator BehaviourTransition(EnemyBehaviour nextBehaviour, float secondsBefore = 0, float secondsDuring = 0,float secondsAfter = 0)
    {
        blockUpdate = true;
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
    public EnemyBehaviour CurrentBehaviour {get { return currentBehaviour; } }
    #endregion
}
