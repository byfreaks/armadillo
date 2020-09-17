using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PointHelper
{
    static public void PointAtTarget(Transform center, Vector3 target, bool invertScaleY = false){
        var targetPos = target;
        targetPos.x = targetPos.x - center.position.x;
        targetPos.y = targetPos.y - center.position.y;
        var angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        center.rotation = Quaternion.Euler(new Vector3(0,0, angle));
        if(invertScaleY){
            if(target.x > center.position.x ){
                center.localScale = new Vector3(1, 1,1);
            } else {
                center.localScale = new Vector3(1,-1,1);
            }
        }
    }
}
