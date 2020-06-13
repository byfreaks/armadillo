using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    //Components
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private SpriteRenderer sr;

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

    void Start()
    {
        //Create and save component references
        sr = gameObject.AddComponent<SpriteRenderer>();
        rb = gameObject.AddComponent<Rigidbody2D>();
        bc = gameObject.AddComponent<BoxCollider2D>();

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
        //Movement
        rb.velocity = new Vector2( Input.GetAxis("Horizontal") * moveSpeed ,rb.velocity.y);

        //Jump
        if(Input.GetKeyDown(KeyCode.Space)){
            //TODO: create input class
            rb.AddForce( new Vector2(0, jumpForce) );
        }

        if(Input.GetKeyDown(KeyCode.E) && attackMask != null && atk == null){
            atk = Instantiate(attackMask);
            atk.transform.parent = this.transform;
            //set postiion (temporal)
            var mePos = this.transform.position;
            atk.transform.position = new Vector2(mePos.x + 1, mePos.y);
        }

        if(Input.GetMouseButtonDown(1) && testProjectile != null){
            var proj = Instantiate(testProjectile);
            proj.transform.position = this.transform.position;
            proj.GetComponent<ProjectileController>().Setup( Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position, 19.8f  );
        }

    }
}
