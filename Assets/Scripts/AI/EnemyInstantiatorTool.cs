﻿using System.Collections;
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
        public float enemySpeed = 5f;
        public EnemyContext context;
        public EnemyType enemyType;

        [Header("Turret Properties")]
        public float firingRate;
        public int projectileDamage;
        public float projectileSpeed;

        [Header("Enemy Car Properties")]
        public int numberOfPassengers;
        public int numberOfTurrets;

        #region Entities Instantiators
        //Methods without parameters take values from editor
        public GameObject createEnemy()
        {
            GameObject enemy = Instantiate(prefabEnemy);
            EnemyController ec = enemy.GetComponent<EnemyController>();
            ec.constructor(transform.position,enemySpeed,context,enemyType);

            return enemy;
        }
        public GameObject createTurret()
        {
            GameObject turret = Instantiate(prefabTorret);
            TurretController tc = turret.GetComponent<TurretController>();
            tc.constructor(transform.position,firingRate,projectileDamage,projectileSpeed);
            
            return turret;
        }
        public GameObject createCar()
        {
            //Create Car
            GameObject enemyCar = Instantiate(prefabCar);
            CarController cc = enemyCar.GetComponent<CarController>();
            cc.constructor(transform.position,numberOfPassengers,numberOfTurrets);

            //Create Driver
            createEnemy(transform.position,enemySpeed,EnemyContext.OtherCar,EnemyType.Driver, enemyCar);

            //Create Torrets
            for(int i=0; i<numberOfTurrets;i++) cc.linkAsWeapon(createTurret());

            //Create Passengers
            for(int i=0; i<numberOfPassengers;i++)
                createEnemy(transform.position,enemySpeed,EnemyContext.OtherCar,EnemyType.Fighter,enemyCar);

            return enemyCar;
        }
        public void CreateFromEncounter(Encounter encounter)
        {
            foreach (Vehicle vehicle in encounter.Vehicles)
            {
                //Create Car
                GameObject enemyCar = Instantiate(vehicle.EnemyVehicle);
                CarController carController = enemyCar.GetComponent<CarController>();
                carController.constructor(transform.position, vehicle.Passangers.Count, vehicle.NumberOfTurrets);

                //Create Driver
                GameObject driver = Instantiate(prefabEnemy);
                EnemyController enemyControllerDriver = driver.GetComponent<EnemyController>();
                enemyControllerDriver.constructor(transform.position, enemySpeed, EnemyContext.OtherCar, EnemyType.Driver, enemyCar);

                //Create Torrets
                for(int i = 0; i < vehicle.NumberOfTurrets; i++){
                    carController.linkAsWeapon(createTurret());
                }

                //Create Passengers
                foreach (GameObject passenger in vehicle.Passangers)
                {
                    GameObject enemy = Instantiate(passenger);
                    EnemyController enemyController = enemy.GetComponent<EnemyController>();
                    enemyController.constructor(transform.position, enemySpeed, EnemyContext.OtherCar, EnemyType.Fighter, enemyCar);
                }
            }
        }
        //Methods with parameters are used to pass values from the code
        public GameObject createEnemy(Vector3 position, float enemySpeed, EnemyContext context, EnemyType enemyType, GameObject vehicle = null)
        {
            GameObject enemy = Instantiate(prefabEnemy);
            EnemyController ec = enemy.GetComponent<EnemyController>();
            ec.constructor(position,enemySpeed,context,enemyType,vehicle);

            return enemy;
        }
        public GameObject createTurret(Vector3 position, float firingRate, int projectileDamage, float projectileSpeed)
        {
            GameObject turret = Instantiate(prefabTorret);
            TurretController tc = turret.GetComponent<TurretController>();
            tc.constructor(position,firingRate,projectileDamage,projectileSpeed);
            
            return turret;
        }
        #endregion  
    }
}
