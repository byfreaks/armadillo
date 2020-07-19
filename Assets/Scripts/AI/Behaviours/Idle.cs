using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public class Idle : EnemyBehaviour
    {

        //Entity Data with this Behaviour
        private EnemyController ec;
        

        public Idle(EnemyController ec)
        {
            this.ec = ec;
        }

        /* Behaviour flow */
        public override void init()
        {  
            ec.rb.velocity = Vector2.zero;
            //Debug
            Debug.Log("Start: Idle");
            ec.sr.color = new Color32(255,255,255,255);
            //
        }

        public override void update()
        { 
            //Do nothing...
        }
        public override void final()
        {
            //Debug
            Debug.Log("End: Idle");
            //
        }
    
        /* Helpers */
        public override bool checkBehaviourConditions()
        {
            return true;
        }

        public override string getBehaviourName(){
            return "Idle";
        }

    } 
}
