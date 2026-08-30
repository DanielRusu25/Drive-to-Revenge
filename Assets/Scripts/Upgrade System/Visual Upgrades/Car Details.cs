using TMPro;
using UnityEngine;

public class CarDetails : MonoBehaviour
{
    [Header("TMP Text Components")]
    public TMP_Text nameText;
    public TMP_Text horsepowerText;
    public TMP_Text brakeToqueText;
    public TMP_Text tyersText;
    public TMP_Text suspensionText;
    public TMP_Text nrOfGearsText;
    public TMP_Text drivetrainText;
    public TMP_Text monetyText;

    [Header("Data Objects")]
    public CurrentCarManager currentCarManager;
    public CarSwapper carSwapper;

    private int currentCarIndex;

    public void Update()
    {
        currentCarIndex = carSwapper.currentShowCarIndex; // +1 because the first car is always the main car


        nameText.text = currentCarManager.mainCars[currentCarIndex].name;
        horsepowerText.text = "Horsepower: " + currentCarManager.mainCars[currentCarIndex].engine.horsepower.ToString();
        brakeToqueText.text = "Brake Torque: " + currentCarManager.mainCars[currentCarIndex].brakes.brakeTorque.ToString();

        //Tier Upgrade type
        var tyerFound = currentCarManager.mainCars[currentCarIndex].upgrades.Find(t => t.type == UpgradeType.Tyers);
        tyersText.text = "Tyres: " + tyerFound.currentTier.ToString();

        //Suspension Upgrade type
        var suspensionFound = currentCarManager.mainCars[currentCarIndex].upgrades.Find(t => t.type == UpgradeType.Suspension);
        suspensionText.text = "Suspension: " + suspensionFound.currentTier.ToString();

        nrOfGearsText.text = "Number of Gears: " + currentCarManager.mainCars[currentCarIndex].transmission.numberOfGears.ToString();

        //Drivetrain Upgrade type
        switch (currentCarManager.mainCars[currentCarIndex].transmission.drivetrain)
        {
            case Drivetrain.FrontWheelDrive:
                drivetrainText.text = "Drivetrain: FWD";
                break;
            case Drivetrain.RearWheelDrive:
                drivetrainText.text = "Drivetrain: RWD";
                break;
            case Drivetrain.FourWheelDrive:
                drivetrainText.text = "Drivetrain: AWD";
                break;
        }

        monetyText.text = "Money: " + currentCarManager.currentMoney.ToString();
    }
}
