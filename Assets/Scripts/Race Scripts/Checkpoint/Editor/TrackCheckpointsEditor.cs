using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(TrackCheckpoints))]
[System.Serializable]
public class TrackCheckpointsEditor : Editor
{
    private TrackCheckpoints trackCheckpoints;
    private SerializedObject SO;

    //Race Type Systems
    private SerializedProperty raceType;
    private SerializedProperty lapSystem;

    //Game Modes Systems
    private SerializedProperty gameMode;
    private SerializedProperty timeAttackManager;
    private SerializedProperty sprintManager; // may be modified

    //Countdown system variables
    private SerializedProperty countdownPanelControl;
    private SerializedProperty messageToDisplay;
    private SerializedProperty countdownTime;

    //Black screen system variables;
    private SerializedProperty blackPanel;
    private SerializedProperty blackFadeSpeed;

    //Teleport system variables
    private SerializedProperty missedCheckopointDistance;
    private SerializedProperty checkpointTeleportDistance;

    //Checkpoint control system variables
    public SerializedProperty numberOfActiveCheckpoints;

    //Sound System
    public SerializedProperty raceAudioManager;


    private void OnEnable()
    {
        trackCheckpoints = (TrackCheckpoints)target;
        SO = new SerializedObject(target);

        //Race Type Systems
        raceType = SO.FindProperty("raceType");
        lapSystem = SO.FindProperty("lapSystem");

        //Game Modes Systems
        gameMode = SO.FindProperty("gameMode");
        timeAttackManager = SO.FindProperty("timeAttackManager");
        sprintManager = SO.FindProperty("sprintManager");

        //Coutdown system variables
        countdownPanelControl = SO.FindProperty("countdownPanelControl");  
        messageToDisplay = SO.FindProperty("messageToDisplay");
        countdownTime = SO.FindProperty("countdownTime");

        //Black screen system variables
        blackPanel = SO.FindProperty("blackPanel");
        blackFadeSpeed = SO.FindProperty("blackFadeSpeed");

        //Teleport system variables
        missedCheckopointDistance = SO.FindProperty("missedCheckopointDistance");
        checkpointTeleportDistance = SO.FindProperty("checkpointTeleportDistance");

        //Checkpoint control system variables
        numberOfActiveCheckpoints = SO.FindProperty("numberOfActiveCheckpoints");

        //Sound System
        raceAudioManager = SO.FindProperty("raceAudioManager");
    }

    public override void OnInspectorGUI()
    {
        SO.Update();


        GUILayout.Space(25);
        GUILayout.Label("Race Type",EditorStyles.boldLabel);
            EditorGUILayout.PropertyField( raceType , GUIContent.none);
            RaceType rt = (RaceType)raceType.enumValueIndex;
            if (rt == RaceType.Lap)
                EditorGUILayout.PropertyField( lapSystem , new GUIContent("Lap System script") );
            else
                lapSystem.objectReferenceValue = null;

        GUILayout.Space(25);
        GUILayout.Label("Game Mode:",EditorStyles.boldLabel);
            EditorGUILayout.PropertyField (gameMode , GUIContent.none);
            GameMode gm = (GameMode)gameMode.enumValueIndex;
            if (gm == GameMode.TimeAttack)
                {
                    EditorGUILayout.PropertyField (timeAttackManager ,new GUIContent("Time Attack script"));
                    sprintManager.objectReferenceValue = null;
                }
            else if(gm == GameMode.Sprint)
                {
                    EditorGUILayout.PropertyField (sprintManager ,new GUIContent("Sprint script"));
                    timeAttackManager.objectReferenceValue = null;
                }
        GUILayout.Space(25);
        GUILayout.Label("Countdown System",EditorStyles.boldLabel);
        GUILayout.Space(5);
            EditorGUILayout.PropertyField( countdownPanelControl , new GUIContent("Countdown Panel Control Script: "));
            EditorGUILayout.PropertyField( messageToDisplay  , new GUIContent("Message to display: "));
            EditorGUILayout.PropertyField( countdownTime , new GUIContent("Time of the countdown: " , "In seonds"));
        
        GUILayout.Space(25);
        GUILayout.Label("Black Screen System",EditorStyles.boldLabel);
        GUILayout.Space(5);
            EditorGUILayout.PropertyField( blackPanel , new GUIContent("Black Panel: "));
            EditorGUILayout.PropertyField( blackFadeSpeed , new GUIContent("How fast to fade in/out: "));

        GUILayout.Space(25);
        GUILayout.Label("Teleport System",EditorStyles.boldLabel);
        GUILayout.Space(5);
            GUILayout.Label("Distances:");
            EditorGUILayout.PropertyField( missedCheckopointDistance , new GUIContent("For checkpoint to be missed:"));
            EditorGUILayout.PropertyField( checkpointTeleportDistance , new GUIContent("From checkpoint to teleport:")); 

        GUILayout.Space(25);
        GUILayout.Label("Checkpoint Control System", EditorStyles.boldLabel);
        GUILayout.Space(5);
            EditorGUILayout.PropertyField(numberOfActiveCheckpoints , new GUIContent("How many checkpoints active:"));

        GUILayout.Space(25);
        GUILayout.Label("Audio", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField( raceAudioManager , new GUIContent("Race Audio Manager:") );

        SO.ApplyModifiedProperties();
    }
}

