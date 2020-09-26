using UnityEngine;

[System.Serializable]
public class BuildingRequirements{

    //Directional requirements
    
    [SerializeField] private bool top = false;
    [SerializeField] private bool bottom = false;
    [SerializeField] private bool right = false;
    [SerializeField] private bool left = false;
    [SerializeField] private bool anySide = false;

    public bool Top { get => top; }
    public bool Bottom { get => bottom; }
    public bool Right { get => right; }
    public bool Left { get => left; }
    public bool AnySide { get => anySide; }
}