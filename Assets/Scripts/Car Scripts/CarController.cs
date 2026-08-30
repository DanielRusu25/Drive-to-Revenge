using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Drivetrain
{
  FrontWheelDrive,
  RearWheelDrive,
  FourWheelDrive
}

public enum TransmisionType
{
  Manual,
  Automatic
}

public enum GearState
{
  Running,
  CheckingChange,
  Changing,
}

[RequireComponent(typeof(CarAudio))]
[RequireComponent(typeof(CarEffects))]
public class CarController : MonoBehaviour
{


  //CAR SETUP

  
  //Transmision
  public int numberOfGears ;
  public float[] gearRatios; //Gear 0 is the reverse gear
  public float finalDriveRatio;
  public TransmisionType transmisionType = TransmisionType.Manual;
  public float shiftTime = 0.5f;


  //Engine
  public float engineHorsepower;
  public AnimationCurve horsepowerToRPMCurve;

  public float idleRPM;
  public float redlineRPM;
  public float maxRPM;

  //Nitrous
  public bool useNitrous;
  public float nitrousPower;
  public float nitrousCapacity;
  public int nitrousUseSpeed;
  public int nitrousRechargeSpeed;

  //Steering
  public int maxSteeringAngle = 27; // The maximum angle that the tires can reach while rotating the steering wheel.
  public float steeringSpeed = 0.5f; // How fast the steering wheel turns.

  //Brakes
  public int brakeTorque = 350; // The strength of the wheel brakes.
  public int handbrakeDriftMultiplier;

  //Other Stuff
  public Drivetrain drivetrain = Drivetrain.RearWheelDrive;
  public Vector3 bodyMassCenter; // This is a vector that contains the center of mass of the car. I recommend to set this value
                                 // in the points x = 0 and z = 0 of your car. You can select the value that you want in the y axis,
                                 // however, you must notice that the higher this value is, the more unstable the car becomes.
                                 // Usually the y value goes from 0 to 1.5.

  //WHEELS
  /*
  The following variables are used to store the wheels' data of the car. We need both the mesh-only game objects and wheel
  collider components of the wheels. The wheel collider components and 3D meshes of the wheels cannot come from the same
  game object; they must be separate game objects.
 
  The order of the wheels are like this:
  0 - front left
  1 - front right
  2 - rear left
  3 - rear right
  */
  public GameObject[] wheelMeshes;
  public WheelCollider[] wheelColliders;


  //PRIVATE VARIABLES - some variables are public because they are used in other scripts

  /*
  IMPORTANT: The following variables should not be modified manually since their values are automatically given via script.
  */
  Rigidbody carRigidbody; // Stores the car's rigidbody.
  float steeringAxis; // Used to know whether the steering wheel has reached the maximum value. It goes from -1 to 1.
  public float moveAxis;
  float driftingAxis;
  public int carSpeed; // It is used in km/h
  public float localVelocityX;

  //Engine variables
  public int currentGear;
  public float currentEngineRPM;
  private float currentWheelRPM;
  private float currentEngineTorque;
  public GearState gearState; //Used only in the automatic transmision

  //Nitrous variables
  public float currentNitrousCapacity;
  private float currentNitrousPower;


  //Action variables
  //Most of the following variables are modifed outside this script
  //If we want the player to control the car, they will be inside the "ApplyInput" script
  //If we want the AI to control the car, they will be insde the "AiLogic" script

  //These bools are set to true only when we want their action to be done
  public bool shouldSteer;
  public bool shouldHandbrake;
  public bool shouldShift;
  public bool shouldReverse;
  public bool shouldNitro;

  public bool isDrifting; // Used to know whether the car is drifting or not.
  public bool isTractionLocked; // Used to know whether the traction of the car is locked or not.


  //The following variables are used to store information about the input values
  public float steerContextValue;
  public float moveContextValue;
  public int gearContextValue;

  /*
  The following variables are used to store information about sideways friction of the wheels (such as
  extremumSlip,extremumValue, asymptoteSlip, asymptoteValue and stiffness). We change this values to
  make the car to start drifting.
  */
  private WheelFrictionCurve[] wheelFrictionCurves;
  private float[] extremumSlips;



