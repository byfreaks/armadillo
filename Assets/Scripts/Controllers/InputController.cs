using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        this.LeftKey();
    }

    public bool LeftKey(){
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
           Debug.Log("Left Key");
           return true; 
        }else{
            return false;
        }
    }
}
