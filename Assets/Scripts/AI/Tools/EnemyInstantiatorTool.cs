using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI{
    [ExecuteInEditMode]
    public class EnemyInstantiatorTool : MonoBehaviour
    {


        [Header("Templates")]
        public GameObject prefabEnemy;
        public GameObject prefabCar;
        public GameObject prefabTorret;
        [HideInInspector] public Encounter scriptedEncounter = null;

        [Header("Enemy Properties")]
        public float enemySpeed;
        public EnemyContext context;
        public EnemyObjective objective;

        [Header("Enemy Car Properties")]
        public int numberOfPassengers;
        public int numberOfTorrets;

        public void CreateFromEncounter(Encounter enc){
            //TODO: proper encounter instantiator
            //This method is a HACK. Instantiatior tools should have mehtods to instantiate objects using outside reference (maybe :) )
            this.prefabCar = enc.Vehicle;
            var vehicle = createCar();
            foreach(GameObject passenger in enc.Passangers){
                this.prefabEnemy = enc.Passangers[0];
                createEnemy(5f, EnemyContext.OtherCar, EnemyObjective.RangeAttack, vehicle);
            }
        }

        public GameObject createEnemy()
        {
            GameObject enemy = Instantiate(prefabEnemy);
            EnemyController ec = enemy.GetComponent<EnemyController>();
            ec.enemyConstructor(transform.position,enemySpeed,context,objective);

            return enemy;
        }

        public GameObject createEnemy(float enemySpeed, EnemyContext context, EnemyObjective objective, GameObject vehicle = null)
        {
            GameObject enemy = Instantiate(prefabEnemy);
            EnemyController ec = enemy.GetComponent<EnemyController>();
            ec.enemyConstructor(transform.position,enemySpeed,context,objective,vehicle);

            return enemy;
        }

        public GameObject createCar()
        {
            GameObject enemyCar = Instantiate(prefabCar);
            CarController cc = enemyCar.GetComponent<CarController>();

            //Set values
            enemyCar.transform.position = transform.position;
            //Create Driver
            createEnemy(5f,EnemyContext.OtherCar,EnemyObjective.Drive, enemyCar);
            //Create Torrets
            for(int i=0; i<numberOfTorrets;i++)
                cc.linkAsPassenger(createTorret());
            //Create Passengers
            for(int i=0; i<numberOfPassengers;i++)
                createEnemy(5f,EnemyContext.OtherCar,EnemyObjective.MeleeAttack,enemyCar);

            return enemyCar;
        }

        public GameObject createTorret()
        {
            GameObject torret = Instantiate(prefabTorret);

            //Set values
            torret.transform.position = transform.position;
            return torret;
        }
    }
}
