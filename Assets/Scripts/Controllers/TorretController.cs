using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorretController : MonoBehaviour
{
    
    //Components
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private SpriteRenderer sr;

    [Header("Shooter")]
    public Rigidbody2D shooter;
    public GameObject shooterSeat;

    [Header("Attributes")]
    private Vector2 direction;
    public float projectileSpeed;
    public GameObject projectileTemplate;

    // Start is called before the first frame update
    void Start()
    {
        //Create and save component references
        //sr = gameObject.AddComponent<SpriteRenderer>();
        rb = gameObject.AddComponent<Rigidbody2D>();
        bc = gameObject.AddComponent<BoxCollider2D>();

        //TEST: These attributes will be set differently
        rb.bodyType = RigidbodyType2D.Kinematic;
        projectileSpeed = 20f;
        //
    }

    // Update is called once per frame
    void Update()
    {
        //Update entity position
        if(shooter != null) shooter.position = shooterSeat.transform.position;
    }

    public void pointTo(Vector2 point){
        direction = point - rb.position;
    }

    public void mount(Rigidbody2D shooter){
        this.shooter = shooter;
    }

    public void unMount(){
        shooter = null;
    }

    public void shoot(){
        GameObject projectile = Instantiate(projectileTemplate);
        projectile.transform.position = transform.position;
        projectile.GetComponent<ProjectileController>().Setup(direction, projectileSpeed);
    }

}
