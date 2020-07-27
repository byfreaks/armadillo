using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AI{
    
    [CustomEditor(typeof(EnemyInstantiatorTool))]
    [CanEditMultipleObjects]
    public class EnemyInstatiatorToolEditor: Editor
    {
        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();
            EnemyInstantiatorTool tool = (EnemyInstantiatorTool) target;
            
            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Actions");
            if(GUILayout.Button("Create Enemy"))
            {
                tool.createEnemy();
            }
            if(GUILayout.Button("Create Car"))
            {
                tool.createCar();
            }
            if(GUILayout.Button("Create Torret"))
            {
                tool.createTorret();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }

}
