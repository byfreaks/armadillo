using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public List<LoopableBackground> Backgrounds = new List<LoopableBackground>();

    void Start()
    {
        var pos = 1;
        foreach(LoopableBackground background in Backgrounds){
            var obj = GameObject.CreatePrimitive(PrimitiveType.Quad);
            obj.name = background.name == "" ? $"Layer {background.order??pos}" : background.name;
            obj.transform.SetParent(this.transform);
            obj.AddComponent<LoopableBackgroundController>().Build(background, pos++);
        }
    }
}
