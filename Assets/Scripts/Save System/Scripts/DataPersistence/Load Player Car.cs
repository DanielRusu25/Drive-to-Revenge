using UnityEngine;

public class LoadPlayerCar : MonoBehaviour, IDataPersistence
{

    public CarController carController;
    public WheelCollider[] wheelColliders = new WheelCollider[4];
    
    public void LoadData(GameData data)
    {
        //World space
        carController.transform.position = data.playerPosition;
        carController.transform.eulerAngles = data.playerRotation;

        //Tramsmission
        carController.numberOfGears = data.mainCars[0].transmission.numberOfGears;
        carController.finalDriveRatio = data.mainCars[0].transmission.finalDrive;
        carController.transmisionType = data.mainCars[0].transmission.type;
        carController.drivetrain = data.mainCars[0].transmission.drivetrain;

        //Engine
        carController.engineHorsepower = data.mainCars[0].engine.horsepower;
        carController.idleRPM = data.mainCars[0].engine.idleRPM;
        carController.redlineRPM = data.mainCars[0].engine.redlineRPM;
        carController.maxRPM = data.mainCars[0].engine.maxRPM;

        //Nitrous
        carController.useNitrous = data.mainCars[0].nitrous.useNitrous;
        carController.nitrousPower = data.mainCars[0].nitrous.nitrousPower;
        carController.nitrousCapacity = data.mainCars[0].nitrous.nitrousCapacity;
        carController.nitrousUseSpeed = data.mainCars[0].nitrous.nitrousUseSpeed;
        carController.nitrousRechargeSpeed = data.mainCars[0].nitrous.nitrousRechargeSpeed;

        //Brakes
        carController.brakeTorque = data.mainCars[0].brakes.brakeTorque;

        //Tyres
        foreach (WheelCollider wheelCollider in wheelColliders)
        {
            // Forward friction
            WheelFrictionCurve forwardFriction = wheelCollider.forwardFriction;
            forwardFriction.stiffness = data.mainCars[0].tyres.forwardStiffnes;
            wheelCollider.forwardFriction = forwardFriction;

            // Sideways friction
            WheelFrictionCurve sidewaysFriction = wheelCollider.sidewaysFriction;
            sidewaysFriction.stiffness = data.mainCars[0].tyres.sidewaysStiffnes;
            wheelCollider.sidewaysFriction = sidewaysFriction;
        }
        //Suspension
        carController.maxSteeringAngle = data.mainCars[0].suspension.maxSteeringAngle;
        foreach (WheelCollider wheelCollider in wheelColliders)
        {
            JointSpring suspensionSpring = wheelCollider.suspensionSpring;
            suspensionSpring.spring = data.mainCars[0].suspension.spring;
            suspensionSpring.damper = data.mainCars[0].suspension.dampen;
            wheelCollider.suspensionSpring = suspensionSpring;

            wheelCollider.suspensionDistance = data.mainCars[0].suspension.rideHeight;
            wheelCollider.wheelDampingRate = data.mainCars[0].suspension.dampingRate;


        }

    }

    public void SaveData(GameData data)
    {
        data.playerPosition = carController.transform.position;
        data.playerRotation = carController.transform.eulerAngles;
    }
}
