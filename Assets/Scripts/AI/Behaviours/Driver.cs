using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public class Driver : EnemyBehaviour
    {
        //Entities controllers
        private EnemyController ec;
        private CarController cc;
        
        //Driver AI values
        private Vector2 anchorPoint;
        private Vector2 boardPoint;
        private Vector2 currentPoint;
        private Vector2 differenceToTarget;
        private DriverActions currentAction;
        private float zoneProb;
        private float boardProb;

        public Driver(EnemyController ec, CarController cc)
        {
            this.ec = ec;
            this.cc = cc;

            //Get screen zone (left or right) (TODO: Replace these instructions)
            if(Random.Range(0,2) == 0)
            {
                this.anchorPoint = new Vector2(20,-14);
                this.boardPoint = new Vector2(2,-14); 
            }
            else
            {
                this.anchorPoint = new Vector2(-20,-14);
                this.boardPoint = new Vector2(-2,-14); 
            }
        }

        #region Behaviour Flow
        public override void init()
        {  
            cc.linkAsDriver(ec.gameObject);
            initAction(DriverActions.MoveInZone);

            //Debug
            Debug.Log("Start: Driver");
            ec.sr.color = new Color32(239,225,30,90);
            //
        }
        public override void update()
        {   
            //Check conditions to keep at this behaviour
            if(checkBehaviourConditions())
            {
                updateAction();
                checkActionEnd();
            }
        }
        public override void final()
        {
            cc.unlinkDriver();
            //Debug
            Debug.Log("End: Driver");
            //
        }
        #endregion
        
        #region Behaviour Helpers
        public override bool checkBehaviourConditions()
        {
            return true;
        }

        public override string getBehaviourName(){
            return "Driver";
        }
        #endregion

        #region AI Methods
        void initAction(DriverActions action)
        {
            if(action == DriverActions.Idle)
            {
                cc.moveTo(Vector2.zero,0f);
                zoneProb = 0f; //[HARDCODE]
                boardProb = 0f; //[HARDCODE]
                
            }
            else if(action == DriverActions.MoveInZone)
                currentPoint = anchorPoint + new Vector2(Random.Range(-3f,3f), Random.Range(-5f,5f));
            else if(action == DriverActions.MoveToBoardZone)
                currentPoint = boardPoint;
            else if(action == DriverActions.Escape)
                currentPoint = Vector2.zero;

            currentAction = action;
        }
        void updateAction()
        {
            if(currentAction == DriverActions.MoveInZone || currentAction == DriverActions.MoveToBoardZone)
            {
                differenceToTarget = currentPoint - (Vector2) cc.transform.position;
                cc.moveTo(differenceToTarget.normalized, 6.5f); //[HARDCODE]
            }
            else if(currentAction == DriverActions.Escape)
            {
                cc.moveTo(Vector2.right, 15f); //[HARDCODE]
            }
            else if(currentAction == DriverActions.Idle)
            {
                zoneProb += 0.0003f; //[HARDCODE]
                boardProb += 0.0007f; //[HARDCODE]
            }
        }
        void checkActionEnd()
        {
            if(currentAction == DriverActions.MoveInZone || currentAction == DriverActions.MoveToBoardZone)
            {
                if(differenceToTarget.magnitude < 3f) //[HARDCODE]
                    initAction(DriverActions.Idle);
                
            }
            else if(currentAction == DriverActions.Idle)
            {
                if(cc.getNumberOfPassangers() == 0 && cc.getNumberOfActiveWeapons() == 0)
                    initAction(DriverActions.Escape);
                else
                {
                    float nextAction = Random.Range(0f,1f);
                    if(nextAction < zoneProb)
                        initAction(DriverActions.MoveInZone);
                    else if(cc.getNumberOfPassangers() > 0 && nextAction < boardProb)
                        initAction(DriverActions.MoveToBoardZone);
                }
            }
        }
        #endregion
    } 
}
