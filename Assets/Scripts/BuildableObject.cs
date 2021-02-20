using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class BuildableObject : MonoBehaviour
{

    //Components
    private Health hc;
    [HideInInspector]
    public bool isGhost;
    public bool isClippable;
    public bool CanBeBuilt { get => CheckCost(); }
    public BuildingRequirements requirements;
    public BuildingRequirements constraints;
    [Range(0,360)]
    public int surfaceArc;

    private DamageTypes damageFrom = DamageTypes.PLAYER_DAMAGE;

    private void Awake() {
        if(!(transform.name == "BuildingObject"))
            if(isClippable)
                this.gameObject.layer = (int)SystemLayer.BuildableClip;
            else
                this.gameObject.layer = (int)SystemLayer.Buildable;
        hc = GetComponent<Health>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.TryGetComponent<Damage>(out var dmg)){
            if(damageFrom.HasFlag(dmg.type)){
                hc.decrementHealthPoints( dmg.damagePoints );
                if(!hc.IsAlive){
                    Destroy();
                }
            }
        }
    }

    private bool CheckCost(){
        //Checks whether or not the player has enough materials to build the object.
        return true;
    }

    public void Destroy(){
        GameObject.Destroy(this.gameObject, 0.01f);
    }

    /// Builds required objects & components
    public GameObject Build(){

        //Surface arc
        if(surfaceArc != 0){
            this.gameObject.GetComponent<BoxCollider2D>().usedByEffector = true;
            var plat = gameObject.AddComponent<PlatformEffector2D>();
            plat.surfaceArc = surfaceArc;
        }
        
        return this.gameObject;
    }
}
