using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public sealed class  DriveToBoard : Drive
    {   
        #region Properties
        #endregion
        
        #region Methods
        public DriveToBoard(EnemyController ec, CarController cc) : base(ec, cc){}
        #endregion

        #region Behaviour flow
        public override void init()
        {  
            currentPoint = boardPoint;
            
            Debug.Log("Start: DriveToBoard"); //[DEBUG]
            ec.sr.color = new Color32(245,34,134,255); //[DEBUG] Sprite Color: Pink
        }
        public override void update()
        { 
            distanceToPoint = currentPoint - (Vector2) cc.transform.position;
            cc.moveTo(distanceToPoint.normalized, 6.5f); //[HARDCODE]
            checkBehaviourEnd();            
        }
        public override void final()
        {
            cc.moveTo(Vector2.zero,0f);
            cc.InBoardingPosition = true;
            Debug.Log("End: DriveToBoard"); //[DEBUG]
        }
        #endregion
        
        #region Behaviour Helpers
        public override void checkBehaviourEnd()
        {
            //[AI TRANSITION]: DriveInZone
            if(distanceToPoint.magnitude < 1f) //[HARDCODE]
                ec.StartCoroutine(
                    ec.BehaviourTransition(
                        nextBehaviour: new DriveInZone(ec,cc),
                        secondsDuring: 5f //[HARDCODE]
                    )
                );
        }
        public override string getBehaviourName()
        {
            return "DriveToBoard";
        }
        #endregion
    } 
}
