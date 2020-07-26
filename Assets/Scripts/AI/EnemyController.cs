using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class EnemyController : MonoBehaviour
{
    //Components
    public Rigidbody2D rb;
    public BoxCollider2D bc;
    public SpriteRenderer sr;
    public Health hc;
    public CorpseController corpse;

    //Enemy Logic Values
    private EnemyBehaviour currentBehaviour;
    public EnemyBehaviour nextBehaviour;
    public EnemyContext currentContext;
    public EnemyObjective currentObjective;

    //Entity to be managed by him
    public GameObject entityManaged;

    //Attributes
    public float closeDistance;
    public float moveSpeed;
    public Sprite sprite;
    public DamageTypes damageFrom;
    
    // Start is called before the first frame update
    void Start()
    {
        //Create and save component references
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = this.sprite;
        rb = gameObject.AddComponent<Rigidbody2D>();
        bc = gameObject.AddComponent<BoxCollider2D>();
        hc = gameObject.AddComponent<Health>();

        //Enemy Logic values
        currentBehaviour = new Idle(this);
        currentBehaviour.init();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check if it is necessary a transition to a new Behaviour
        if(nextBehaviour != null) doBehaviourTransition();
        else
        {           
            //Calculate next behaviour (when is Idle)
            if(currentBehaviour.getBehaviourName() == "Idle") calculateNextBehaviour();

            else currentBehaviour.update();
        }
    }

    //Enemy logic
    void calculateNextBehaviour()
    {
        if(currentContext == EnemyContext.SameCar && currentObjective == EnemyObjective.MeleeAttack) nextBehaviour = new MoveToTarget(this, GameObject.Find("Player"));
    }

    //Helper (Move to a static helper?)
    void doBehaviourTransition()
    {
        currentBehaviour.final();
        //WaitSeconds or something?
        currentBehaviour = nextBehaviour;
        nextBehaviour = null;
        currentBehaviour.init();
    }

    //Damage detection
    void OnTriggerEnter2D(Collider2D other) {
        if(other.TryGetComponent<Damage>(out var dmg)){
            if(damageFrom.HasFlag(dmg.type)){
                hc.decrementHealthPoints( dmg.damagePoints );
                if(!hc.IsAlive && currentBehaviour.getBehaviourName() != "Dead"){
                    nextBehaviour = new Dead(this);
                }
            }
        }
    }
}
