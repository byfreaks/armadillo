using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponController))]
[CanEditMultipleObjects]
public class WeaponControllerEditor : Editor
{
    public Rect pos { get => GUILayoutUtility.GetLastRect(); }
    bool info_fold = false;

    //Serialized properties
    SerializedProperty type;
    //Melee
    SerializedProperty hitbox;

    //Ranged
    SerializedProperty projectile, firerate;

    private void OnEnable() {
        type = serializedObject.FindProperty("WeaponType");
        //melee
        hitbox = serializedObject.FindProperty("hitbox");
        //ranged
        projectile = serializedObject.FindProperty("projectile");
        firerate = serializedObject.FindProperty("fireRate");
    }

    public override void OnInspectorGUI(){

        serializedObject.Update();

        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("GENERAL CONFIGURATION");
        DrawDefaultInspector();
        WeaponController weapon = target as WeaponController;

        //WEAPON TYPE
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("WEAPON TYPE CONFIGURATION");
        EditorGUILayout.PropertyField(type, new GUIContent("Weapon Type"));

        EditorGUILayout.Space(10);

        switch(weapon.WeaponType){
            case WeaponController.weaponType.ranged:
                EditorGUILayout.PropertyField(projectile, new GUIContent("Projectile"));
                EditorGUILayout.PropertyField(firerate, new GUIContent("Rate of Fire (per Second)"));
            break;

            case WeaponController.weaponType.melee:
                EditorGUILayout.PropertyField(hitbox, new GUIContent("Hitbox"));
            break;
        }

        //INFO
        EditorGUILayout.Space(20);

        info_fold = EditorGUI.BeginFoldoutHeaderGroup(new Rect(pos.x, pos.y+20, pos.width, 20),info_fold,"Info");
        if(info_fold){
            EditorGUILayout.Space(15);
            EditorGUILayout.LabelField("Current Command", weapon.currentCommand.ToString().ToUpper() );
            if(weapon.wielderTransform!=null)EditorGUILayout.LabelField("Wielder", weapon.wielderTransform.gameObject.name);
        }
        EditorGUI.EndFoldoutHeaderGroup();

        EditorGUILayout.Space(20);
        serializedObject.ApplyModifiedProperties();

    }
}
