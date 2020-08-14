using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Health : MonoBehaviour
{
    [SerializeField]
    private int healthPoints;
    [SerializeField]
    private int maxHealthPoints;
    [SerializeField]
    private int minHealthPoints;
    private bool alive;
    private GameObject damageFeedback;
    private IEnumerator coroutine;

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
        coroutine = activeDamageFeedback();
        StartCoroutine(coroutine);
        this.HealthPoints = this.HealthPoints - points;
    }

    public void killEntity(){
        this.HealthPoints = 0;
    }

    public void setDamageFeedback(GameObject feedback){
        this.damageFeedback = feedback;
    }

    public IEnumerator activeDamageFeedback(){
        var df = this.damageFeedback.GetComponent<SpriteRenderer>();
        df.enabled = false;
        df.enabled = true;
        for (float ft = 1f; ft >= 0; ft -= 0.09f) 
        {
            Color c = df.color;
            c.a = ft;
            df.color = c;
            yield return new WaitForSeconds(.1f);
        }
    }
}
