using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AI{
    
    [CustomEditor(typeof(EnemyInstantiatorTool))]
    [CanEditMultipleObjects]
    public class EnemyInstatiatorToolEditor: Editor
    {

        SerializedProperty EncounterData;

        private void OnEnable() {
            EncounterData = serializedObject.FindProperty("scriptedEncounter");
        }

        public override void OnInspectorGUI()
        {

            serializedObject.Update();
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

            EditorGUILayout.Space(20);
            EditorGUILayout.LabelField("Spawn Encounter");
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(EncounterData, new GUIContent(""));
            if(GUILayout.Button("Create From Encounter") && EncounterData != null)
            {
                var enc = EncounterData.objectReferenceValue as System.Object as Encounter;
                tool.CreateFromEncounter(enc);
            }
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }

}
