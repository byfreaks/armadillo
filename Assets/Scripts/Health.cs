using UnityEngine;

[ExecuteInEditMode]
public class Health : MonoBehaviour
{
    private int healthPoints;
    private int maxHealthPoints;
    private int minHealthPoints;
    private bool alive;

    private void Awake() {
        this.healthPoints = 100;
        this.maxHealthPoints = 100;
        this.minHealthPoints = 10;
        this.alive = true;
    }

    //Propiedad que sirve para verificar si una entidad está viva tomando en cuenta los puntos de vida de la misma.
    public bool IsAlive{
        get { 
            return this.HealthPoints <= 0 ? false:true;
        }
        set { alive = value; }
    }

    public int MaxHealthPoints{
        get { return maxHealthPoints; }
        set { maxHealthPoints = value; }
    }

    public int MinHealthPoints{
        get { return minHealthPoints; }
        set { minHealthPoints = value; }
    }

    public int HealthPoints{
        get { return healthPoints; }
        set {
            if(value >= this.MaxHealthPoints){
                healthPoints = this.MaxHealthPoints;
            }else if(value <= 0){
                healthPoints = 0;
            }else{
                healthPoints = value; 
            }
        }
    }

    public void incrementHealthPoints(int points){
        int newHealthPoints = this.HealthPoints + points;
        this.HealthPoints = newHealthPoints > this.MaxHealthPoints ? this.MaxHealthPoints : newHealthPoints;
    } 

    public void decrementHealthPoints(int points){
        this.HealthPoints = this.HealthPoints - points;
    }

    public void killEntity(){
        this.HealthPoints = 0;
    }
}