  // Start is called before the first frame update
  void Start()
  {
    //In this part, we set the 'carRigidbody' value with the Rigidbody attached to this
    //gameObject. Also, we define the center of mass of the car with the Vector3 given
    //in the inspector.
    carRigidbody = gameObject.GetComponent<Rigidbody>();
    carRigidbody.centerOfMass = bodyMassCenter;

    //Initial setup to calculate the drift value of the car. This part could look a bit
    //complicated, but do not be afraid, the only thing we're doing here is to save the default
    //friction values of the car wheels so we can set an appropiate drifting value later.

    wheelFrictionCurves = new WheelFrictionCurve[wheelColliders.Length];
    extremumSlips = new float[wheelColliders.Length];

    for (int i = 0; i < wheelColliders.Length; i++)
    {
      wheelFrictionCurves[i] = new WheelFrictionCurve
      {
        extremumSlip = wheelColliders[i].sidewaysFriction.extremumSlip,
        extremumValue = wheelColliders[i].sidewaysFriction.extremumValue,
        asymptoteSlip = wheelColliders[i].sidewaysFriction.asymptoteSlip,
        asymptoteValue = wheelColliders[i].sidewaysFriction.asymptoteValue,
        stiffness = wheelColliders[i].sidewaysFriction.stiffness
      };

      extremumSlips[i] = wheelColliders[i].sidewaysFriction.extremumSlip;
    }

    //We setup the gearbox system
    numberOfGears = gearRatios.Length - 1;
    currentGear = 1;
    if (transmisionType == TransmisionType.Automatic)
      gearState = GearState.Running;

    //We setup the nitrous system
    currentNitrousPower = 1f;
    if (useNitrous == true)
    {
      currentNitrousCapacity = nitrousCapacity;
      shouldNitro = false;
    }

  }

  // Update is called once per frame
  void Update()
  {
    //CAR DATA

    // We determine the speed of the car.
    carSpeed = Convert.ToInt32(carRigidbody.linearVelocity.magnitude * 3.6f);

    // We slowly update the move axis 
    moveAxis = Mathf.Lerp(moveAxis, moveContextValue, Time.deltaTime);

    // Save the local velocity of the car in the x axis. Used to know if the car is drifting.
    localVelocityX = transform.InverseTransformDirection(carRigidbody.linearVelocity).x;

    //CAR PHYSICS

    //Engine and Braking
    CalculateEngineRPM();
    if (transmisionType == TransmisionType.Manual)
      StartCoroutine(GearShift(gearContextValue));
    ApplyMovement();

    //Nitrous
    if (useNitrous == true)
      CalculateNitrous();

    //Handbrake
    if (shouldHandbrake == true)
      Handbrake();
    else
      RecoverTraction();

    //Steering
    if (shouldSteer == true)
      Steer();
    else
      ResetSteeringAngle();

    // We call this method in order to match the wheel collider movements with the 3D meshes of the wheels.
    AnimateWheelMeshes();

  }


  //
  //ENGINE AND BRAKING METHODS
  //



  //This function controls the automatic and manual gearbox system
  public IEnumerator GearShift(int gearShift)
  {
    {
      if (transmisionType == TransmisionType.Manual)
      {
        //Manual code here
        if (shouldShift == true)
        {
          if (currentGear + gearShift <= numberOfGears && currentGear + gearShift >= 0)
            currentGear += gearShift;
  
          yield return new WaitForEndOfFrame();
          shouldShift = false;
        }
        yield break;
      }
      else
      {
        //Automatic code here
        gearState = GearState.CheckingChange;
        if (currentGear + gearShift >= 1)
        {
          if (gearShift > 0)
          {
            yield return new WaitForSeconds(0.7f);
            if (currentEngineRPM < redlineRPM || currentGear >= numberOfGears)
            {
              gearState = GearState.Running;
              yield break;
            }
          }

          if (gearShift < 0)
          {
            yield return new WaitForSeconds(0.3f);
            if (currentEngineRPM > redlineRPM / 2 || currentGear < 1)
            {
              gearState = GearState.Running;
              yield break;
            }
          }

          gearState = GearState.Changing;
          yield return new WaitForSeconds(shiftTime);
          currentGear += gearShift;
        }
        gearState = GearState.Running;
      }
    }
  }

