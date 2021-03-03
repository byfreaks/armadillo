using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vehicle {
  [SerializeField] public GameObject EnemyVehicle;
  [SerializeField] public List<GameObject> Passengers; 
  [SerializeField] public int NumberOfTurrets; 
}
