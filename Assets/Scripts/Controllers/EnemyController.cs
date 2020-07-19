using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //TODO: These definitions must be part of other Manager/Controller/Class
    public enum BehaviourState
    {
        FollowTarget,
        Attack,
        Idle,
        Drive,
        Aim,
        Shoot,
        Dead
    }
    public enum ContextState
    {
        SameCar,
        EnemyCar
    }

    //Components
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private SpriteRenderer sr;
    private Health hc;
    private CorpseController corpse;

    //States
    public BehaviourState currentBehaviourState;
    public ContextState currentContextState;

    //Target
    public GameObject target;
    private Vector2 targetPosition;

    //Entity to be managed by him
    public GameObject entityManaged;

    //Attributes
    private float distanceToAttack;
    private float moveSpeed;
    private int moveDirection;
    public Sprite sprite;
    
    // Start is called before the first frame update
    void Start()
    {
        //Create and save component references
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = this.sprite;
        rb = gameObject.AddComponent<Rigidbody2D>();
        bc = gameObject.AddComponent<BoxCollider2D>();
        hc = gameObject.AddComponent<Health>();
        targetPosition = target.GetComponent<Rigidbody2D>().position;

        //TEST: These attributes will be set differently
        distanceToAttack = 0.5f;
        moveSpeed = 5f;
        //
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentBehaviourState != BehaviourState.Dead)
        {
            //Update ref to his target
            targetPosition = target.GetComponent<Rigidbody2D>().position;
            
            //Check where is the enemy
            currentContextState = getContextState();

            //Check what should do the enemy
            currentBehaviourState = getBehaviourState(currentContextState, targetPosition);

            //Execute state action 
            updateByState(currentBehaviourState, targetPosition);
        }
    }

    public ContextState getContextState()
    {
        //TODO: Calculate where is the enemy
        return currentContextState;
    }

    public BehaviourState getBehaviourState(ContextState enemyContext, Vector2 targetPosition)
    {
        if(!hc.IsAlive)
        {
            return BehaviourState.Dead;
        }
        else if(enemyContext == ContextState.SameCar)
        {
            //Close to his target
            if(Mathf.Abs(targetPosition.x - rb.position.x) < distanceToAttack) return BehaviourState.Attack;
            //Far from his target
            else return BehaviourState.FollowTarget;
        }
        else if(enemyContext == ContextState.EnemyCar)
        {
            //TODO
            return currentBehaviourState;
        }    
        return BehaviourState.Idle;
    }

    public void updateByState(BehaviourState enemyBehaviour, Vector2 targetPosition){
        if(enemyBehaviour == BehaviourState.FollowTarget)
        {
            moveDirection = (targetPosition.x - rb.position.x > 0) ? 1 : -1;           
            rb.velocity = new Vector2(moveSpeed * moveDirection, rb.velocity.y);
            Debug.Log("FollowTarget...");
            sr.color = new Color32(255,255,255,255);
        }
        else if(enemyBehaviour == BehaviourState.Attack)
        {
            rb.velocity = Vector2.zero;
            Debug.Log("Attack...");
            sr.color = new Color32(230,83,83,90);
        }
        else if(enemyBehaviour == BehaviourState.Drive)
        {
            entityManaged.GetComponent<CarController>().moveTo(1, 7f);
            Debug.Log("Drive...");
        }
        else if(enemyBehaviour == BehaviourState.Aim)
        {
            entityManaged.GetComponent<TorretController>().pointTo(targetPosition);
            Debug.Log("Aim...");
        }
        else if(enemyBehaviour == BehaviourState.Shoot)
        {
            entityManaged.GetComponent<TorretController>().shoot();
            Debug.Log("Shoot!");
            currentBehaviourState = BehaviourState.Aim;
        }
        else if(enemyBehaviour == BehaviourState.Dead)
        {
            if(corpse == null) corpse = gameObject.AddComponent<CorpseController>();
            bc.isTrigger = true;
            Debug.Log("Dead!");
        }
        else if(enemyBehaviour == BehaviourState.Idle)
            Debug.Log("Idle...");
    }

}
