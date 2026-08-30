using System;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeTier
{
    None = -1,
    Stock = 0,
    Street = 1,
    Sport = 2,
    Race = 3
}

public enum UpgradeType
{
    Engine,                         //Horsepower
    Brakes,                         //Brake torque
    Transmission,                   //Gear ratios
    Driveshaft,                     //Shift Time
    Drivetrain,                     //Drivetrain  - has only 3 tiers(AWD,RWD,FWD)          
    Suspension,                     //Max Steer Angle
    Tyers,                          //Stiffness (forwardn and sideways)                      
    Nitrous,                        //Active, Power, Capacity, Use Speed, Recharge Speed
    // Add more as needed
}


[Serializable]
public struct UpgradeState
{
    public UpgradeType type;
    public UpgradeTier currentTier;
}

[System.Serializable]
public class GameData
{
    public long lastUpdated;

    //Normal Player Variabels
    public Vector3 playerPosition;
    public Vector3 playerRotation;

    public int currentCarIndex;
    public int currentMoney;

    //Car Variabels
    [Serializable]
    public class Car
    {
        //Standard
        public string name;
        public int carIndex;
        public bool owned;

        //Transmission
        [Serializable]
        public class Transmission
        {
            public int numberOfGears;
            public float[] gearRatios;
            public float finalDrive;
            public float shifTime;
            public Drivetrain drivetrain;
            public TransmisionType type;
        }
        public Transmission transmission = new Transmission();

        //Engine
        [Serializable]
        public class Engine
        {
            public int horsepower;
            public float idleRPM;
            public float redlineRPM;
            public float maxRPM;
        }
        public Engine engine = new Engine();

        //Nitrous
        [Serializable]
        public class Nitrous
        {
            public bool useNitrous;
            public float nitrousPower;
            public float nitrousCapacity;
            public int nitrousUseSpeed;
            public int nitrousRechargeSpeed;
        }
        public Nitrous nitrous = new Nitrous();

        //Brakes
        [Serializable]
        public class Brakes
        {
            public int brakeTorque;
        }
        public Brakes brakes = new Brakes();

        //Tiers

        [Serializable]
        public class Tyres
        {
            public float forwardStiffnes;
            public float sidewaysStiffnes;
        }
        public Tyres tyres = new Tyres();


        //Suspension
        [Serializable]
        public class Suspension
        {
            public int maxSteeringAngle;
            public int spring;
            public int dampen;
            public float rideHeight;
            public float dampingRate;
        }
        public Suspension suspension = new Suspension();

        //Upgrades
        public List<UpgradeState> upgrades;

        //Meshes
        [Serializable]
        public class VisualUpgrades
        {
            public string bodyHexColor;
            public int wheelId;
            [Serializable]
            public class WheelCoords
            {
                public float wheelX;
                public float wheelY;
                public float wheelFrontZ;
                public float wheelRearZ;
            }
            public WheelCoords wheelCoords = new WheelCoords();
        }
        public VisualUpgrades visualUpgrades = new VisualUpgrades();

        //Sounds
        [Serializable]
        public class Sounds
        {
            public string lowAccelClipName;
            public string lowDecelClipName;
            public string highAccelClipName;
            public string highDecelClipName;
        }
        public Sounds sounds = new Sounds();


    }

