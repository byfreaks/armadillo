using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public class Passenger : EnemyBehaviour
    {
        //Entities controllers
        private EnemyController ec;
        private CarController cc;
        
        public Passenger(EnemyController ec, CarController cc)
        {
            this.ec = ec;
            this.cc = cc;
        }

        #region Behaviour Flow
        public override void init()
        {  
            cc.linkAsPassenger(ec.gameObject);

            //Debug
            Debug.Log("Start: Passenger");
            ec.sr.color = new Color32(80,97,31,90);
            //
        }
        public override void update()
        {   
            //Check conditions to keep at this behaviour
            if(checkBehaviourConditions())
            {
                
            }
            else ec.CurrentBehaviour = new Idle(ec);
        }
        public override void final()
        {
            cc.unlinkPassenger(ec.gameObject);
            //Debug
            Debug.Log("End: Passenger");
            //
        }
        #endregion
        
        #region Behaviour Helpers
        public override bool checkBehaviourConditions()
        {
            return true;
        }

        public override string getBehaviourName(){
            return "Passenger";
        }
        #endregion

        #region AI Methods
        #endregion
    } 
}
