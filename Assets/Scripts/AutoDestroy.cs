using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    Coroutine DestroyRoutine;
    public float TimeToDestroyInSeconds = 1;
    
    void Start()
    {
        if(DestroyRoutine == null)
            DestroyRoutine = StartCoroutine("DestroyOn", TimeToDestroyInSeconds);
        else
            print("Houston we've got a problem: Coroutine Not Null");
    }
    bool StopDestroy(){
        try
        {
            StopCoroutine(DestroyRoutine);
            return true;
        }
        catch (System.Exception ex)
        {
            print(ex.Message);
            return false;
        }
    }
    
    bool DestroyNow(){
        try
        {
            GameObject.Destroy(this.gameObject);
            return true;
        }
        catch (System.Exception ex)
        {
            print(ex.Message);
            return false;
        }
    }

    IEnumerator DestroyOn(float seconds){
        yield return new WaitForSeconds(seconds);
        GameObject.Destroy(this.gameObject);
    }

    private void OnDestroy() {
        //Make sure coroutine is dead
        StopCoroutine(DestroyRoutine);
    }
}
