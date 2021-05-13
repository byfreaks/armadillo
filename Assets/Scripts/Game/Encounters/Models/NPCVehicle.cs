using UnityEngine;

namespace Armadillo.Game.Encounters.Models
{
    public abstract class NPCVehicle {
        [Tooltip("If provided, Encounter Manager will spawn prefab instad of building the vehicle using other settings!")]
        public GameObject overridePrefab;
        public bool hasPrefab => overridePrefab != null;
    }
}