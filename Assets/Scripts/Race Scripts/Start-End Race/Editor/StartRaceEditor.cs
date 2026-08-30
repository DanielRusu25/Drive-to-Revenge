using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StartRace))]
[System.Serializable]
public class StartingRaceEditor : Editor
{
    private StartRace startingRace;
    private SerializedObject SO;

    //Display
    public SerializedProperty countdownPanelControl;
    public SerializedProperty messageToDisplay;

    //UI
    public SerializedProperty thingsToEnable; 
    public SerializedProperty thingsToDisable;

    //Lights
    public SerializedProperty lights;

    //Player Input
    public SerializedProperty controlActions;
    public SerializedProperty playerInput;

    //Other scripts
    public SerializedProperty trackCheckpoints;
    public SerializedProperty raceAudioManager;

    //In case the race is 'Sprint', we need some extrea stuff
    private SerializedProperty gameMode;
    public SerializedProperty aiCarsContainer;

    private void OnEnable()
    {
        startingRace = (StartRace)target;
        SO = new SerializedObject(target);

        //Display
        countdownPanelControl = SO.FindProperty("countdownPanelControl");
        messageToDisplay = SO.FindProperty("messageToDisplay");

        //UI
        thingsToEnable = SO.FindProperty("thingsToEnable");
        thingsToDisable = SO.FindProperty("thingsToDisable");

        //Lights
        lights = SO.FindProperty("lights");

        //Player Input
        controlActions = SO.FindProperty("controlActions");
        
        //Other Scripts
        trackCheckpoints = SO.FindProperty("trackCheckpoints"); 
        raceAudioManager = SO.FindProperty("raceAudioManager");

        //In case the race is 'Sprint', we need some extrea stuff
        gameMode = SO.FindProperty("gameMode");
        aiCarsContainer = SO.FindProperty("aiCarsContainer");
    }

    public override void OnInspectorGUI()
    {
        SO.Update();

        //Creation of a bold style
        GUIStyle boldStyle = new GUIStyle(GUI.skin.label);
        boldStyle.fontStyle = FontStyle.Bold;

        GUILayout.Space(25);
        GUILayout.Label("Display" , EditorStyles.boldLabel);
        GUILayout.Space(5);
            EditorGUILayout.PropertyField( countdownPanelControl , new GUIContent ("Countdown Panel Control:") );
            EditorGUILayout.PropertyField( messageToDisplay , new GUIContent ("Message to Display") );

        GUILayout.Space(15);
        GUILayout.Label("UI" , EditorStyles.boldLabel);
            EditorGUILayout.PropertyField( thingsToEnable , new GUIContent ("Things to enable at the start of the race ") );
            EditorGUILayout.PropertyField( thingsToDisable , new GUIContent ("Things to disable at the start of the race") );
        
        GUILayout.Space(15);
        GUILayout.Label("Lights" ,  EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(new GUIContent ("They must be put in reverse order of them turning on"));
            EditorGUILayout.PropertyField( lights , GUIContent.none );

        GUILayout.Space(15);
        GUILayout.Label("Playe Input - Control Action to Disable" , EditorStyles.boldLabel);
        GUILayout.Space(5);
            EditorGUILayout.PropertyField( controlActions , new GUIContent ("") );
        
       

        GUILayout.Space(15);
        GUILayout.Label("Other Scripts" , EditorStyles.boldLabel);
        GUILayout.Space(5);
            EditorGUILayout.PropertyField( trackCheckpoints , new GUIContent("Track Checkpoints"));
            EditorGUILayout.PropertyField( raceAudioManager , new GUIContent("Race Audio Manager"));
        
        
        if (trackCheckpoints != null)
        {
            GameMode gm = startingRace.trackCheckpoints.gameMode;
            if (gm == GameMode.Sprint)
            {
                GUILayout.Space(15);
                GUILayout.Label("AI Cars Container" , EditorStyles.boldLabel);
                GUILayout.Space(5);
                    EditorGUILayout.PropertyField (aiCarsContainer ,new GUIContent(""));
        }}

        SO.ApplyModifiedProperties();
    }
}

