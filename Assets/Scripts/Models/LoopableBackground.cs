using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LoopableBackground
{
    public string name;
    public int? order;
    public float speed;
    public Sprite texture;

    public Vector2 offset { get => new Vector2(speed, 0); }
}
