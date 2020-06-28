﻿using System.Collections;
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
        Shoot
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
        rb = gameObject.AddComponent<Rigidbody2D>();
        bc = gameObject.AddComponent<BoxCollider2D>();
        targetPosition = target.GetComponent<Rigidbody2D>().position;

        //TEST: These attributes will be set differently
        distanceToAttack = 0.5f;
        moveSpeed = 5f;
        sr.sprite = this.sprite;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        //
        
    }

    // Update is called once per frame
    void Update()
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

    public ContextState getContextState()
    {
        //TODO: Calculate where is the enemy
        return currentContextState;
    }

    public BehaviourState getBehaviourState(ContextState enemyContext, Vector2 targetPosition)
    {
        if(enemyContext == ContextState.SameCar){
            //Close to his target
            if(Mathf.Abs(targetPosition.x - rb.position.x) < distanceToAttack) return BehaviourState.Attack;
            //Far from his target
            else return BehaviourState.FollowTarget;
        }
        else if(enemyContext == ContextState.EnemyCar){
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
            Debug.Log("Siguiendo el objetivo");
            sr.color = new Color32(255,255,255,255);
        }
        else if(enemyBehaviour == BehaviourState.Attack)
        {
            rb.velocity = Vector2.zero;
            Debug.Log("Atacando");
            sr.color = new Color32(230,83,83,90);
        }
        else if(enemyBehaviour == BehaviourState.Drive)
        {
            int action = Random.Range(-1,2);
            Debug.Log("Idle...");
            entityManaged.GetComponent<CarController>().moveTo(action, 7f);
            currentBehaviourState = BehaviourState.Idle;
        }
        else if(enemyBehaviour == BehaviourState.Aim)
        {
            entityManaged.GetComponent<TorretController>().pointTo(targetPosition);
        }
        else if(enemyBehaviour == BehaviourState.Shoot)
        {
            entityManaged.GetComponent<TorretController>().shoot();
            BehaviourState[] values = {BehaviourState.Aim, BehaviourState.Shoot};
            int random = Random.Range(0, values.Length);
            currentBehaviourState = (BehaviourState)  values.GetValue(random);
        }
        else if(enemyBehaviour == BehaviourState.Idle)
            Debug.Log("Idle...");
    }

}
