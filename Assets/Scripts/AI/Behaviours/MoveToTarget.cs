using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public sealed class MoveToTarget : EnemyBehaviour
    {
        #region Properties
        private int moveDirection;
        private GameObject target;
        private Vector2 targetPosition;
        #endregion

        #region Methods
        public MoveToTarget(EnemyController ec, GameObject target) : base(ec)
        {
            this.target = target;
            this.targetPosition = target.transform.position;
        }
        #endregion

        #region Behaviour flow
        public override void init()
        {  
            ec.EquipedWeapon.Set(WeaponCommands.hold,Vector2.zero);
            Debug.Log("Start: MoveToTarget"); //[DEBUG]
            ec.sr.color = new Color32(239,105,9,200); //[DEBUG]
        }
        public override void update()
        {
            //Update target position
            this.targetPosition = target.transform.position;

            //Update
            if(ec.OnGround)
            {
                moveDirection = (targetPosition.x - ec.rb.position.x > 0) ? 1 : -1;               
                ec.rb.velocity = new Vector2(ec.MoveSpeed * moveDirection, ec.rb.velocity.y);
            }

            checkBehaviourEnd();
        }
        public override void final()
        {
            ec.rb.velocity = Vector2.zero;
            
            Debug.Log("End: MoveToTarget"); //[DEBUG]
        }
        #endregion

        #region Helpers
        public override void checkBehaviourEnd()
        {
            //[AI TRANSITION]: MeleeAttack
            //Close to his target
            if(Mathf.Abs(targetPosition.x - ec.rb.position.x) <= ec.ContactDistance)
                ec.StartCoroutine(
                    ec.BehaviourTransition(
                        nextBehaviour: new MeleeAttack(ec,target)
                    )
                );
        }
        public override string getBehaviourName()
        {
            return "MoveToTarget";
        }
        #endregion
    } 
}
