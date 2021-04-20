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
    public bool hasSprite;
    public Color damageColor, gainColor;

    [Header("FeedbackEffect Settings")]
    public float duration = 0.4f;

    private void Awake() {
        this.healthPoints = 100;
        this.maxHealthPoints = 100;
        this.minHealthPoints = 10;
        this.alive = true;
    }

    private void Start() {       
        this.damageColor = Color.red;
        this.gainColor = Color.green;
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

    public void incrementHealthPoints(int points, bool showFeedback = true){
        if(showFeedback)
            this.StartFeedbackEffect(gainColor);

        int newHealthPoints = this.HealthPoints + points;
        this.HealthPoints = newHealthPoints > this.MaxHealthPoints ? this.MaxHealthPoints : newHealthPoints;
    } 

    public void decrementHealthPoints(int points, bool showFeedback = true){
        if(showFeedback)
            this.StartFeedbackEffect();

        this.HealthPoints = this.HealthPoints - points;
    }

    public void killEntity(){
        this.HealthPoints = 0;
    }

    private void StartFeedbackEffect(Color? color = null){
        if(!color.HasValue) color = Color.white;
        
        if(this.gameObject.TryGetComponent<SpriteRenderer>(out var sr)){
            if(sr.sharedMaterial.HasProperty("_Tint")){
                if(coroutine != null){
                    StopCoroutine(coroutine);
                }
                coroutine = FeedbackEffect(sr, color.Value);
                StartCoroutine( coroutine );
            } else {
                Debug.Log($"Health Component FeedbackEffect does not work with {sr.sharedMaterial.name}");
            }
        } else {
            Debug.Log($"Game Object has no Sprite Renderer to apply the effect to");
        }
    }

    private IEnumerator FeedbackEffect(SpriteRenderer sr, Color color){
        sr.sharedMaterial.SetColor("_Tint", color);

        var alpha = 0f;

        for(float i = duration; i>= 0; i -= Time.deltaTime){
            alpha = (i-0) / (duration-0);
            sr.sharedMaterial.SetColor("_Tint", new Color(color.r, color.g, color.b, alpha) );
            
            yield return null;
        }

        StopCoroutine(coroutine);
    }
}
