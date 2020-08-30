using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    //[REVIEW] Should projectile properties be part of this controller?
    [Header("Properties")]
    [SerializeField] private Vector2 direction;
    [SerializeField] private float firingRate;
    [SerializeField] private bool canShoot;
    [SerializeField] private float projectileSpeed; //[REVIEW]
    [SerializeField] private int projectileDamage; //[REVIEW]
    [SerializeField] private GameObject projectileTemplate = null;
    
    //Components
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private SpriteRenderer sr;

    [Header("Shooter")]
    [SerializeField] private GameObject shooter;
    [SerializeField] private GameObject shooterSeat = null;

    #region Unity Engine Loop Methods     
    void Start()
    {
        //Create and save component references
        //sr = gameObject.AddComponent<SpriteRenderer>(); //[TODO] Replace Prefab SpriteRenderer with this SpriteRenderer
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        bc = gameObject.AddComponent<BoxCollider2D>();
    }
    void Update()
    {
        if(IsActive) shooter.transform.position = shooterSeat.transform.position; //Shooter position updated to turret position
    }
    #endregion

    #region Turret Usage Methods
    public bool mount(GameObject shooter)
    {
        if(!IsActive)
        {
            this.shooter = shooter;
            return true;
        }
        else
            return false; 
    }
    public void unMount()
    {
        shooter = null;
    }
    public void pointTo(Vector2 point)
    {
        direction = point - rb.position; //[TODO] Visual aiming of the turret
    }
    public void shoot(){
        if(!canShoot) return;

        GameObject projectile = Instantiate(projectileTemplate);
        //[REVIEW] Set Damage component
        projectile.AddComponent<Damage>();
        projectile.GetComponent<Damage>().setDamage(DamageTypes.ENM_BULLET, projectileDamage);
        projectile.transform.position = transform.position;
        projectile.GetComponent<ProjectileController>().Setup(direction, projectileSpeed);
        //
        StartCoroutine(Cooldown());
    }
    #endregion

    #region Turret Logic
    public IEnumerator Cooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(firingRate);
        canShoot = true;
    }
    #endregion

    #region Setters&Getters
    public void constructor(Vector3 position, float firingRate,int projectileDamage, float projectileSpeed)
    {
        transform.position = position;
        this.firingRate = firingRate;
        this.projectileDamage = projectileDamage;
        this.projectileSpeed = projectileSpeed;
        canShoot = true;
    }
    public bool IsActive { get { return (shooter != null); }  }
    #endregion
}
