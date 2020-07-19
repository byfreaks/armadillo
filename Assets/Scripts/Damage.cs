using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {  
    public DamageTypes type;
    public int points;

    public int damagePoints { 
        get{
            return points;
        }
        set{
            points = value;
        } 
    }

    public void setDamage(DamageTypes type, int points){
        this.type = type;
        this.points = points;
    }
}