  //In this method we calculate the current engine RPM and calculate the torque of the car
  //We do some calculations for the automatic gearbox in this method
  public void CalculateEngineRPM()
  {

    if (transmisionType == TransmisionType.Automatic)
    {
      if (Mathf.Abs(carSpeed) == 0)
      {
        if (moveAxis < 0)
          shouldReverse = true;
        else
          shouldReverse = false;
      }


      if (shouldReverse == true)
        currentGear = 0;
      else if (gearState == GearState.Running)
      {
        if (currentGear == 0)
          currentGear = 1;
        else
        {
          if (currentEngineRPM > redlineRPM)
            StartCoroutine(GearShift(1));

          if (currentEngineRPM < redlineRPM / 2)
            StartCoroutine(GearShift(-1));
        }
      }

    }

    if ((moveAxis <= 0 && shouldReverse == false) || (moveAxis > 0 && shouldReverse == true))
    {
      currentEngineRPM = Mathf.Lerp(currentEngineRPM, Mathf.Max(idleRPM, redlineRPM * moveAxis) + Random.Range(-50, 50), Time.deltaTime);
      currentEngineTorque = 0;
    }
    else
    {
      //Calculating the current wheel RPM, from the average of all wheels in use 
      switch (drivetrain)
      {
        case Drivetrain.FrontWheelDrive:
          currentWheelRPM = Mathf.Abs((wheelColliders[0].rpm + wheelColliders[1].rpm) / 2f)
                            * gearRatios[currentGear] * finalDriveRatio;
          break;

        case Drivetrain.RearWheelDrive:
          currentWheelRPM = Mathf.Abs((wheelColliders[2].rpm + wheelColliders[3].rpm) / 2f)
                            * gearRatios[currentGear] * finalDriveRatio;
          break;

        case Drivetrain.FourWheelDrive:
          currentWheelRPM = Mathf.Abs((wheelColliders[0].rpm + wheelColliders[1].rpm + wheelColliders[2].rpm + wheelColliders[3].rpm) / 4f)
                            * gearRatios[currentGear] * finalDriveRatio;
          break;
      }

      //Calculating the Engine RPM with the average Wheel RPM, and than getting the engine torque
      currentEngineRPM = Mathf.Lerp(currentEngineRPM, Mathf.Max(idleRPM - 100, currentWheelRPM), Time.deltaTime * 3f);

      currentEngineTorque = (horsepowerToRPMCurve.Evaluate(currentEngineRPM / maxRPM) * engineHorsepower / currentEngineRPM)
                            * gearRatios[currentGear] * finalDriveRatio * 5252f
                            * Mathf.Abs(moveAxis) * currentNitrousPower;
    }
  }

  //In this method we apply the engine torque to the wheels, or brake accordingly
  public void ApplyMovement()
  {
    if ((moveAxis >= 0 && shouldReverse == false) || (moveAxis < 0 && shouldReverse == true))
    {
      RemoveBrakes();

      switch (drivetrain)
      {
        case Drivetrain.FrontWheelDrive:
          wheelColliders[0].motorTorque = currentEngineTorque;
          wheelColliders[1].motorTorque = currentEngineTorque;
          break;

        case Drivetrain.RearWheelDrive:
          wheelColliders[2].motorTorque = currentEngineTorque;
          wheelColliders[3].motorTorque = currentEngineTorque;
          break;

        case Drivetrain.FourWheelDrive:
          wheelColliders[0].motorTorque = currentEngineTorque / 2;
          wheelColliders[1].motorTorque = currentEngineTorque / 2;
          wheelColliders[2].motorTorque = currentEngineTorque / 2;
          wheelColliders[3].motorTorque = currentEngineTorque / 2;
          break;

      }
    }
    else
    {
      FootBrake();
    }
  }

