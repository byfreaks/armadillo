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
        Idle
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

    //States
    private BehaviourState currentBehaviourState;
    private ContextState currentContextState;

    //Target
    public GameObject target;
    private Vector2 targetPosition;

    //Attributes
    public float distanceToAttack;
    public float moveSpeed;
    public Sprite sprite;
    
    // Start is called before the first frame update
    void Start()
    {
        //Create and save component references
        sr = gameObject.AddComponent<SpriteRenderer>();
        rb = gameObject.AddComponent<Rigidbody2D>();
        bc = gameObject.AddComponent<BoxCollider2D>();
        targetPosition = target.GetComponent<Transform>().position;

        //TEST: These attributes will be set differently
        distanceToAttack = 3f;
        moveSpeed = 1.5f;
        sr.sprite = this.sprite;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        //
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check where is the enemy
        currentContextState = getContextState();

        //Check what should do the enemy
        currentBehaviourState = getBehaviourState(currentContextState, targetPosition);

        //Execute state action 
        updateByState(currentBehaviourState, targetPosition);
    }

    public ContextState getContextState()
    {
        //TODO: Calculate where is the enemy
        return ContextState.SameCar;
    }

    public BehaviourState getBehaviourState(ContextState enemyContext, Vector2 targetPosition)
    {
        if(enemyContext == ContextState.SameCar){
            //Close to his target
            if(Mathf.Abs(targetPosition.x - transform.position.x) < distanceToAttack) return BehaviourState.Attack;
            //Far from his target
            else return BehaviourState.FollowTarget;
        }
        
        //TODO: else if(enemyContext == ContextState.EnemyCar){}    
        return BehaviourState.Idle;
    }

    public void updateByState(BehaviourState enemyBehaviour, Vector2 targetPosition){
        if(enemyBehaviour == BehaviourState.FollowTarget)
        {
            int moveDirectionX = (targetPosition.x - rb.position.x > 0) ? 1 : -1;           
            rb.velocity = new Vector2(moveSpeed * moveDirectionX, rb.velocity.y);
            Debug.Log("Siguiendo el objetivo");
        }
        else if(enemyBehaviour == BehaviourState.Attack)
        {
            rb.velocity = Vector2.zero;
            sr.color = new Color32(230,83,83,90);
            Debug.Log("Atacando");
        }
        else if(enemyBehaviour == BehaviourState.Idle)
            Debug.Log("Idle...");
    }

}
