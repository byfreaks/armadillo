using System.Collections.Generic;
using Armadillo.Game.Encounters.Models;
using UnityEngine;

namespace Armadillo.Game.Encounters.Models
{
    [System.Serializable]
    public class MediumVehicle : NPCVehicle {
        public GameObject mediumVehicleBase;
        public List<MediumVehicleSegment> vehicleSegments;
        public MediumVehicleApproachDirection approachDirection;
    }
}