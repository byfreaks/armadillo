using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public sealed class DriveInit : EnemyBehaviour
    {
        #region Properties
        #endregion

        #region Methods
        public DriveInit(EnemyController ec, CarController cc) : base(ec,cc) {}
        #endregion

        #region Behaviour Flow
        public override void init()
        {
            cc.linkAsDriver(ec.gameObject);
            Debug.Log("Start: DriveInit"); //[DEBUG]
        }
        public override void update()
        {
            checkBehaviourEnd();
        }
        public override void final()
        {
            Debug.Log("Final: DriveInit"); //[DEBUG]
        }
        #endregion

        #region Behaviour Helpers
        public override bool checkInitConditions()
        {
            return (!cc.HasDriver);
        }
        public override void checkBehaviourEnd()
        {
            //[AI TRANSITION]: DriveIdle
            ec.StartCoroutine(
                ec.BehaviourTransition(
                    nextBehaviour: new DriveIdle(ec,cc)
                )
            );
        }
        public override string getBehaviourName(){
            return "DriveInit";
        }
        #endregion
    } 
}