  //This function sets all wheel's brake torque to 0
  public void RemoveBrakes()
  {
    wheelColliders[0].brakeTorque = 0;
    wheelColliders[1].brakeTorque = 0;
    wheelColliders[2].brakeTorque = 0;
    wheelColliders[3].brakeTorque = 0;
  }

  // This function applies brake torque to the wheels according to the brake force given by the user.
  public void FootBrake()
  {
    for (int i = 0; i < wheelColliders.Length; i++)
    {
      wheelColliders[i].brakeTorque = Mathf.Abs(brakeTorque * moveAxis);
      wheelColliders[i].motorTorque = currentEngineTorque;
    }
  }


  //
  //STEERING METHODS
  //

  

  //The following method takes the front wheels and rotates them to the disered position.
  //The speed of this movement will depend on the 'steeringSpeed' variable.
  public void Steer()
  {
    steeringAxis = Mathf.Lerp(steeringAxis, steerContextValue, Time.deltaTime * steeringSpeed * 10f);

    var steeringAngle = steeringAxis * maxSteeringAngle;
    wheelColliders[0].steerAngle = Mathf.Lerp(wheelColliders[0].steerAngle, steeringAngle, steeringSpeed);
    wheelColliders[1].steerAngle = Mathf.Lerp(wheelColliders[1].steerAngle, steeringAngle, steeringSpeed);
  }

  //The following method takes the front car wheels to their default position (rotation = 0). The speed of this movement will depend
  // on the steeringSpeed variable.
  public void ResetSteeringAngle()
  {
    {
      if (steeringAxis < 0f)
        steeringAxis = steeringAxis + (Time.deltaTime * steeringSpeed * 10f);
      else if (steeringAxis > 0f)
        steeringAxis = steeringAxis - (Time.deltaTime * steeringSpeed * 10f);

      if (Mathf.Abs(wheelColliders[0].steerAngle) < 5f)
        steeringAxis = 0f;

      var steeringAngle = steeringAxis * maxSteeringAngle;
      wheelColliders[0].steerAngle = Mathf.Lerp(wheelColliders[0].steerAngle, steeringAngle, steeringSpeed);
      wheelColliders[1].steerAngle = Mathf.Lerp(wheelColliders[1].steerAngle, steeringAngle, steeringSpeed);
    }
  }


  //
  //Handbrake
  //


  // This function is used to make the car lose traction. By using this, the car will start drifting. The amount of traction lost
  // will depend on the handbrakeDriftMultiplier variable. If this value is small, then the car will not drift too much, but if
  // it is high, then you could make the car to feel like going on ice.
  public void Handbrake()
  {
    CancelInvoke("RecoverTraction");
    // We are going to start losing traction smoothly, there is were our 'driftingAxis' variable takes
    // place. This variable will start from 0 and will reach a top value of 1, which means that the maximum
    // drifting value has been reached. It will increase smoothly by using the variable Time.deltaTime.
    driftingAxis = driftingAxis + (Time.deltaTime);
    float secureStartingPoint = driftingAxis * extremumSlips[0] * handbrakeDriftMultiplier;

    if (secureStartingPoint < extremumSlips[0])
      driftingAxis = extremumSlips[0] / (extremumSlips[0] * handbrakeDriftMultiplier);

    if (driftingAxis > 1f)
      driftingAxis = 1f;

    //If the 'driftingAxis' value is not 1f, it means that the wheels have not reach their maximum drifting
    //value, so, we are going to continue increasing the sideways friction of the wheels until driftingAxis
    // = 1f.
    if (driftingAxis < 1f)
      for (int i = 0; i < wheelColliders.Length; i++)
      {
        wheelFrictionCurves[i].extremumSlip = extremumSlips[i] * handbrakeDriftMultiplier * driftingAxis;
        wheelColliders[i].sidewaysFriction = wheelFrictionCurves[i];
      }

    // Whenever the player uses the handbrake, it means that the wheels are locked, so we set 'isTractionLocked = true'
    // and, as a consequense, the car starts to emit trails to simulate the wheel skids.
    isTractionLocked = true;

    //In the end, apply the brakes to the rear wheels
    wheelColliders[2].brakeTorque = brakeTorque / 2;
    wheelColliders[3].brakeTorque = brakeTorque / 2;
  }

