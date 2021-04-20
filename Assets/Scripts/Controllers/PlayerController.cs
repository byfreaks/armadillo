using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PlayerController : MonoBehaviour
{
    #region SETUP
    //Components
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private SpriteRenderer sr;
    private Health hc;
    private Animator ani;
    private InteractableController interactable;
    private DriverActor dac;

    //Weapon
    private WeaponController wc;

    [Header("Graphics")]
    //Sprite Settings
    [SerializeField]
    private Sprite sprite = null;
    //Animation
    public RuntimeAnimatorController animationController;
    public Material material;

    [Header("Temporal")]
    //Weapon Settings
    public WeaponController EquipedWeapon = null;

    [SerializeField]
    private DamageTypes damageFrom = DamageTypes.ENM_DAMAGE;

    Controller2D controller;
    [Header("Movement")]
    [Tooltip("Max height of a jump")]
    public float jumpHeight = 4f;
    [Tooltip("Time to reach jump's highest point")]
    public float timeToJumpApex = .4f;
    private float jumpForce;

    [Tooltip("Max player speed")]
    [SerializeField] private float moveSpeed = 10; 

    //Player Controllers
    private PlayerInput input;

    [Tooltip("Movement acceleration while in mid-air")]
    [SerializeField] float accelerationTimeAirborne = .2f;

    [Tooltip("Movement acceleration while grounded")]
    [SerializeField] float accelerationTimeGrounded = .1f; 

    [Tooltip("Constant downward acceleration")]
    public float gravity = 0;
    Vector3 velocity; //TODO: expose as label

    private float velocityXSmoothing; //never set

    [System.Serializable]
    public struct PlayerStatus{
        public bool dead;
        public bool canMove;
        public bool canShoot;
        public bool grounded;

        public void init(){
            dead = false;
            canMove = true;
            canShoot = false;
            grounded = false;
        }

        public void set_dead(){
            dead = true;
            canMove = false;
            GameHelper.GameManager.PlayerIsDead();
        }

    }
    [SerializeField]
    PlayerStatus status = new PlayerStatus();

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.TryGetComponent<Damage>(out var dmg)){
            if(damageFrom.HasFlag(dmg.type)){
                hc.decrementHealthPoints( dmg.damagePoints );
                if(!hc.IsAlive && !status.dead){
                    HandlePlayerDeath();
                }
            }
        }
    }


    void Awake()
    {
        //PlayerStatus setup
        status.init();

        //Create and save component references
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.material = new Material(material);
        bc = gameObject.AddComponent<BoxCollider2D>();
        hc = gameObject.AddComponent<Health>();
        hc.hasSprite = true;
        input = gameObject.AddComponent<PlayerInput>();
        ani = gameObject.AddComponent<Animator>();
        interactable = gameObject.AddComponent<InteractableController>();

        controller = gameObject.GetComponent<Controller2D>();

        dac = gameObject.GetComponent<DriverActor>();

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

        //Interaction
        interactable.interactableType = InteractableType.Actor;
        interactable.areaSize = new Vector2(4, 4);

        //Animation
        if(animationController != null)
            ani.runtimeAnimatorController = animationController;
    }

    private void Start() {
        //calculate gravity and jumpforce
        //EQ:   jumpHeight = (gravity * timeToJumpApex^2) / 2
        //SOL:  gravity = (2 * jumpHeight) / timeToJumpApex^2
        gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
        //EQ:   jumpVelocity = gravity * timeToJumpApex
        jumpForce = Mathf.Abs(gravity) * timeToJumpApex;

    }
    #endregion

    private void Update() {
        if(dac.isMounted){
            //Handle unmounting conditions
            
            if(dac.ride.TryGetComponent<TurretController>(out var turret)){
                turret.pointTo( input.CursorWorldPos );
                if(InputController.mouseAction(ICActions.key, 0)){
                    turret.shoot();
                }
            }

            if(Input.GetKey(KeyCode.Q)){
                dac.UnMount();
            }

        } else {

            Debug.Log("normal movement");

             //Status
            if(!status.dead && !hc.IsAlive){ HandlePlayerDeath(); }

            //Movement
            this.HandlePlayerMovement();

            //Equipment
            this.HandlePlayerWeapons();

            //Animations
            this.HandlePlayerAnimation();
        }

        
    }

    void HandlePlayerDeath(){
        status.set_dead();
        // this.gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
        CorpseController corpse = gameObject.AddComponent<CorpseController>();
        bc.isTrigger = true;
    }


    void HandlePlayerMovement(){
        if(controller.collisions.above || controller.collisions.below){
            velocity.y = 0;
        }

        if(input.Jump && controller.collisions.below){
            velocity.y = jumpForce;
            
        }

        Vector2 rawInput = new Vector2 (input.AxisHorizontal, 0);
        float targetVelocityX = rawInput.x * moveSpeed;

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandlePlayerWeapons(){
        if(EquipedWeapon){
            //TODO: REMOVE ONCE WEAPON EQUIP HAS BEEN IMPLEMENTED
            var weapon = EquipedWeapon;
            weapon.wielderTransform = this.transform;
            weapon.wielderType = WeaponController.WielderType.player;

            if(InputController.mouseAction(ICActions.key, 1)){
                EquipedWeapon.Set(WeaponCommands.hold, input.CursorDirection(this.transform.position));
                if(InputController.mouseAction(ICActions.keyDown, 0)){
                    EquipedWeapon.Attack(input.GetCursorDirection(this.transform));
                }
            } else if (InputController.mouseAction(ICActions.key, 2)){
                EquipedWeapon.Set(WeaponCommands.point, input.CursorDirection(this.transform.position));
            } else if(Input.GetKey(KeyCode.Alpha2)) {
                EquipedWeapon.Set(WeaponCommands.store, Vector2.zero);
            } else {
                EquipedWeapon.Set(WeaponCommands.sheath, Vector2.zero);
            }
        }
    }

    void HandlePlayerAnimation(){
        ani.SetBool("walking", input.AxisHorizontal != 0); 
        ani.SetBool("grounded", controller.collisions.below);
        ani.SetFloat("vertical_speed", velocity.y);

        sr.flipX = input.CursorWorldPos.x < this.transform.position.x;
    }

}
