using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public class MeleeAttack : EnemyBehaviour
    {

        //Attributes
        private int moveDirection;

        //Target
        private GameObject target;
        private Vector2 targetPosition;

        //Entity Data with this Behaviour
        private EnemyController ec;
        

        public MeleeAttack(EnemyController ec, GameObject target)
        {
            this.ec = ec;
            this.target = target;
        }

         /* Behaviour flow */
        public override void init()
        {  
            //Debug
            Debug.Log("Start: Attack");
            ec.sr.color = new Color32(230,83,83,90);
            //
        }
        public override void update()
        {
            //Update target position
            targetPosition = target.GetComponent<Rigidbody2D>().position;
            
            //Check conditions to keep at this behaviour
            if(checkBehaviourConditions())
            {
                //Do Nothing...
            }
            else
                ec.CurrentBehaviour = new MoveToTarget(ec, target);
        }
        public override void final()
        {
            //Debug
            Debug.Log("End: Attack");
            //
        }
    
        /* Helpers */
        public override bool checkBehaviourConditions()
        {
            //Far from his target
            if(Mathf.Abs(targetPosition.x - ec.rb.position.x) > ec.ContactDistance) return false;
            //Close to his target
            else return true;
        }

        public override string getBehaviourName(){
            return "MeleeAttack";
        }

    } 
}
