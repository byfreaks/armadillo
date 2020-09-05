using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public sealed class  DriveInZone : Drive
    {   
        #region Properties
        #endregion
        
        #region Methods
        public DriveInZone(EnemyController ec, CarController cc) : base(ec, cc){}
        #endregion

        #region Behaviour flow
        public override void init()
        {  
            if(!cc.HasDriver) cc.linkAsDriver(ec.gameObject); //[REVIEW]: Is it the right place??
            currentPoint = anchorPoint + new Vector2(Random.Range(-3f,3f), Random.Range(-5f,5f));
            cc.InBoardingPosition = false;
            
            Debug.Log("Start: DriveInZone"); //[DEBUG]
            ec.sr.color = new Color32(159,33,184,255); //[DEBUG] Sprite Color: Purple
        }
        public override void update()
        { 
            distanceToPoint = currentPoint - (Vector2) cc.transform.position;
            cc.moveTo(distanceToPoint.normalized, 6.5f); //[HARDCODE]
            checkBehaviourEnd();
        }
        public override void final()
        {
            Debug.Log("End: DriveInZone"); //[DEBUG]
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
            return "DriveInZone";
        }
        #endregion
    } 
}
