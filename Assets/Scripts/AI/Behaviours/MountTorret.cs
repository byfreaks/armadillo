using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public sealed class MountTorret : EnemyBehaviour
    {
        #region Properties
        #endregion
        
        #region Methods
        public MountTorret(EnemyController ec) : base(ec) {}
        #endregion

        #region Behaviour flow
        public override void init()
        {  
            Debug.Log("Start: MountTorret"); //[DEBUG]
            ec.sr.color = new Color32(230,83,83,90); //[DEBUG]
        }
        public override void update()
        {
            //[TODO] MountTorret 
            checkBehaviourEnd();
        }
        public override void final()
        {
            Debug.Log("End: MountTorret"); //[DEBUG]
        }
        #endregion

        #region Helpers
        public override void checkBehaviourEnd(){}
        public override string getBehaviourName()
        {
            return "MountTorret";
        }
        #endregion
    } 
}
