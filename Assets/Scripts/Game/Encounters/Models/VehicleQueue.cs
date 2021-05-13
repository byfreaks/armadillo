using System.Collections.Generic;

namespace Armadillo.Game.Encounters.Models
{
    [System.Serializable]
    public class VehicleQueue<T> where T : NPCVehicle
    {
        public List<T> vehicleQueue = new List<T>();
        public int activeVehicles;
        public int vehiclesLeft => vehicleQueue.Count;
        public int totalCount => vehiclesLeft + activeVehicles;
        public void Add(T vehicle)
        {
            vehicleQueue.Add(vehicle);
        }
        public void Add(List<T> vehicles)
        {
            vehicleQueue.AddRange(vehicles);
        }
    }
}