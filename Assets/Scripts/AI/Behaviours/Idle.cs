using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public sealed class Idle : EnemyBehaviour
    {
        #region Properties
        #endregion
        
        #region Methods
        public Idle(EnemyController ec) : base(ec) {}
        #endregion

        #region Behaviour flow
        public override void init()
        {  
            ec.bc.isTrigger = false; //[HARDCODE] Passenger jump

            Debug.Log("Start: Idle"); //[DEBUG]
            ec.sr.color = new Color32(255,255,255,255); //[DEBUG] Sprite Color: White
        }
        public override void update(){} 
        public override void final()
        {
            Debug.Log("End: Idle"); //[DEBUG]   
        }
        #endregion
        
        #region Helpers
        public override void checkBehaviourEnd(){}
        public override string getBehaviourName()
        {
            return "Idle";
        }
        #endregion
    } 
}
