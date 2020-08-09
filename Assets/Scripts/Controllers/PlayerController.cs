﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Components
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private SpriteRenderer sr;
    private Health hc;

    //Weapon
    private WeaponController wc;

    //Movement Settings
    [SerializeField]
    private float moveSpeed = 10;
    [SerializeField]
    private float jumpForce = 260;

    //Sprite Settings
    [SerializeField]
    private Sprite sprite = null;

    //Player Controllers
    private PlayerInput input;

    //Weapon Settings
    public WeaponController EquipedWeapon = null;

    private DamageTypes damageFrom = DamageTypes.ENM_DAMAGE;

    [SerializeField]
    public struct PlayerStatus{
        public bool dead;
        public bool canMove;
        public bool canShoot;

        public void init(){
            dead = false;
            canMove = true;
            canShoot = false;
        }

        public void set_dead(){
            dead = true;
            canMove = false;
            GameHelper.GameManager.PlayerIsDead();
        }
    }
    PlayerStatus status = new PlayerStatus();
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.TryGetComponent<Damage>(out var dmg)){
            if(damageFrom.HasFlag(dmg.type)){
                hc.decrementHealthPoints( dmg.damagePoints );
                if(!hc.IsAlive && !status.dead){
                    PlayerDeath();
                }
            }
        }
    }

    void Start()
    {

        //PlayerStatus setup
        status.init();

        //Create and save component references
        sr = gameObject.AddComponent<SpriteRenderer>();
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        bc = gameObject.AddComponent<BoxCollider2D>();
        hc = gameObject.AddComponent<Health>();
        input = gameObject.AddComponent<PlayerInput>();

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

    void PlayerDeath(){
        status.set_dead();
        this.gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
        CorpseController corpse = gameObject.AddComponent<CorpseController>();
        bc.isTrigger = true;
    }

    void Update()
    {

        if(!status.dead && !hc.IsAlive){
            status.set_dead();
            this.gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
            CorpseController corpse = gameObject.AddComponent<CorpseController>();
            bc.isTrigger = true;
        }

        if(status.canMove)
            rb.velocity = new Vector2(input.AxisHorizontal * moveSpeed, rb.velocity.y);

        //Jump
        if(input.Jump && status.canMove){
            rb.AddForce( new Vector2(0, jumpForce) );
        }

        //Handle weapons
        if(EquipedWeapon!=null){
            //Equip
            //TODO: create and user a Wield() in weaponcontroller method instead
            var weapon = EquipedWeapon;
            weapon.wielderTransform = this.transform;
            weapon.Wielder = WeaponController.wielder.player;

            if(InputController.mouseAction(ICActions.key, 1)){
                weapon.Set(WeaponCommands.hold);
                if(InputController.mouseAction(ICActions.keyDown, 0)){
                    weapon.Attack(input.GetCursorDirection(this.transform));
                }
            } else if (InputController.mouseAction(ICActions.key, 2)){
                weapon.Set(WeaponCommands.point);
            } else if(Input.GetKey(KeyCode.Alpha2)) {
                weapon.Set(WeaponCommands.store);
            } else {
                weapon.Set(WeaponCommands.sheath);
            }
        }

    }
}
