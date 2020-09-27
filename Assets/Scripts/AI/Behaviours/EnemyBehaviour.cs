using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public abstract class EnemyBehaviour
    {
        #region Properties
        protected EnemyController ec;
        public CarController cc {get; set;} //[REVIEW]: The accesor must be protected (Temporal Solution: unlink when the enemy dead and it is a Driver)
        #endregion
        
        #region Methods
        public EnemyBehaviour(EnemyController ec, CarController cc = null)
        {
            this.ec = ec;
            this.cc = cc;
        }
        #endregion
        
        #region Behaviour flow
        public abstract void init();
        public abstract void update();
        public abstract void final();
        #endregion
        
        #region Behaviour Helpers
        public virtual bool checkInitConditions() { return true; }
        public abstract void checkBehaviourEnd();
        public abstract string getBehaviourName();
        #endregion
    }
} 