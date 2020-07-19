using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {  
    public DamageTypes name;
    public int points;

    public int damagePoints { 
        get{
            return points;
        }
        set{
            points = value;
        } 
    }

    public void setDamage(DamageTypes name, int points){
        this.name = name;
        this.points = points;
    }
}
