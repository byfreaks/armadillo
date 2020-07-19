using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [System.Serializable]
    public abstract class EnemyBehaviour
    {

        /* Behaviour flow */
        public abstract void init();
        public abstract void update();
        public abstract void final();
        
        /* Helpers */
        public abstract bool checkBehaviourConditions();
        public abstract string getBehaviourName();

    }
} 