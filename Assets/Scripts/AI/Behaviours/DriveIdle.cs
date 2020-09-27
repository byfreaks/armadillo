using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public sealed class  DriveIdle : EnemyBehaviour
    {
        #region Properties
        private float zoneProb;
        private float boardProb;
        private Vector2 anchorPoint;
        private Vector2 boardPoint;
        #endregion
        
        #region Methods
        public DriveIdle(EnemyController ec, CarController cc) : base(ec, cc)
        {
            this.zoneProb = 0f;
            this.boardProb = 0f;
            //Get screen zone (left or right)
            if(Random.Range(0,2) == 0)
            {
                this.anchorPoint = new Vector2(20,-14);
                this.boardPoint = new Vector2(0,-15);
            }
            else
            {
                this.anchorPoint = new Vector2(-20,-14);
                this.boardPoint = new Vector2(0,-15); 
            }
        }
        #endregion

        #region Behaviour flow
        public override void init()
        {  
            cc.moveTo(Vector2.zero,0f);
            
            Debug.Log("Start: DriveIdle"); //[DEBUG]
            ec.sr.color = new Color32(145,156,5,255); //[DEBUG] Sprite Color: Yellow
        }
        public override void update()
        { 
            zoneProb += 0.0003f; //[HARDCODE]
            boardProb += 0.0007f; //[HARDCODE]
            checkBehaviourEnd();
        }
        public override void final()
        {
            Debug.Log("End: DriveIdle"); //[DEBUG]
        }
        #endregion
        
        #region Behaviour Helpers
        public override void checkBehaviourEnd()
        {
            //[AI TRANSITION]: DriveEscape
            if(cc.NumberOfCurrentPassengers == 0 && cc.NumberOfActiveWeapons == 0)
                ec.StartCoroutine(
                    ec.BehaviourTransition(
                        nextBehaviour: new DriveEscape(ec,cc)
                    )
                );
            else
            {
                float nextAction = Random.Range(0f,1f);
                //[AI TRANSITION]: DriveToPoint == InZone
                if(nextAction < zoneProb)
                    ec.StartCoroutine(
                        ec.BehaviourTransition(
                            nextBehaviour: new DriveToPoint(ec,cc,anchorPoint,false),
                            secondsBefore: 5f //[HARDCODE]
                        )
                    );
                //[AI TRANSITION]: DriveToPoint == BoardZone
                else if(cc.NumberOfCurrentPassengers > 0 && nextAction < boardProb)
                    ec.StartCoroutine(
                        ec.BehaviourTransition(
                            nextBehaviour: new DriveToPoint(ec,cc,boardPoint,true),
                            secondsBefore: 5f //[HARDCODE]
                        )
                    );
            }
        }
        public override string getBehaviourName()
        {
            return "DriveIdle";
        }
        #endregion
    } 
}
