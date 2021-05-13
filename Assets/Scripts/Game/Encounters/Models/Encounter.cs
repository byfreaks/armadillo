using Armadillo.Game.Encounters.Infrastructure;

namespace Armadillo.Game.Encounters.Models
{
    [System.Serializable]
    public class Encounter
    {
        public Encounter(EncounterData data)
        {
            data.SmallVehicles.ForEach(v => smallVehicles.Add(v));
            data.MediumVehicles.ForEach(v => mediumVehicles.Add(v));
        }
        public EncounterStatus status = EncounterStatus.unstarted;
        public int totalVehicles => smallVehicles.totalCount + mediumVehicles.totalCount;
        public VehicleQueue<SmallVehicle> smallVehicles = new VehicleQueue<SmallVehicle>();
        public VehicleQueue<MediumVehicle> mediumVehicles = new VehicleQueue<MediumVehicle>();
    }
}