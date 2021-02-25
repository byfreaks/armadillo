using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "ScriptedEncounter"), System.Serializable]
public class Encounter : ScriptableObject
{
    public List<Vehicle> Vehicles;
}
