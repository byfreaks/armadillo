using System.Collections.Generic;
using UnityEngine;

namespace Armadillo.Game.Encounters.Models
{
    [CreateAssetMenu(fileName = "NewEncounterData", menuName = "Encounter"), System.Serializable]
    public class EncounterData : ScriptableObject
    {
        public List<SmallVehicle> SmallVehicles;
        public List<MediumVehicle> MediumVehicles;
    }
}

