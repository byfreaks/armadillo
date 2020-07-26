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
            //Referencia al componente Health para poder acceder a sus propiedades y métodos.
            EnemyInstantiatorTool tool = (EnemyInstantiatorTool) target;
            
            //Botones de acción para ejecutar los distintos métodos descritos en el componente Health.
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
