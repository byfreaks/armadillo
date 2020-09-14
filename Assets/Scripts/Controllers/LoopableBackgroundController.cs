using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopableBackgroundController : MonoBehaviour
{
    private LoopableBackground background;

    private Material material;
    private MeshRenderer meshRenderer;

    public void Build(LoopableBackground bg, int pos){
        material = new Material(Shader.Find("Unlit/Transparent"));
        material.mainTexture = bg.texture.texture;

        gameObject.GetComponent<MeshRenderer>().material = material;
        gameObject.transform.position = Camera.main.transform.position;
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, bg.order??pos );
        gameObject.transform.localScale = new Vector3(20, 12, 1); //FIXME: Hardcode

        background = bg;
    }

    private void Update() {
        if(background!=null)
            material.mainTextureOffset += background.offset * Time.deltaTime;
    }
}