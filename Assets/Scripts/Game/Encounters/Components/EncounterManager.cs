using System.Collections;
using UnityEngine;
using BitStrap;
using Armadillo.Game.Encounters.Infrastructure;
using Armadillo.Game.Encounters.Models;

namespace Armadillo.Game.Encounters.Components
{
    public class EncounterManager : MonoBehaviour
    {
        public bool debug = false;

        [Tooltip("Max number of small vehicles that can appear on screen on any given time")]
        public int maxSmallVehiclesOnScreen = 3;

        [Tooltip("Max number of medium vehicles that can appear on screen on any given time")]
        public int maxMediumVehiclesOnScreen = 2;

        public Encounter currentEncounter;

        Vector2 randomPosition => Random.Range(0, 2) == 0 ? fixedFrontPosition : fixedBackPosition;

        Vector2 fixedBackPosition = new Vector2(-10f, 5f);
        Vector2 fixedFrontPosition = new Vector2(20f, 5f);

        Transform spawnPos;
        Transform entities;
        Coroutine advanceEncounterCoroutine;
        [SerializeField] float advanceEncounterDelayInSeconds = 3f;

        private void Awake()
        {
            spawnPos = transform.Find("InstantiationPosition");
            entities = transform.Find("Entities");
        }
        #region test
        public EncounterData TEST_encounterData;
        [Button]
        public void TEST_LoadEncounter()
        {
            LoadEncounter(TEST_encounterData);
        }

        #endregion
        #region Encounter Logic
        [Button]
        public void AdvanceEncounter()
        {

            switch (currentEncounter.status)
            {
                case EncounterStatus.unstarted:
                    BeginEncounter();
                    break;

                case EncounterStatus.started:
                    ContinueEncounter();
                    break;

                case EncounterStatus.ended:
                    break;

                default:
                    throw new System.NotImplementedException($"Encounter status {currentEncounter.status} not handled");

            }

        }

        private void BeginEncounter()
        {

            if (currentEncounter != null)
            {
                if (debug) Debug.Log($"ENCOUNTER - BEGIN: {currentEncounter.totalVehicles}");
                currentEncounter.status = EncounterStatus.started;
                AdvanceEncounter();
            }
            else
            {
                Debug.LogError("Attempted to begin encounter before loading one!");
            }
        }

        private void ContinueEncounter()
        {
            var smallToSpawn = Mathf.Clamp(maxSmallVehiclesOnScreen - currentEncounter.smallVehicles.activeVehicles, 0, currentEncounter.smallVehicles.vehiclesLeft);
            var mediumToSpawn = Mathf.Clamp(maxMediumVehiclesOnScreen - currentEncounter.mediumVehicles.activeVehicles, 0, currentEncounter.mediumVehicles.vehiclesLeft);

            if (currentEncounter.smallVehicles.totalCount + currentEncounter.mediumVehicles.totalCount <= 0)
            {
                EndEncounter();
                return;
            }

            if (smallToSpawn > 0)
            {
                SpawnFromQueue(ref currentEncounter.smallVehicles, smallToSpawn);
            }

            if (mediumToSpawn > 0)
            {
                SpawnFromQueue(ref currentEncounter.mediumVehicles, mediumToSpawn);
            }

            if (debug) Debug.Log($"ENCOUNTER - STATUS: Small vehicles: {currentEncounter.smallVehicles.activeVehicles} MediumVehicles: {currentEncounter.mediumVehicles.activeVehicles}");
        }

        private void EndEncounter()
        {
            if (debug) Debug.Log("ENCOUNTER - ENDED");
            currentEncounter.status = EncounterStatus.ended;

            //TODO: Check if necessary 
            // currentEncounter = null; 
        }
        #endregion   
        #region logic
        private void SpawnFromQueue<T>(ref VehicleQueue<T> queue, int amountToSpawn) where T : NPCVehicle
        {
            for (int i = 0; i < amountToSpawn; i++)
            {
                var vehicleData = queue.vehicleQueue.Pop();
                var spawned = VehicleSpawnerService.SpawnVehicle(vehicleData);
                spawned.AddComponent<EncounterVehicle>().Setup(this, vehicleData);
                queue.activeVehicles++;
            }
        }

        private IEnumerator AdvanceAfterSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            AdvanceEncounter();
            advanceEncounterCoroutine = null;
        }

        public void LoadEncounter(EncounterData data)
        {
            currentEncounter = new Encounter(data);
        }

        public void VehicleDestroyed(System.Type vehicleType)
        {
            switch (vehicleType.Name)
            {
                case nameof(SmallVehicle):
                    currentEncounter.smallVehicles.activeVehicles--;
                    break;

                case nameof(MediumVehicle):
                    currentEncounter.mediumVehicles.activeVehicles--;
                    break;

                default:
                    Debug.LogError("Destroyed vehicle type not supported");
                    return;
            }

            if (advanceEncounterCoroutine != null)
            {
                StopCoroutine(advanceEncounterCoroutine);
                advanceEncounterCoroutine = null;
            }
            advanceEncounterCoroutine = StartCoroutine(AdvanceAfterSeconds(advanceEncounterDelayInSeconds));
        }
        #endregion
    }
}