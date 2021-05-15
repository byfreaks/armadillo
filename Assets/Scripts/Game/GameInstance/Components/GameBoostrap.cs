using UnityEngine;
using Armadillo.Game.Encounters.Components;
using Armadillo.Game.GameInstance.Models.ScriptableObjects;
using Armadillo.Game.GameInstance.Infrastructure;

namespace Armadillo.Game.GameInstance.Components
{
    struct inst
    {
        public GameManager game;
        public EncounterManager encounter;
        public BackgroundManager background;
        public BuildingManager building;

        public GameObject vehicle;
        public GameObject player;

        public GameObject camera;
    }

    struct pos
    {
        public Vector3 center  => new Vector3(10, 6, -5);
        public Vector3 cam     => new Vector3(10, 6, -30);
        public Vector3 vehicle => new Vector3(10, 4.5f, -10);
        public Vector3 player  => new Vector3(10, 8, -5);
        
        public Vector3 center2(float zpos) => new Vector3(10, 6, zpos);
    }

    public class GameBoostrap : MonoBehaviour
    {
        inst inst;
        pos pos;
        [SerializeField] GameBoostrapData data;
        private void Awake() {
            this.SetupScene();
        }

        private void SetupScene()
        {
            // ### INSTANTIATE
            // Managers
            var manFolder = new GameObject("Managers").transform;

            inst.game       = Inst<GameManager>(data.game, manFolder);
            inst.encounter  = Inst<EncounterManager>(data.encounter, manFolder);
            inst.background = Inst<BackgroundManager>(data.background, manFolder);
            inst.building   = Inst<BuildingManager>(data.building, manFolder);

            // Objects
            inst.vehicle = Inst(data.vehicle).Pos(pos.vehicle);
            inst.player  = Inst(data.player).Pos(pos.player);

            // Scene
            inst.camera = Inst(data.camera);
            
            // ### CONFIG
            // Managers
            inst.background.gameObject.Pos(pos.center);
            inst.building.VehicleObject = inst.vehicle;

            //Objects

            //Scenes
            inst.camera.transform.position = pos.cam;

        }

        private T Inst<T>(GameObject reference, Transform parent = null)
            => Instantiate(reference, Vector3.zero, Quaternion.identity, parent)
                .ClearName("(Clone)")
                .GetComponent<T>();

        private GameObject Inst(GameObject obj, Transform parent = null)
            => Instantiate(obj, Vector3.zero, Quaternion.identity, parent)
                .ClearName("(Clone)");

    }
}
