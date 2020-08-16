using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public abstract class Drive : EnemyBehaviour
    {
        #region Properties
        protected Vector2 anchorPoint;
        protected Vector2 boardPoint;
        protected Vector2 currentPoint;
        protected Vector2 distanceToPoint;
        #endregion

        #region Methods
        public Drive(EnemyController ec, CarController cc) : base(ec,cc)
        {
            //Get screen zone (left or right) ([TODO] Replace these instructions)
            //[REVIEW]: This code is executing each time that the driver behaviour is changed
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
        #endregion

        #region Behaviour Flow
        #endregion
        
        #region Behaviour Helpers
        #endregion
    } 
}
