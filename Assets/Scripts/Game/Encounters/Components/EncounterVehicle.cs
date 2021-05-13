using UnityEngine;
using BitStrap;
using Armadillo.Game.Encounters.Models;

namespace Armadillo.Game.Encounters.Components
{
    public class EncounterVehicle : MonoBehaviour {
        EncounterManager manager;
        NPCVehicle vehicleData;
        bool destroyed = false;
        public void Setup(EncounterManager manager, NPCVehicle vehicleData){
            this.manager = manager;
            this.vehicleData = vehicleData;
        }

        public void Destroyed(){
            manager.VehicleDestroyed(vehicleData.GetType());
        }

        [Button]
        public void Destroy()
        {
            if(destroyed == true)
            {
                Debug.LogError($"Attempted to destroy already destoryed vehicle {this.gameObject.GetInstanceID()}");
                return;
            } else {
                destroyed = true;
                this.Destroyed();
                //TODO: Handle proper vehicle 'destruction' in AI
            }
        }
    }
}