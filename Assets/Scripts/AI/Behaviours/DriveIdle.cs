using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public sealed class  DriveIdle : Drive
    {
        #region Properties
        private float zoneProb;
        private float boardProb;
        #endregion
        
        #region Methods
        public DriveIdle(EnemyController ec, CarController cc) : base(ec, cc)
        {
            this.zoneProb = 0f;
            this.boardProb = 0f;
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
                //[AI TRANSITION]: DriveInZone
                if(nextAction < zoneProb)
                    ec.StartCoroutine(
                        ec.BehaviourTransition(
                            nextBehaviour: new DriveInZone(ec,cc),
                            secondsBefore: 5f //[HARDCODE]
                        )
                    );
                //[AI TRANSITION]: DriveToBoard
                else if(cc.NumberOfCurrentPassengers > 0 && nextAction < boardProb)
                    ec.StartCoroutine(
                        ec.BehaviourTransition(
                            nextBehaviour: new DriveToBoard(ec,cc),
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
