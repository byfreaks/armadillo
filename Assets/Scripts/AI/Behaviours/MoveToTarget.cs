using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public class MoveToTarget : EnemyBehaviour
    {

        //Attributes
        private int moveDirection;

        //Target
        private GameObject target;
        private Vector2 targetPosition;

        //Entity Data with this Behaviour
        private EnemyController ec;
        

        public MoveToTarget(EnemyController ec, GameObject target)
        {
            this.ec = ec;
            this.target = target;
        }

        /* Behaviour flow */
        public override void init()
        {  
            //Debug
            Debug.Log("Start: MoveToTarget");
            ec.sr.color = new Color32(239,105,9,200);
            //
        }

        public override void update()
        {
            //Update target position
            targetPosition = target.GetComponent<Rigidbody2D>().position;

            //Check conditions to keep at this behaviour
            if(checkBehaviourConditions())
            {
                //Update
                moveDirection = (targetPosition.x - ec.rb.position.x > 0) ? 1 : -1;               
                ec.rb.velocity = new Vector2(ec.MoveSpeed * moveDirection, ec.rb.velocity.y);
            }
            else
                if(target.name == "Player") ec.CurrentBehaviour = new MeleeAttack(ec, target);
        }
        public override void final()
        {
            ec.rb.velocity = Vector2.zero;
            //Debug
            Debug.Log("End: MoveToTarget");
            //
        }
    
        /* Helpers */
        public override bool checkBehaviourConditions()
        {
            //Far from his target
            if(Mathf.Abs(targetPosition.x - ec.rb.position.x) > ec.ContactDistance) return true;
            //Close to his target
            else return false;
        }

        public override string getBehaviourName(){
            return "MoveToTarget";
        }

    } 
}
