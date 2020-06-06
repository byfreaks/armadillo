using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Health))]
[CanEditMultipleObjects]
public class HealthEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //Referencia al componente Health para poder acceder a sus propiedades y métodos.
        Health health = (Health)target;

        //Campos de propiedades del componente Health.
        EditorGUILayout.Space();
        health.HealthPoints = EditorGUILayout.IntField("Health points", health.HealthPoints);
        health.MaxHealthPoints = EditorGUILayout.IntField("Max health points", health.MaxHealthPoints);
        health.MinHealthPoints = EditorGUILayout.IntField("Min health points", health.MinHealthPoints);
        health.IsAlive = EditorGUILayout.Toggle("Is alive", health.IsAlive);
        EditorGUILayout.Space(20);
        
        //Botones de acción para ejecutar los distintos métodos descritos en el componente Health.
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
            health.killEntity();
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
