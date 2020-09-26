using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugTools
{
    public static void DrawBounds(Bounds box, Color? color = null){
        var bounds = box;
        //Left line
        Debug.DrawLine( new Vector2(bounds.min.x, bounds.min.y), new Vector2(bounds.min.x, bounds.max.y), color??Color.yellow );

        //Right line
        Debug.DrawLine( new Vector2(bounds.max.x, bounds.min.y), new Vector2(bounds.max.x, bounds.max.y), color??Color.yellow );

        //Bottom line
        Debug.DrawLine( new Vector2(bounds.min.x, bounds.min.y), new Vector2(bounds.max.x, bounds.min.y), color??Color.yellow );

        //Top line
        Debug.DrawLine( new Vector2(bounds.min.x, bounds.max.y), new Vector2(bounds.max.x, bounds.max.y), color??Color.yellow );
    }
}
