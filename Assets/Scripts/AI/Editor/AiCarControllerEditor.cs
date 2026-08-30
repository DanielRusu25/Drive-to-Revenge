using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AiCarController))]
[System.Serializable]
public class AiCarControllerEditor : Editor
{
  private AiCarController aiCarController;
  private SerializedObject SO;
  //
  //
  //CAR SETUP
  //
  //
  //Engine
  private SerializedProperty numberOfGears;
  private SerializedProperty gearRatios;
  private SerializedProperty finalDriveRatio;
  private SerializedProperty shiftTime;

  private SerializedProperty engineHorsepower;
  private SerializedProperty horsepowerToRPMCurve;

  //RPMs
  private SerializedProperty idleRPM;
  private SerializedProperty redlineRPM;
  private SerializedProperty maxRPM;

  //Nitrous
  private SerializedProperty useNitrous;
  private SerializedProperty nitrousPower;
  private SerializedProperty nitrousCapacity;
  private SerializedProperty nitrousUseSpeed;
  private SerializedProperty nitrousRechargeSpeed;

  //Steering
  private SerializedProperty maxSteeringAngle;
  private SerializedProperty steeringSpeed;

  //Brakes
  private SerializedProperty brakeTorque;
  private SerializedProperty handbrakeDriftMultiplier;

  //Other Stuff
  private SerializedProperty drivetrain;
  private SerializedProperty bodyMassCenter;

  //
  //
  //WHEELS VARIABLES
  //
  //
  private SerializedProperty wheelMeshes;
  private SerializedProperty wheelColliders;


  private void OnEnable()
  {
    aiCarController = (AiCarController)target;
    SO = new SerializedObject(target);

    numberOfGears = SO.FindProperty("numberOfGears");
    gearRatios = SO.FindProperty("gearRatios");
    finalDriveRatio = SO.FindProperty("finalDriveRatio");
    shiftTime = SO.FindProperty("shiftTime");

    engineHorsepower = SO.FindProperty("engineHorsepower");
    horsepowerToRPMCurve = SO.FindProperty("horsepowerToRPMCurve");

    idleRPM = SO.FindProperty("idleRPM");
    redlineRPM = SO.FindProperty("redlineRPM");
    maxRPM = SO.FindProperty("maxRPM");

    useNitrous = SO.FindProperty("useNitrous");
    nitrousPower = SO.FindProperty("nitrousPower");
    nitrousCapacity = SO.FindProperty("nitrousCapacity");
    nitrousUseSpeed = SO.FindProperty("nitrousUseSpeed");
    nitrousRechargeSpeed = SO.FindProperty("nitrousRechargeSpeed");

    maxSteeringAngle = SO.FindProperty("maxSteeringAngle");
    steeringSpeed = SO.FindProperty("steeringSpeed");

    brakeTorque = SO.FindProperty("brakeTorque");
    handbrakeDriftMultiplier = SO.FindProperty("handbrakeDriftMultiplier");

    drivetrain = SO.FindProperty("drivetrain");
    bodyMassCenter = SO.FindProperty("bodyMassCenter");

    wheelMeshes = SO.FindProperty("wheelMeshes");
    wheelColliders = SO.FindProperty("wheelColliders");
  }

  public override void OnInspectorGUI()
  {

    SO.Update();

    GUILayout.Space(25);
    GUILayout.Label("CAR SETUP", EditorStyles.boldLabel);
    GUILayout.Space(10);
    //
    //
    //CAR SETUP
    //
    //
    //

    GUILayout.Label("Gearbox");

      numberOfGears.intValue = EditorGUILayout.IntSlider("The Number of Gears: ", numberOfGears.intValue, 4, 10);
      EditorGUILayout.PropertyField(gearRatios, new GUIContent("Gear Ratios: ", "Needs to have the number of gears +1 , gear 0 being the reverse gear"));
      EditorGUILayout.PropertyField(finalDriveRatio, new GUIContent("Final Drive Ratio: "));
      EditorGUILayout.PropertyField(shiftTime, new GUIContent("Shift Time", "In seconds"));

    GUILayout.Space(10);
    GUILayout.Label("Engine");

      EditorGUILayout.PropertyField(engineHorsepower, new GUIContent("Engine Horsepower: "));
      EditorGUILayout.PropertyField(horsepowerToRPMCurve, new GUIContent("Horsepower to RPM curve: "));

    GUILayout.Space(10);
    GUILayout.Label("RPMs");
    
      EditorGUILayout.PropertyField(idleRPM, new GUIContent("Idle RPM: "));
      EditorGUILayout.PropertyField(redlineRPM, new GUIContent("Redline RPM: "));
      EditorGUILayout.PropertyField(maxRPM, new GUIContent("Max RPM: "));

    GUILayout.Space(10);
    GUILayout.Label("Nitrous");
      useNitrous.boolValue = EditorGUILayout.BeginToggleGroup("Use nitrous?", useNitrous.boolValue);
      EditorGUILayout.PropertyField(nitrousPower, new GUIContent("Nitrous Power: ","While the nitrous is active, the torque will be multiplied by this amount"));
      EditorGUILayout.PropertyField(nitrousCapacity , new GUIContent("Nitrous Capacity:"));
      EditorGUILayout.PropertyField(nitrousUseSpeed , new GUIContent("Nitrous Use Speed: "));
      EditorGUILayout.PropertyField(nitrousRechargeSpeed , new GUIContent("Nitrous Recharge Speed: "));
      EditorGUILayout.EndToggleGroup();

    GUILayout.Space(10);
    GUILayout.Label("Steering");

      maxSteeringAngle.intValue = EditorGUILayout.IntSlider("Max Steering Angle:", maxSteeringAngle.intValue, 10, 45);
      steeringSpeed.floatValue = EditorGUILayout.Slider("Steering Speed:", steeringSpeed.floatValue, 0.1f, 1f);

    GUILayout.Space(10);
    GUILayout.Label("Brakes");

      EditorGUILayout.PropertyField(brakeTorque, new GUIContent(" Brake Torque"));
      handbrakeDriftMultiplier.intValue = EditorGUILayout.IntSlider("Drift Multiplier:", handbrakeDriftMultiplier.intValue, 1, 10);

    GUILayout.Space(10);
    GUILayout.Label("Other Stuff");

      EditorGUILayout.PropertyField(drivetrain, new GUIContent("Drivetrain of the car: "));
      EditorGUILayout.PropertyField(bodyMassCenter, new GUIContent("Mass Center of Car: "));

    //
    //
    //WHEELS
    //
    //

    GUILayout.Space(25);
    GUILayout.Label("WHEELS", EditorStyles.boldLabel);
    GUILayout.Space(10);

    EditorGUILayout.HelpBox(new GUIContent($"Wheels must be added like this: \n 0 - front left \n 1 - front right \n 2 - rear left \n 3 - rear right"));
    EditorGUILayout.PropertyField(wheelMeshes, new GUIContent("Wheel meshes: "));
    EditorGUILayout.PropertyField(wheelColliders, new GUIContent("Wheel colliders: "));

    //END

    GUILayout.Space(10);
    SO.ApplyModifiedProperties();

  }

}
