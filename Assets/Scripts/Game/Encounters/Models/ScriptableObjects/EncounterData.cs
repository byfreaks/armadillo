using System.Collections.Generic;
using UnityEngine;

namespace Armadillo.Game.Encounters.Models
{
    [CreateAssetMenu(fileName = "NewEncounterData", menuName = "Data/Encounter", order = 0), System.Serializable]
    public class EncounterData : ScriptableObject
    {
        public List<SmallVehicle> SmallVehicles;
        public List<MediumVehicle> MediumVehicles;
    }
}

