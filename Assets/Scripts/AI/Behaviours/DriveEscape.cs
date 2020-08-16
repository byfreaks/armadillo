using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public sealed class  DriveEscape : Drive
    {   
        #region Properties
        #endregion
        
        #region Methods
        public DriveEscape(EnemyController ec, CarController cc) : base(ec, cc){}
        #endregion

        #region Behaviour flow
        public override void init()
        {  
            currentPoint = Vector2.zero;
            
            Debug.Log("Start: DriveEscape"); //[DEBUG]
            ec.sr.color = new Color32(6, 19, 161,255); //[DEBUG] Sprite Color: Blue
        }
        public override void update()
        { 
            cc.moveTo(Vector2.right, 15f); //[HARDCODE]
            checkBehaviourEnd();
        }
        public override void final()
        {
            Debug.Log("End: DriveEscape"); //[DEBUG]
        }
        #endregion
        
        #region Behaviour Helpers
        public override void checkBehaviourEnd(){}
        public override string getBehaviourName()
        {
            return "DriveEscape";
        }
        #endregion
    } 
}
