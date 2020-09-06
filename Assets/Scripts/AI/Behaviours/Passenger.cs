using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public sealed class Passenger : EnemyBehaviour
    {
        #region Properties
        private GameObject playerVehicle;
        #endregion

        #region Methods
        public Passenger(EnemyController ec, CarController cc) : base(ec, cc){}
        #endregion

        #region Behaviour Flow
        public override void init()
        {  
            cc.linkAsPassenger(ec.gameObject);

            Debug.Log("Start: Passenger");  //[DEBUG]
            ec.sr.color = new Color32(1,89,13,250); //[DEBUG] Sprite Color: Green
        }
        public override void update()
        {   
            if(!cc.HasDriver) ec.EnemyType = EnemyType.Driver; //[MODIFIED ENEMY TYPE] Figther -> Driver
            else if(cc.NextAvailableWeaponController != null) ec.EnemyType = EnemyType.Shooter; //[MODIFIED ENEMY TYPE] Figther -> Shooter
            checkBehaviourEnd();    
        }
        public override void final()
        {
            cc.unlinkPassenger(ec.gameObject);
            //Final steps just to board the player's vehicle
            if(ec.EnemyType == EnemyType.Fighter) 
            {
                ec.bc.isTrigger = true; //[HARDCODE]
                ec.rb.velocity = new Vector2(0f,0f); //[HARDCODE]
                ec.rb.AddForce(new Vector2(0f,700f)); //[HARDCODE]
            }
            Debug.Log("End: Passenger"); //[DEBUG]
        }
        #endregion
        
        #region Behaviour Helpers
        public override void checkBehaviourEnd()
        {
            //[AI TRANSITION]: Idle
            if(cc.InBoardingPosition)
                ec.StartCoroutine(
                    ec.BehaviourTransition(
                        nextBehaviour: new Idle(ec),
                        secondsDuring: 1f
                    )
                );
        }
        public override string getBehaviourName()
        {
            return "Passenger";
        }
        #endregion

        #region AI Methods
        #endregion
    } 
}