  // This function is used to recover the traction of the car when the user has stopped using the car's handbrake.
  public void RecoverTraction()
  {
    isTractionLocked = false;
    driftingAxis = driftingAxis - (Time.deltaTime / 1.5f);
    if (driftingAxis < 0f)
    {
      driftingAxis = 0f;
    }

    //If the 'driftingAxis' value is not 0f, it means that the wheels have not recovered their traction.
    //We are going to continue decreasing the sideways friction of the wheels until we reach the initial
    // car's grip.
    if (wheelFrictionCurves[0].extremumSlip > extremumSlips[0])
    {
      for (int i = 0; i < wheelColliders.Length; i++)
      {
        wheelFrictionCurves[i].extremumSlip = extremumSlips[i] * handbrakeDriftMultiplier * driftingAxis;
        wheelColliders[i].sidewaysFriction = wheelFrictionCurves[i];
      }

      Invoke("RecoverTraction", Time.deltaTime);

    }
    else if (wheelFrictionCurves[0].extremumSlip < extremumSlips[0])
    {
      for (int i = 0; i < wheelColliders.Length; i++)
      {
        wheelFrictionCurves[i].extremumSlip = extremumSlips[i];
        wheelColliders[i].sidewaysFriction = wheelFrictionCurves[i];
      }

      driftingAxis = 0f;
    }
  }


  //
  //Nitrous
  //

  public void CalculateNitrous()
  {
    if (shouldNitro == true)
    {
      if ((int)currentNitrousCapacity > 0 && currentEngineTorque > 0) 
      {
        //Using the nitrous
        currentNitrousPower = nitrousPower;
        currentNitrousCapacity -= nitrousUseSpeed * Time.deltaTime;
        if (currentNitrousCapacity <= 0)
          currentNitrousCapacity = 0;
      }
      else
      {
        //Ran out of nitrous
        shouldNitro = false;
      }
    }
    else
    {
      //Recharging the nitrous
      currentNitrousPower = 1;
      currentNitrousCapacity += nitrousRechargeSpeed * Time.deltaTime;
      if(currentNitrousCapacity >= nitrousCapacity)
        currentNitrousCapacity = nitrousCapacity;
    }
  }

  //
  //Extra Methods
  //

  // This method matches both the position and rotation of the WheelColliders with the WheelMeshes.
  void AnimateWheelMeshes()
  {
    try
    {
      Quaternion FLWRotation;
      Vector3 FLWPosition;
      wheelColliders[0].GetWorldPose(out FLWPosition, out FLWRotation);
      wheelMeshes[0].transform.position = FLWPosition;
      wheelMeshes[0].transform.rotation = FLWRotation;

      Quaternion FRWRotation;
      Vector3 FRWPosition;
      wheelColliders[1].GetWorldPose(out FRWPosition, out FRWRotation);
      wheelMeshes[1].transform.position = FRWPosition;
      wheelMeshes[1].transform.rotation = FRWRotation;

      Quaternion RLWRotation;
      Vector3 RLWPosition;
      wheelColliders[2].GetWorldPose(out RLWPosition, out RLWRotation);
      wheelMeshes[2].transform.position = RLWPosition;
      wheelMeshes[2].transform.rotation = RLWRotation;

      Quaternion RRWRotation;
      Vector3 RRWPosition;
      wheelColliders[3].GetWorldPose(out RRWPosition, out RRWRotation);
      wheelMeshes[3].transform.position = RRWPosition;
      wheelMeshes[3].transform.rotation = RRWRotation;
    }
    catch (Exception ex)
    {
      Debug.LogWarning(ex);
    }
  }

}
