using UnityEngine;
using Armadillo.Game.Encounters.Models;

namespace Armadillo.Game.Encounters.Infrastructure
{
    public static class VehicleSpawnerService
    {
        //TODO: remove hardcode of default spawns
        public static Vector3 BackSpawnPosition => new Vector3(-7, 5, 5);
        public static Vector3 FrontSpawnPosition => new Vector3(20, 5, 5);
        public static Vector3 RandomPosition => Random.Range(0, 1) == 0 ? BackSpawnPosition : FrontSpawnPosition;

        private static Transform entitiesParent;
        public static Transform EntitiesParent
        {
            get
            {
                if (entitiesParent == null)
                    return entitiesParent = GameObject.Find("Entities").transform;
                else
                    return entitiesParent;
            }
        }

        public static GameObject SpawnVehicle(NPCVehicle vehicle, Vector3? position = null, Transform parent = null)
        {
            if (vehicle.hasPrefab)
            {
                return GameObject.Instantiate(vehicle.overridePrefab, position ?? RandomPosition, Quaternion.identity, parent ?? EntitiesParent);
            }
            else
            {
                switch (vehicle.GetType().Name)
                {
                    case nameof(SmallVehicle):
                        return BuildVehicle(vehicle as SmallVehicle, position ?? RandomPosition, parent ?? EntitiesParent);

                    case nameof(MediumVehicle):
                        return BuildVehicle(vehicle as MediumVehicle, position ?? RandomPosition, parent ?? EntitiesParent);

                    default:
                        throw new System.Exception("Vehicle type not supported");
                }
            }
        }

        public static GameObject BuildVehicle(SmallVehicle vehicle, Vector3 position, Transform parent)
        {
            throw new System.NotImplementedException("Small vehicle building is not yet supported");
        }

        public static GameObject BuildVehicle(MediumVehicle vehicle, Vector3 position, Transform parent)
        {
            //TODO: place medium vehicle base + segment instantiation logic here
            throw new System.NotImplementedException("Small vehicle building is not yet supported");
        }
    }
}