    public Car[] mainCars = new Car[6];

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData()
    {
        playerPosition = new Vector3(-280, -1, 110);
        playerRotation = new Vector3(0, 65, 0);


        currentMoney = 500000;


        //
        // Current Car
        //
        {
            mainCars[0] = new Car();
            mainCars[0].carIndex = 1;
            mainCars[0].name = "Mustang";
            mainCars[0].owned = true;

            // Transmission
            mainCars[0].transmission.numberOfGears = 6;
            mainCars[0].transmission.gearRatios = new float[] { -3.8f, 3.2f, 2.0f, 1.4f, 1.0f, 0.8f };
            mainCars[0].transmission.finalDrive = 2.4f;
            mainCars[0].transmission.shifTime = 0.15f;
            mainCars[0].transmission.drivetrain = Drivetrain.RearWheelDrive;
            mainCars[0].transmission.type = TransmisionType.Manual;

            // Engine
            mainCars[0].engine.horsepower = 180;
            mainCars[0].engine.idleRPM = 900;
            mainCars[0].engine.redlineRPM = 5500;
            mainCars[0].engine.maxRPM = 7500;

            // Nitrous
            mainCars[0].nitrous.useNitrous = false;

            // Brakes
            mainCars[0].brakes.brakeTorque = 1800;

            // Tyres
            mainCars[0].tyres.forwardStiffnes = 1.1f;
            mainCars[0].tyres.sidewaysStiffnes = 1.8f;

            // Suspension
            mainCars[0].suspension.maxSteeringAngle = 42;
            mainCars[0].suspension.spring = 30000;
            mainCars[0].suspension.dampen = 4000;
            mainCars[0].suspension.rideHeight = 0.32f;
            mainCars[0].suspension.dampingRate = 0.23f;

            // Upgrades
            mainCars[0].upgrades = new List<UpgradeState>();
            mainCars[0].upgrades.Add(new UpgradeState { type = UpgradeType.Engine, currentTier = UpgradeTier.Stock });
            mainCars[0].upgrades.Add(new UpgradeState { type = UpgradeType.Brakes, currentTier = UpgradeTier.Stock });
            mainCars[0].upgrades.Add(new UpgradeState { type = UpgradeType.Transmission, currentTier = UpgradeTier.Stock });
            mainCars[0].upgrades.Add(new UpgradeState { type = UpgradeType.Driveshaft, currentTier = UpgradeTier.Stock });
            mainCars[0].upgrades.Add(new UpgradeState { type = UpgradeType.Suspension, currentTier = UpgradeTier.Stock });
            mainCars[0].upgrades.Add(new UpgradeState { type = UpgradeType.Tyers, currentTier = UpgradeTier.Stock });
            mainCars[0].upgrades.Add(new UpgradeState { type = UpgradeType.Nitrous, currentTier = UpgradeTier.None });

            // Visual Upgrades
            mainCars[0].visualUpgrades.wheelId = 0;
            mainCars[0].visualUpgrades.bodyHexColor = "#0000FF ";
            mainCars[0].visualUpgrades.wheelCoords.wheelX = 0.95f;
            mainCars[0].visualUpgrades.wheelCoords.wheelY = 0.6f;
            mainCars[0].visualUpgrades.wheelCoords.wheelFrontZ = 1.7f;
            mainCars[0].visualUpgrades.wheelCoords.wheelRearZ = -1.4f;
        }


        //
        // Mustang
        //
        {
            mainCars[1] = new Car();
            mainCars[1].carIndex = 1;
            mainCars[1].name = "Mustang";
            mainCars[1].owned = true;

            // Transmission
            mainCars[1].transmission.numberOfGears = 6;
            mainCars[1].transmission.gearRatios = new float[] { -3.8f, 3.2f, 2.0f, 1.4f, 1.0f, 0.8f, 0.6f }; //Needs to have the max number of gears, even though they will not be used
            mainCars[1].transmission.finalDrive = 2.4f;
            mainCars[1].transmission.shifTime = 0.15f;
            mainCars[1].transmission.drivetrain = Drivetrain.RearWheelDrive;
            mainCars[1].transmission.type = TransmisionType.Manual;

            // Engine
            mainCars[1].engine.horsepower = 180;
            mainCars[1].engine.idleRPM = 900;
            mainCars[1].engine.redlineRPM = 5500;
            mainCars[1].engine.maxRPM = 7500;

            // Nitrous
            mainCars[1].nitrous.useNitrous = false;

            // Brakes
            mainCars[1].brakes.brakeTorque = 1800;

            // Tyres
            mainCars[1].tyres.forwardStiffnes = 1.1f;
            mainCars[1].tyres.sidewaysStiffnes = 1.8f;

            // Suspension
            mainCars[1].suspension.maxSteeringAngle = 42;
            mainCars[1].suspension.spring = 30000;
            mainCars[1].suspension.dampen = 4000;
            mainCars[1].suspension.rideHeight = 0.32f;
            mainCars[1].suspension.dampingRate = 0.23f;

            // Upgrades
            mainCars[1].upgrades = new List<UpgradeState>();
            mainCars[1].upgrades.Add(new UpgradeState { type = UpgradeType.Engine, currentTier = UpgradeTier.Stock });
            mainCars[1].upgrades.Add(new UpgradeState { type = UpgradeType.Brakes, currentTier = UpgradeTier.Stock });
            mainCars[1].upgrades.Add(new UpgradeState { type = UpgradeType.Transmission, currentTier = UpgradeTier.Stock });
            mainCars[1].upgrades.Add(new UpgradeState { type = UpgradeType.Driveshaft, currentTier = UpgradeTier.Stock });
            mainCars[1].upgrades.Add(new UpgradeState { type = UpgradeType.Suspension, currentTier = UpgradeTier.Stock });
            mainCars[1].upgrades.Add(new UpgradeState { type = UpgradeType.Tyers, currentTier = UpgradeTier.Stock });
            mainCars[1].upgrades.Add(new UpgradeState { type = UpgradeType.Nitrous, currentTier = UpgradeTier.None });

            // Visual Upgrades
            mainCars[1].visualUpgrades.wheelId = 0;
            mainCars[1].visualUpgrades.bodyHexColor = "#0000FF ";
            mainCars[1].visualUpgrades.wheelCoords.wheelX = 0.95f;
            mainCars[1].visualUpgrades.wheelCoords.wheelY = 0.6f;
            mainCars[1].visualUpgrades.wheelCoords.wheelFrontZ = 1.7f;
            mainCars[1].visualUpgrades.wheelCoords.wheelRearZ = -1.4f;
        }

        //
        // Camaro
        //
        {
            mainCars[2] = new Car();
            mainCars[2].carIndex = 2;
            mainCars[2].name = "Camaro";
            mainCars[2].owned = false;

            // Transmission
            mainCars[2].transmission.numberOfGears = 6;
            mainCars[2].transmission.gearRatios = new float[] { -4.0f, 3.5f, 2.2f, 1.6f, 1.2f, 1.0f , 0.8f }; //Needs to have the max number of gears, even though they will not be used
            mainCars[2].transmission.finalDrive = 2.6f;
            mainCars[2].transmission.shifTime = 0.12f;
            mainCars[2].transmission.drivetrain = Drivetrain.RearWheelDrive;
            mainCars[2].transmission.type = TransmisionType.Manual;

            // Engine
            mainCars[2].engine.horsepower = 220;
            mainCars[2].engine.idleRPM = 950;
            mainCars[2].engine.redlineRPM = 6200;
            mainCars[2].engine.maxRPM = 8000;

            // Nitrous
            mainCars[2].nitrous.useNitrous = false;

            // Brakes
            mainCars[2].brakes.brakeTorque = 2100;

            // Tyres
            mainCars[2].tyres.forwardStiffnes = 1.3f;
            mainCars[2].tyres.sidewaysStiffnes = 2.1f;

            // Suspension
            mainCars[2].suspension.maxSteeringAngle = 44;
            mainCars[2].suspension.spring = 36000;
            mainCars[2].suspension.dampen = 4600;
            mainCars[2].suspension.rideHeight = 0.29f;
            mainCars[2].suspension.dampingRate = 0.26f;

            // Upgrades
            mainCars[2].upgrades = new List<UpgradeState>();
            mainCars[2].upgrades.Add(new UpgradeState { type = UpgradeType.Engine, currentTier = UpgradeTier.Stock });
            mainCars[2].upgrades.Add(new UpgradeState { type = UpgradeType.Brakes, currentTier = UpgradeTier.Stock });
            mainCars[2].upgrades.Add(new UpgradeState { type = UpgradeType.Transmission, currentTier = UpgradeTier.Stock });
            mainCars[2].upgrades.Add(new UpgradeState { type = UpgradeType.Driveshaft, currentTier = UpgradeTier.Stock });
            mainCars[2].upgrades.Add(new UpgradeState { type = UpgradeType.Suspension, currentTier = UpgradeTier.Stock });
            mainCars[2].upgrades.Add(new UpgradeState { type = UpgradeType.Tyers, currentTier = UpgradeTier.Stock });
            mainCars[2].upgrades.Add(new UpgradeState { type = UpgradeType.Nitrous, currentTier = UpgradeTier.None });

            // Visual Upgrades
            mainCars[2].visualUpgrades.wheelId = 1;
            mainCars[2].visualUpgrades.bodyHexColor = "#FFD700";
            mainCars[2].visualUpgrades.wheelCoords.wheelX = 0.95f;
            mainCars[2].visualUpgrades.wheelCoords.wheelY = 0.7f;
            mainCars[2].visualUpgrades.wheelCoords.wheelFrontZ = 1.55f;
            mainCars[2].visualUpgrades.wheelCoords.wheelRearZ = -1.3f;
        }

        //
        // Supra
        //
        {
            mainCars[3] = new Car();
            mainCars[3].carIndex = 3;
            mainCars[3].name = "Supra";
            mainCars[3].owned = false;

            // Transmission
            mainCars[3].transmission.numberOfGears = 7;
            mainCars[3].transmission.gearRatios = new float[] { -4.3f, 3.6f, 2.1f, 1.5f, 1.1f, 0.9f, 0.7f , 0.5f }; //Needs to have the max number of gears, even though they will not be used
            mainCars[3].transmission.finalDrive = 2.5f;
            mainCars[3].transmission.shifTime = 0.1f;
            mainCars[3].transmission.drivetrain = Drivetrain.FourWheelDrive;
            mainCars[3].transmission.type = TransmisionType.Manual;

            // Engine
            mainCars[3].engine.horsepower = 200;
            mainCars[3].engine.idleRPM = 1000;
            mainCars[3].engine.redlineRPM = 6000;
            mainCars[3].engine.maxRPM = 8000;

            // Nitrous
            mainCars[3].nitrous.useNitrous = false;

            // Brakes
            mainCars[3].brakes.brakeTorque = 2000;

            // Tyres
            mainCars[3].tyres.forwardStiffnes = 1.2f;
            mainCars[3].tyres.sidewaysStiffnes = 2f;

            // Suspension
            mainCars[3].suspension.maxSteeringAngle = 45;
            mainCars[3].suspension.spring = 35000;
            mainCars[3].suspension.dampen = 4500;
            mainCars[3].suspension.rideHeight = 0.3f;
            mainCars[3].suspension.dampingRate = 0.25f;

            // Upgrades
            mainCars[3].upgrades = new List<UpgradeState>();
            mainCars[3].upgrades.Add(new UpgradeState { type = UpgradeType.Engine, currentTier = UpgradeTier.Stock });
            mainCars[3].upgrades.Add(new UpgradeState { type = UpgradeType.Brakes, currentTier = UpgradeTier.Stock });
            mainCars[3].upgrades.Add(new UpgradeState { type = UpgradeType.Transmission, currentTier = UpgradeTier.Sport });
            mainCars[3].upgrades.Add(new UpgradeState { type = UpgradeType.Driveshaft, currentTier = UpgradeTier.Stock });
            mainCars[3].upgrades.Add(new UpgradeState { type = UpgradeType.Suspension, currentTier = UpgradeTier.Stock });
            mainCars[3].upgrades.Add(new UpgradeState { type = UpgradeType.Tyers, currentTier = UpgradeTier.Stock });
            mainCars[3].upgrades.Add(new UpgradeState { type = UpgradeType.Nitrous, currentTier = UpgradeTier.None });

            // Visual Upgrades
            mainCars[3].visualUpgrades.wheelId = 1;
            mainCars[3].visualUpgrades.bodyHexColor = "#FFFFFF";
            mainCars[3].visualUpgrades.wheelCoords.wheelX = 0.76f;
            mainCars[3].visualUpgrades.wheelCoords.wheelY = 0.6f;
            mainCars[3].visualUpgrades.wheelCoords.wheelFrontZ = 1.76f;
            mainCars[3].visualUpgrades.wheelCoords.wheelRearZ = -0.76f;
        }

        //
        // Dodge
        //
        {
            mainCars[4] = new Car();
            mainCars[4].carIndex = 4;
            mainCars[4].name = "Dodge";
            mainCars[4].owned = false;

            // Transmission
            mainCars[4].transmission.numberOfGears = 7;
            mainCars[4].transmission.gearRatios = new float[] { -4.2f, 3.7f, 2.3f, 1.6f, 1.3f, 1.0f, 0.8f , 0.6f }; //Needs to have the max number of gears, even though they will not be used
            mainCars[4].transmission.finalDrive = 2.7f;
            mainCars[4].transmission.shifTime = 0.09f;
            mainCars[4].transmission.drivetrain = Drivetrain.FourWheelDrive;
            mainCars[4].transmission.type = TransmisionType.Manual;

            // Engine
            mainCars[4].engine.horsepower = 260;
            mainCars[4].engine.idleRPM = 1000;
            mainCars[4].engine.redlineRPM = 6500;
            mainCars[4].engine.maxRPM = 8200;

            // Nitrous
            mainCars[4].nitrous.useNitrous = false;

            // Brakes
            mainCars[4].brakes.brakeTorque = 2300;

            // Tyres
            mainCars[4].tyres.forwardStiffnes = 1.4f;
            mainCars[4].tyres.sidewaysStiffnes = 2.3f;

            // Suspension
            mainCars[4].suspension.maxSteeringAngle = 45;
            mainCars[4].suspension.spring = 37000;
            mainCars[4].suspension.dampen = 4700;
            mainCars[4].suspension.rideHeight = 0.28f;
            mainCars[4].suspension.dampingRate = 0.27f;

            // Upgrades
            mainCars[4].upgrades = new List<UpgradeState>();
            mainCars[4].upgrades.Add(new UpgradeState { type = UpgradeType.Engine, currentTier = UpgradeTier.Stock });
            mainCars[4].upgrades.Add(new UpgradeState { type = UpgradeType.Brakes, currentTier = UpgradeTier.Stock });
            mainCars[4].upgrades.Add(new UpgradeState { type = UpgradeType.Transmission, currentTier = UpgradeTier.Stock });
            mainCars[4].upgrades.Add(new UpgradeState { type = UpgradeType.Driveshaft, currentTier = UpgradeTier.Stock });
            mainCars[4].upgrades.Add(new UpgradeState { type = UpgradeType.Suspension, currentTier = UpgradeTier.Stock });
            mainCars[4].upgrades.Add(new UpgradeState { type = UpgradeType.Tyers, currentTier = UpgradeTier.Stock });
            mainCars[4].upgrades.Add(new UpgradeState { type = UpgradeType.Nitrous, currentTier = UpgradeTier.None });

            // Visual Upgrades
            mainCars[4].visualUpgrades.wheelId = 2;
            mainCars[4].visualUpgrades.bodyHexColor = "#000000";
            mainCars[4].visualUpgrades.wheelCoords.wheelX = 0.85f;
            mainCars[4].visualUpgrades.wheelCoords.wheelY = 0.65f;
            mainCars[4].visualUpgrades.wheelCoords.wheelFrontZ = 1.55f;
            mainCars[4].visualUpgrades.wheelCoords.wheelRearZ = -1.4f;
        }

        //
        // Porsche
        //
        {
            mainCars[5] = new Car();
            mainCars[5].carIndex = 5;
            mainCars[5].name = "Porsche";
            mainCars[5].owned = false;

            // Transmission
            mainCars[5].transmission.numberOfGears = 8;
            mainCars[5].transmission.gearRatios = new float[] { -4.1f, 3.8f, 2.5f, 1.9f, 1.5f, 1.2f, 1.0f, 0.9f }; //Needs to have the max number of gears, even though they will not be used
            mainCars[5].transmission.finalDrive = 2.9f;
            mainCars[5].transmission.shifTime = 0.07f;
            mainCars[5].transmission.drivetrain = Drivetrain.FourWheelDrive;
            mainCars[5].transmission.type = TransmisionType.Manual;

            // Engine
            mainCars[5].engine.horsepower = 320;
            mainCars[5].engine.idleRPM = 1050;
            mainCars[5].engine.redlineRPM = 7000;
            mainCars[5].engine.maxRPM = 8500;

            // Nitrous
            mainCars[5].nitrous.useNitrous = false;

            // Brakes
            mainCars[5].brakes.brakeTorque = 2500;

            // Tyres
            mainCars[5].tyres.forwardStiffnes = 1.6f;
            mainCars[5].tyres.sidewaysStiffnes = 2.5f;

            // Suspension
            mainCars[5].suspension.maxSteeringAngle = 46;
            mainCars[5].suspension.spring = 39000;
            mainCars[5].suspension.dampen = 5000;
            mainCars[5].suspension.rideHeight = 0.26f;
            mainCars[5].suspension.dampingRate = 0.3f;

            // Upgrades
            mainCars[5].upgrades = new List<UpgradeState>();
            mainCars[5].upgrades.Add(new UpgradeState { type = UpgradeType.Engine, currentTier = UpgradeTier.Stock });
            mainCars[5].upgrades.Add(new UpgradeState { type = UpgradeType.Brakes, currentTier = UpgradeTier.Stock });
            mainCars[5].upgrades.Add(new UpgradeState { type = UpgradeType.Transmission, currentTier = UpgradeTier.Stock });
            mainCars[5].upgrades.Add(new UpgradeState { type = UpgradeType.Driveshaft, currentTier = UpgradeTier.Stock });
            mainCars[5].upgrades.Add(new UpgradeState { type = UpgradeType.Suspension, currentTier = UpgradeTier.Stock });
            mainCars[5].upgrades.Add(new UpgradeState { type = UpgradeType.Tyers, currentTier = UpgradeTier.Stock });
            mainCars[5].upgrades.Add(new UpgradeState { type = UpgradeType.Nitrous, currentTier = UpgradeTier.None });

            // Visual Upgrades
            mainCars[5].visualUpgrades.wheelId = 2;
            mainCars[5].visualUpgrades.bodyHexColor = "#00FFFF";
            mainCars[5].visualUpgrades.wheelCoords.wheelX = 0.77f;
            mainCars[5].visualUpgrades.wheelCoords.wheelY = 0.65f;
            mainCars[5].visualUpgrades.wheelCoords.wheelFrontZ = 1.48f;
            mainCars[5].visualUpgrades.wheelCoords.wheelRearZ = -0.92f;
        }


    }

    //TO DO - Add a way to calculate the percentage of completion
    //After doing this, see "SaveSlot" script, line 49
    /*
    public int GetPercentageComplete() 
    {

    }
    */
}
