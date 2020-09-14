using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public List<LoopableBackground> Backgrounds = new List<LoopableBackground>();
    public Sprite backgroundGradient;
    public Vector2 backgroundGradientScale;

    void Start()
    {
        if(backgroundGradient!=null){
            var obj = new GameObject("BackgroundGradient");
            obj.transform.SetParent(this.transform);
            obj.AddComponent<SpriteRenderer>().sprite = this.backgroundGradient;
            obj.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 50);
            var baseScale = new Vector2(4,1);
            obj.transform.localScale = baseScale * backgroundGradientScale;
        }

        var pos = 1;
        foreach(LoopableBackground background in Backgrounds){
            var obj = GameObject.CreatePrimitive(PrimitiveType.Quad);
            obj.name = background.name == "" ? $"Layer {background.order??pos}" : background.name;
            obj.transform.SetParent(this.transform);
            obj.AddComponent<LoopableBackgroundController>().Build(background, pos++);
        }
    }
}
