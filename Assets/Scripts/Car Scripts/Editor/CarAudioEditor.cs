using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CarAudio))]
[System.Serializable]
public class CarAudioEditor : Editor
{
    private CarAudio carAudio;
    private SerializedObject SO;

    //Engine Sounds
    private SerializedProperty lowAccelClip;
    private SerializedProperty lowDecelClip;
    private SerializedProperty highAccelClip;
    private SerializedProperty highDecelClip;
    private SerializedProperty engineBangClip;

    //Tire Skid Sounds
    private SerializedProperty tireSkidClip;

    //Settings\
    private SerializedProperty volume;
    private SerializedProperty pitchMultiplier;
    private SerializedProperty lowPitchMin;
    private SerializedProperty lowPitchMax;
    private SerializedProperty highPitchMultiplier;
    private SerializedProperty maxRolloffDistance;
    private SerializedProperty useDoppler;
    private SerializedProperty dopplerLevel;

    private void OnEnable()
    {
        carAudio = (CarAudio)target;
        SO = new SerializedObject(target);

        //Engine Sounds
        lowAccelClip = SO.FindProperty("lowAccelClip");
        lowDecelClip = SO.FindProperty("lowDecelClip");
        highAccelClip = SO.FindProperty("highAccelClip");
        highDecelClip = SO.FindProperty("highDecelClip");
        engineBangClip = SO.FindProperty("engineBangClip");

        //Tire Skid Sounds
        tireSkidClip = SO.FindProperty("tireSkidClip");

        //Settings
        volume = SO.FindProperty("volume");
        pitchMultiplier = SO.FindProperty("pitchMultiplier");
        lowPitchMin = SO.FindProperty("lowPitchMin");
        lowPitchMax = SO.FindProperty("lowPitchMax");
        highPitchMultiplier = SO.FindProperty("highPitchMultiplier");
        maxRolloffDistance = SO.FindProperty("maxRolloffDistance");
        useDoppler = SO.FindProperty("useDoppler");
        dopplerLevel = SO.FindProperty("dopplerLevel");
    }

    public override void OnInspectorGUI()
    {
        SO.Update();

        GUILayout.Space(25);
        GUILayout.Label("Engine sounds", EditorStyles.boldLabel);
        GUILayout.Space(10);

        EditorGUILayout.PropertyField(lowAccelClip, new GUIContent("Low Acceleration Clip: ", "The engine at low revs, with throttle open (i.e. begining acceleration at very low speed)"));
        EditorGUILayout.PropertyField(lowDecelClip, new GUIContent("Low Deceleration Clip: ", "The engine at high revs, with throttle open (i.e. accelerating, but almost at max speed)"));
        EditorGUILayout.PropertyField(highAccelClip, new GUIContent("High Acceleration Clip: ", "The engine at low revs, with throttle at minimum (i.e. idling or engine-braking at very low speed)"));
        EditorGUILayout.PropertyField(highDecelClip, new GUIContent("High Deceleration Clip: ", "The engine at high revs, with throttle at minimum (i.e. engine-braking at very high speed)"));
        EditorGUILayout.PropertyField(engineBangClip, new GUIContent("Engine Bang Clip: ", "The engine when it bang and pops while shifting"));
        EditorGUILayout.Space(5);
        EditorGUILayout.HelpBox(new GUIContent("For proper crossfading, the clips pitches should all match, with an octave offset between low and high."));


        GUILayout.Space(25);
        GUILayout.Label("Tire Skid Sounds", EditorStyles.boldLabel);
        GUILayout.Space(5);

        EditorGUILayout.PropertyField(tireSkidClip, new GUIContent("Tire Skid Clip: "));



        GUILayout.Space(25);
        GUILayout.Label("Settings", EditorStyles.boldLabel);
        GUILayout.Space(5);
        volume.floatValue = EditorGUILayout.Slider("Sound volume: ",volume.floatValue , 0 , 1);
        EditorGUILayout.PropertyField(pitchMultiplier, new GUIContent("Pitch Multiplier: "));
        EditorGUILayout.PropertyField(lowPitchMin, new GUIContent("Low Pitch Minimum: "));
        EditorGUILayout.PropertyField(lowPitchMax, new GUIContent("Low Pitch Maximum: "));
        EditorGUILayout.PropertyField(highPitchMultiplier, new GUIContent("High Pitch Multiplier: "));
        EditorGUILayout.PropertyField(maxRolloffDistance, new GUIContent("Maximum Rolloff Distance: ", "When the Audio Listener is outside this distance, the sound is cut off"));
        useDoppler.boolValue = EditorGUILayout.BeginToggleGroup("Use Doppler?", useDoppler.boolValue);
        EditorGUILayout.PropertyField(dopplerLevel, new GUIContent("Doppler Level: "));
        EditorGUILayout.EndToggleGroup();

        SO.ApplyModifiedProperties();
    }
}
