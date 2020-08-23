using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public sealed class MeleeAttack : EnemyBehaviour
    {
        #region Properties
        private GameObject target;
        private Vector2 targetPosition;
        #endregion
        
        #region Methods
        public MeleeAttack(EnemyController ec, GameObject target) : base(ec)
        {
            this.target = target;
            this.targetPosition = target.transform.position;
        }
        #endregion

        #region Behaviour flow
        public override void init()
        {             
            Debug.Log("Start: Attack"); //[DEBUG]
            ec.sr.color = new Color32(230,83,83,90); //[DEBUG]
        }
        public override void update()
        {
            targetPosition = target.transform.position;
            ec.EquipedWeapon.Attack((targetPosition - ec.rb.position).normalized);
            checkBehaviourEnd();
        }
        public override void final()
        {
            Debug.Log("End: Attack"); //[DEBUG]
        }
        #endregion

        #region Helpers
        public override void checkBehaviourEnd()
        {
            //[AI TRANSITION]: MoveToTarget(Player)
            if(Mathf.Abs(targetPosition.x - ec.rb.position.x) > ec.ContactDistance)
            {
                ec.StartCoroutine(
                    ec.BehaviourTransition(
                        nextBehaviour: new MoveToTarget(ec,target)
                    )
                );
            }
            //[AI TRANSITION]: MeleeAttack
            else
                ec.StartCoroutine(
                    ec.BehaviourTransition(
                        nextBehaviour: new MeleeAttack(ec,target),
                        secondsDuring: 2f
                    )
                );
        }
        public override string getBehaviourName()
        {
            return "MeleeAttack";
        }
        #endregion
    } 
}
