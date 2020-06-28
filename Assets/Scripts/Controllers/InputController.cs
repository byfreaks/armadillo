using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputController
{
    public static bool Left(ICActions inputType){
        return keyAction(inputType, KeyCode.A);
    }

    public static bool Right(ICActions inputType){
        return keyAction(inputType, KeyCode.D);
    }

    public static bool Up(ICActions inputType){
        return keyAction(inputType, KeyCode.W);
    }

    public static bool Down(ICActions inputType){
        return keyAction(inputType, KeyCode.S);
    }

    public static bool Jump(ICActions inputType){
        return keyAction(inputType, KeyCode.Space);
    }

    public static bool Build(ICActions inputType){
        return keyAction(inputType, KeyCode.B);
    }

    public static bool Reload(ICActions inputType){
        return keyAction(inputType, KeyCode.R);
    }

    public static bool Shot(ICActions inputType){
        return mouseAction(inputType, 1);
    }

    // Acciones de keyboard
    public static bool keyAction(ICActions inputType, KeyCode key){
        switch (inputType)
        {
            case ICActions.key:
                return !(Input.GetKey(key));
            case ICActions.keyDown:
                return !(Input.GetKeyDown(key));
            case ICActions.keyUp:
                return !(Input.GetKeyUp(key));
            default:
                return false;
        }
    }

    // Acciones de mouse
    public static bool mouseAction(ICActions inputType, int key){
        switch (inputType)
        {
            case ICActions.key:
                return !(Input.GetMouseButton(key));
            case ICActions.keyDown:
                return !(Input.GetMouseButtonDown(key));
            case ICActions.keyUp:
                return !(Input.GetMouseButtonUp(key));
            default:
                return false;
        }
    }
}
