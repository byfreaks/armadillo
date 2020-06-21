using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Vector2 Direction { get; private set; }
    public float Speed { get; private set; }
    
    private bool isSetup = false;
    
    public void Setup(Vector2 dir, float spd){
        if(isSetup) return;
        //Add components
        var bc = this.gameObject.AddComponent<BoxCollider2D>();
        var rb = this.gameObject.AddComponent<Rigidbody2D>();
        bc.isTrigger = rb.isKinematic = true;
        //Set Speed
        rb.velocity = dir.normalized * spd;
        var ad = this.gameObject.AddComponent<AutoDestroy>();
        ad.TimeToDestroyInSeconds = 2f;

    }
}
