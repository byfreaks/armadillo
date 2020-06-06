using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Health))]
[CanEditMultipleObjects]
public class HealthEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Health health = (Health)target;

        EditorGUILayout.Space();
        health.HealthPoints = EditorGUILayout.IntField("Health points", health.HealthPoints);
        health.MaxHealthPoints = EditorGUILayout.IntField("Max health points", health.MaxHealthPoints);
        health.MinHealthPoints = EditorGUILayout.IntField("Min health points", health.MinHealthPoints);
        health.IsAlive = EditorGUILayout.Toggle("Is alive", health.IsAlive);
        EditorGUILayout.Space(30);

        EditorGUILayout.LabelField("Actions");
        if(GUILayout.Button("Increment health points (+1)"))
        {
            health.incrementHealthPoints(1);
        }

        if(GUILayout.Button("Decrement health points (-1)"))
        {
            health.decrementHealthPoints(1);
        }

        if(GUILayout.Button("Kill entity"))
        {
            health.HealthPoints = 0;
        }

        if(GUILayout.Button("Set max health Points"))
        {
            health.HealthPoints = health.MaxHealthPoints;
        }

        if(GUILayout.Button("Set min health Points"))
        {
            health.HealthPoints = health.MinHealthPoints;
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}
