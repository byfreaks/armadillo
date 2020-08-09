using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInput : MonoBehaviour
{
    public bool Shoot { get => InputController.Shot(ICActions.keyDown); }
    public bool Jump { get=> InputController.Jump(ICActions.keyDown); }
    public float AxisHorizontal { get => InputController.HorizontalMovement(); }

    public Vector3 GetCursorDirection(Transform originTransform){
        return Camera.main.ScreenToWorldPoint(InputController.MousePosition()) - originTransform.position;
    }
}
