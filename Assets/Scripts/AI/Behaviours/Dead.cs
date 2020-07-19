using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public class Dead : EnemyBehaviour
    {

        //Entity Data with this Behaviour
        private EnemyController ec;
        

        public Dead(EnemyController ec)
        {
            this.ec = ec;
        }

        /* Behaviour flow */
        public override void init()
        {  
            
            ec.corpse = ec.gameObject.AddComponent<CorpseController>();
            ec.bc.isTrigger = true;
            //Debug
            Debug.Log("Start: Dead");
            ec.sr.color = new Color32(84,79,79,255);
            //
        }

        public override void update()
        { 
            //Do Nothing...
        }
        public override void final()
        {
            //Debug
            Debug.Log("End: Dead");
            //
        }
    
        /* Helpers */
        public override bool checkBehaviourConditions()
        {
            return true;
        }

        public override string getBehaviourName(){
            return "Dead";
        }

    } 
}
