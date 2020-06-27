using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class BuildableObject : MonoBehaviour
{

    public bool CanBeBuilt { get => CheckCost(); }
    public BuildingRequirements requirements;
    [Range(0,360)]
    public int surfaceArc;

    private bool CheckCost(){
        //Checks whether or not the player has enough materials to build the object.
        return true;
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
