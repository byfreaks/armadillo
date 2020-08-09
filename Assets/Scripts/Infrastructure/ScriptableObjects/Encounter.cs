using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "ScriptedEncounter"), System.Serializable]
public class Encounter : ScriptableObject
{
    public GameObject Vehicle;
    public List<GameObject> Passangers;
}
