using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public sealed class MountTorret : EnemyBehaviour
    {
        #region Properties
        private TurretController tc;
        private GameObject target;
        #endregion
        
        #region Methods
        public MountTorret(EnemyController ec, CarController cc, GameObject target) : base(ec)
        {
            this.target = target;
            tc = cc.NextAvailableWeaponController;
        }
        #endregion

        #region Behaviour flow
        public override void init()
        {  
            tc.mount(ec.gameObject);
            
            Debug.Log("Start: MountTorret"); //[DEBUG]
            ec.sr.color = new Color32(3,252,235,90); //[DEBUG] Sprite Color: Cyan
        }
        public override void update()
        {
            tc.pointTo(target.transform.position + new Vector3(Random.Range(-3f,3f),Random.Range(-3f,3f),0)); //[HARDCODE] Random aim miss 
            tc.shoot();
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
