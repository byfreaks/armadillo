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

        [Header("Enemy Properties")]
        public float enemySpeed;
        public EnemyContext context;
        public EnemyType enemyType;

        [Header("Enemy Car Properties")]
        public int numberOfPassengers;
        public int numberOfTorrets;

        public GameObject createEnemy()
        {
            GameObject enemy = Instantiate(prefabEnemy);
            EnemyController ec = enemy.GetComponent<EnemyController>();
            ec.enemyConstructor(transform.position,enemySpeed,context,enemyType);

            return enemy;
        }

        public GameObject createEnemy(float enemySpeed, EnemyContext context, EnemyType enemyType, GameObject vehicle = null)
        {
            GameObject enemy = Instantiate(prefabEnemy);
            EnemyController ec = enemy.GetComponent<EnemyController>();
            ec.enemyConstructor(transform.position,enemySpeed,context,enemyType,vehicle);

            return enemy;
        }

        public GameObject createCar()
        {
            GameObject enemyCar = Instantiate(prefabCar);
            CarController cc = enemyCar.GetComponent<CarController>();

            //Set values
            enemyCar.transform.position = transform.position;
            //Create Driver
            createEnemy(5f,EnemyContext.OtherCar,EnemyType.Driver, enemyCar);
            //Create Torrets
            for(int i=0; i<numberOfTorrets;i++)
                cc.linkAsPassenger(createTorret());
            //Create Passengers
            for(int i=0; i<numberOfPassengers;i++)
                createEnemy(5f,EnemyContext.OtherCar,EnemyType.Fighter,enemyCar);

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
