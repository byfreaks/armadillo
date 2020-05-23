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
    [SerializeField]

    //Sprite Settings
    private Sprite sprite;

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

    }
}
