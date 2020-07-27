using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AI{
    
    [CustomEditor(typeof(EnemyController))]
    [CanEditMultipleObjects]
    public class EnemyControllerEditor: Editor
    {
        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();
            EnemyController ec = (EnemyController) target;
            
            //Show Behaviour
            if(ec.CurrentBehaviour != null) EditorGUILayout.LabelField("Current Behaviour", ec.CurrentBehaviour.getBehaviourName());

            serializedObject.ApplyModifiedProperties();

        }
    }

}
