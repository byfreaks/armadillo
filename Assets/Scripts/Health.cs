using UnityEngine;

[ExecuteInEditMode]
public class Health : MonoBehaviour
{
    private int healthPoints;
    private int maxHealthPoints;
    private int minHealthPoints;
    private bool alive;

    private void Awake() {
        this.IsAlive = true;
        this.HealthPoints = 100;
        this.MaxHealthPoints = 100;
        this.MinHealthPoints = 10;
    }

    public int HealthPoints{
        get { return healthPoints; }
        set { healthPoints = value; }
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

    public void incrementHealthPoints(int points){
        this.HealthPoints = this.HealthPoints + points;
    } 

    public void decrementHealthPoints(int points){
        this.HealthPoints = this.HealthPoints - points;
    }

    public void killEntity(){
        this.HealthPoints = 0;
    }
}
