using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Components
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private SpriteRenderer sr;
    private Health hc;

    //Movement Settings
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpForce;

    //Sprite Settings
    [SerializeField]
    private Sprite sprite;

    //Attack Settings
    //temporal
    [SerializeField]
    private GameObject attackMask;
    private GameObject atk;

    [SerializeField] 
    private GameObject testProjectile;

    [SerializeField]
    public struct PlayerStatus{
        public bool dead;
        public bool canMove;

        public void set_dead(){
            dead = true;
            canMove = false;
            GameHelper.GameManager.PlayerIsDead();
        }
    }
    PlayerStatus status = new PlayerStatus();
    

    void Start()
    {

        //PlayerStatus setup
        status.dead = false;
        status.canMove = true;

        //Create and save component references
        sr = gameObject.AddComponent<SpriteRenderer>();
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        bc = gameObject.AddComponent<BoxCollider2D>();
        hc = gameObject.AddComponent<Health>();

        //Setup
        //TODO: consider setting up component as they are created to declutter code

        //SpriteRenderer settings
        if(sprite != null) sr.sprite = this.sprite;

        //BoxCollider settings
        if(sprite != null){
            //Save old BC settings
            Destroy(this.bc);
            bc = gameObject.AddComponent<BoxCollider2D>();
            //Add old settings to new BC component
        }
    }

    void Update()
    {

        if(!status.dead && !hc.IsAlive){
            status.set_dead();
            this.gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
            CorpseController corpse = gameObject.AddComponent<CorpseController>();
            bc.isTrigger = true;
        }


        //Movement
        if(status.canMove)
            rb.velocity = new Vector2( InputController.HorizontalMovement() * moveSpeed ,rb.velocity.y);

        //Jump
        if(InputController.Jump(ICActions.keyDown) && status.canMove){
            rb.AddForce( new Vector2(0, jumpForce) );
        }

        if(InputController.MeleeAttack(ICActions.keyDown) && attackMask != null && atk == null){
            atk = Instantiate(attackMask);
            atk.AddComponent<Damage>();
            atk.GetComponent<Damage>().setDamage(DamageTypes.PLY_MELEE, 10);
            atk.transform.parent = this.transform;
            //set postiion (temporal)
            var mePos = this.transform.position;
            atk.transform.position = new Vector2(mePos.x + 1, mePos.y);
        }

        if(InputController.Shot(ICActions.keyDown) && testProjectile != null){
            var proj = Instantiate(testProjectile);
            proj.AddComponent<Damage>();
            proj.GetComponent<Damage>().setDamage(DamageTypes.PLY_BULLET, 20);
            proj.transform.position = this.transform.position;
            proj.GetComponent<ProjectileController>().Setup( Camera.main.ScreenToWorldPoint(InputController.MousePosition()) - this.transform.position, 19.8f  );
        }

    }
}
