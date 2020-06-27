using UnityEngine;

[System.Serializable]
public class BuildingRequirements{

    //Directional requirements
    
    [SerializeField] private bool top;
    [SerializeField] private bool bottom;
    [SerializeField] private bool right;
    [SerializeField] private bool left;

    public bool Top { get => top; }
    public bool Bottom { get => bottom; }
    public bool Right { get => right; }
    public bool Left { get => left; }
    public bool Sides { get => Left && Right; }
}