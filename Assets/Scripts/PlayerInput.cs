using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInput : MonoBehaviour
{
    public bool Shoot { get => InputController.Shot(ICActions.keyDown); }
    public bool Jump { get=> InputController.Jump(ICActions.keyDown); }
    public float AxisHorizontal { get => InputController.HorizontalMovement(); }
    public Vector3 CursorWorldPos{ get=> Camera.main.ScreenToWorldPoint(InputController.MousePosition()); }

    public Vector3 GetCursorDirection(Transform originTransform){
        return CursorWorldPos - originTransform.position;
    }

    public Vector3 CursorDirection(Vector2 origin){
        //TODO: remove this legacy method (still in use)
        var dir = Camera.main.ScreenToWorldPoint(InputController.MousePosition()) - (Vector3)origin;
        var distance = 1.5f;
        dir += new Vector3(0,0,-1);
        return dir.normalized * distance;
    }

}
