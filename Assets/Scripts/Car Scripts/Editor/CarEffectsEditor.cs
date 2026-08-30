using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CarEffects))]
[System.Serializable]
public class CarEffectsEditor : Editor
{
    private CarEffects carEffects;
    private SerializedObject SO;

    //Smoke Effects
    private SerializedProperty rearLeftSmoke;
    private SerializedProperty rearRightSmoke;

    //Tire Skid Trails
    private SerializedProperty rearLeftTireSkid;
    private SerializedProperty rearRightTireSkid;

    //Yellow Exausht Particles
    private SerializedProperty useYellowParticles;
    private SerializedProperty leftYellowExausth;
    private SerializedProperty rightYellowExausth;

    //Nitrous Particles
    private SerializedProperty useNitrousParticles;
    private SerializedProperty leftNitrousExausth;
    private SerializedProperty rightNitrousExausth;

    private void OnEnable()
    {
        carEffects = (CarEffects)target;
        SO = new SerializedObject(target);

        //Smoke Effects
        rearLeftSmoke = SO.FindProperty("rearLeftSmoke");
        rearRightSmoke = SO.FindProperty("rearRightSmoke");

        //Tire Skid Trails
        rearLeftTireSkid = SO.FindProperty("rearLeftTireSkid");
        rearRightTireSkid = SO.FindProperty("rearRightTireSkid");

        //Yellow Exausth Particles
        useYellowParticles = SO.FindProperty("useYellowParticles");
        leftYellowExausth = SO.FindProperty("leftYellowExausth");
        rightYellowExausth = SO.FindProperty("rightYellowExausth");

        //Nitrous Particles
        useNitrousParticles = SO.FindProperty("useNitrousParticles");
        leftNitrousExausth = SO.FindProperty("leftNitrousExausth");
        rightNitrousExausth = SO.FindProperty("rightNitrousExausth");
    }

    public override void OnInspectorGUI()
    {
        SO.Update();

        GUILayout.Space(25);
        GUILayout.Label("Smoke Particles", EditorStyles.boldLabel);
        GUILayout.Space(5);
        EditorGUILayout.PropertyField(rearLeftSmoke, new GUIContent("Rear Left Smoke: "));
        EditorGUILayout.PropertyField(rearRightSmoke, new GUIContent("Rear Right Smoke: "));

        GUILayout.Space(25);
        GUILayout.Label("Skid Trails", EditorStyles.boldLabel);
        GUILayout.Space(5);
        EditorGUILayout.PropertyField(rearLeftTireSkid, new GUIContent("Rear Left Tire Skid: "));
        EditorGUILayout.PropertyField(rearRightTireSkid, new GUIContent("Rear Right Tire Skid: "));

        GUILayout.Space(25);
        useYellowParticles.boolValue = EditorGUILayout.BeginToggleGroup("Use Yellow Exausth Particles?", useYellowParticles.boolValue);
        GUILayout.Space(5);
        EditorGUILayout.PropertyField(leftYellowExausth, new GUIContent("Left Yellow Paricles: "));
        EditorGUILayout.PropertyField(rightYellowExausth, new GUIContent("Right Yellow Paricles: "));
        EditorGUILayout.EndToggleGroup();

        GUILayout.Space(25);
        useNitrousParticles.boolValue = EditorGUILayout.BeginToggleGroup("Use Nitrous Particles?", useNitrousParticles.boolValue);
        GUILayout.Space(5);
        EditorGUILayout.PropertyField(leftNitrousExausth, new GUIContent("Left Nitrous Paricles: "));
        EditorGUILayout.PropertyField(rightNitrousExausth, new GUIContent("Right Nitrous Paricles: "));
        EditorGUILayout.EndToggleGroup();

        SO.ApplyModifiedProperties();
    }
}
