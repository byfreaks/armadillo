using UnityEngine;

namespace Armadillo.Game.GameInstance.Models.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameBoostrapData", menuName = "Data/GameBoostrapData", order = 0), System.Serializable]

    public class GameBoostrapData : ScriptableObject
    {
        [TextArea] public string textarea;

        [Header("Managers")]
        public GameObject game;
        public GameObject encounter;
        public GameObject building;
        public GameObject background;

        [Header("Objects")]
        public GameObject player;
        public GameObject vehicle;

        [Header("Scene")]
        public GameObject camera;
    }
}