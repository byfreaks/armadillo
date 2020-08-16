using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public sealed class  Dead : EnemyBehaviour
    {
        #region Properties
        #endregion

        #region Methods
        public Dead(EnemyController ec, CarController cc = null) : base(ec, cc){}
        #endregion Methods
        
        #region Behaviour flow
        public override void init()
        {  
            if(cc != null) cc.unlinkDriver();
            ec.corpse = ec.gameObject.AddComponent<CorpseController>();
            ec.bc.isTrigger = true;
            ec.BlockUpdate = true;
            
            Debug.Log("Start: Dead"); //[DEBUG]
            ec.sr.color = new Color32(84,79,79,255); //[DEBUG] Sprite Color: Gray
        }
        public override void update(){}
        public override void final()
        {
            Debug.Log("End: Dead"); //[DEBUG]
        }
        #endregion
        
        #region Helpers
        public override void checkBehaviourEnd(){}
        public override string getBehaviourName(){
            return "Dead";
        }
        #endregion
    } 
}
