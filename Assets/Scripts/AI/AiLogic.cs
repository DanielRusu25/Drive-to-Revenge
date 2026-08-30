using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CarController))]
public class AiLogic : MonoBehaviour
{
    private CarController carController;

    public AiWaypointContainer aiWaypointContainer;
    public int waypointRange;
    private List<Transform> aiWaypoints;

    [Range(0.01f, 0.05f)]
    public float steerAgresion = 0.03f;
    public float maxSpeedInBrakeZone = 60f; //This should be in KPH

    [Range(30, 50)]
    public int reverseDistance;
    [Range(100, 180)]
    public int reverseAngle;

    private int currentWaypoint;
    private float distanceToCheckpoint;
    private float currentSteerAngle;
    private int maxSteerAngle;

    private float gasInput;
    private float gasDampen;
    private bool preveiousReversing;
    [HideInInspector] public bool shouldBrake;



    void Start()
    {
        carController = GetComponent<CarController>();
        maxSteerAngle = carController.maxSteeringAngle;
        aiWaypoints = aiWaypointContainer.aiWaypoints;
        currentWaypoint = 0;
    }

    void Update()
    {
        WaypointDetect();
        Steer();
        Move();
    }

    //This function updates the currentWaypoint to the correct one
    private void WaypointDetect()
    {
        distanceToCheckpoint = Vector3.Distance(aiWaypoints[currentWaypoint].position, transform.position);
        if (distanceToCheckpoint < waypointRange)
            currentWaypoint = (currentWaypoint + 1) % aiWaypoints.Count;

        Debug.DrawRay(transform.position, aiWaypoints[currentWaypoint].position - transform.position, Color.cyan);
    }

    //This function calculates how the car should steer
    private void Steer()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        currentSteerAngle = Vector3.SignedAngle(fwd, aiWaypoints[currentWaypoint].position - transform.position, Vector3.up);



        if ((currentSteerAngle <= -5 || currentSteerAngle >= 5) && (currentSteerAngle >= -175 && currentSteerAngle <= 175))
        {
            if (preveiousReversing == carController.shouldReverse)//If we had just changed orientation, we are NOT steering
            {
                //Steering
                carController.shouldSteer = true;
                carController.steerContextValue = Mathf.Clamp(currentSteerAngle / maxSteerAngle, -1, 1);
            }
            else
            {
                //Not steering
                carController.shouldSteer = false;
                carController.steerContextValue = 0;

                StartCoroutine(WaitThanChangeOrientation(1f));
            }
        }
        else
        {
            //Not steering
            carController.shouldSteer = false;
            carController.steerContextValue = 0;
        }


    }

    private void Move()
    {
        gasInput = Mathf.Clamp01(1f - Mathf.Abs(carController.carSpeed * steerAgresion * currentSteerAngle) / carController.maxSteeringAngle);
        if (shouldBrake == true && carController.carSpeed > maxSpeedInBrakeZone)
            gasInput = -gasInput * (Mathf.Clamp01(carController.carSpeed / maxSpeedInBrakeZone) * 2 - 1);

        if (Mathf.Abs(currentSteerAngle) > reverseAngle && distanceToCheckpoint < reverseDistance)
            gasInput = -gasInput;


        gasDampen = Mathf.Lerp(gasDampen, gasInput, Time.deltaTime * 3f);

        carController.moveContextValue = gasDampen;
    }


    private IEnumerator WaitThanChangeOrientation(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        preveiousReversing = carController.shouldReverse;
    }
}
