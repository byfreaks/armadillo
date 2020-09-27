using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public sealed class  DriveToPoint : EnemyBehaviour
    {   
        #region Properties
        private Vector2 targetPoint;
        private Vector2 distanceToPoint;
        private bool boardingMode;
        #endregion
        
        #region Methods
        public DriveToPoint(EnemyController ec, CarController cc, Vector2 targetPoint, bool boardingMode) : base(ec, cc) 
        {
            this.targetPoint = targetPoint;
            this.boardingMode = boardingMode;
        }
        #endregion

        #region Behaviour flow
        public override void init()
        {  
            cc.InBoardingPosition = false;
            
            Debug.Log("Start: DriveToPoint"); //[DEBUG]
            ec.sr.color = new Color32(159,33,184,255); //[DEBUG] Sprite Color: Purple
        }
        public override void update()
        { 
            distanceToPoint = targetPoint - (Vector2) cc.transform.position;
            cc.moveTo(distanceToPoint.normalized, 6.5f); //[HARDCODE]
            checkBehaviourEnd();
        }
        public override void final()
        {
            if(boardingMode)
            {
                cc.InBoardingPosition = true;
                cc.moveTo(Vector2.zero,0f);
            }
            Debug.Log("End: DriveToPoint"); //[DEBUG]
        }
        #endregion
        
        #region Behaviour Helpers
        public override void checkBehaviourEnd()
        {
            //[AI TRANSITION]: DriveIdle
            if(distanceToPoint.magnitude < 3f) //[HARDCODE]
                ec.StartCoroutine(
                    ec.BehaviourTransition(
                        nextBehaviour: new DriveIdle(ec,cc),
                        secondsAfter: 5f //[HARDCODE]
                    )
                );
        }
        public override string getBehaviourName(){
            return "DriveToPoint";
        }
        #endregion
    } 
}
