using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using System.Linq;

public class UpgradeMenu : MonoBehaviour
{
    public CurrentCarManager currentCarManager;

    public UpgradeType upgradeType;
    public UpgradeTier currentTier;

    [Serializable]
    public class Upgrades
    {
        public bool purchesed;
        public UpgradeTier tier;
        public List<float> value;  //This must have the same number and in the same order as how they are inside the 'Car' Struct
        public int cost;
        public string dataText;

        [Space(20)]
        public bool showTier;
        public bool showValue;
        [Serializable]
        public struct UiTMP
        {
            public TMP_Text priceTMP;
            public TMP_Text dataTMP;
            public TMP_Text nameTMP;
        }
        public UiTMP uiTMP;



    }

    public Upgrades[] upgrades;
    private List<float> dataToModify = new List<float>();
    private int currentMoney;

    public void SaveUpgrade()
    {
        var index = currentCarManager.mainCars[0].upgrades.FindIndex(t => t.type == upgradeType);
        UpgradeState upgradeState = new UpgradeState
        {
            type = upgradeType,
            currentTier = this.currentTier
        };

        if (index >= 0)
            currentCarManager.mainCars[0].upgrades[index] = upgradeState;
        else
            currentCarManager.mainCars[0].upgrades.Add(upgradeState);


        currentCarManager.currentMoney = this.currentMoney;
    }

    public void LoadUpgrade()
    {
        var found = currentCarManager.mainCars[0].upgrades.Find(t => t.type == upgradeType);
        currentTier = found.currentTier;

        var index = Array.FindIndex(upgrades, t => t.tier == currentTier);

        if (index >= 0)
            upgrades[index].purchesed = true;

        this.currentMoney = currentCarManager.currentMoney;
    }

    //This needs to be called to set up the different values on the car
    public void ApplyUpgradeds(GameObject button)
    {
        LoadUpgrade();

        FindData();

        int index = button.transform.GetSiblingIndex();
        if (index != (int)UpgradeTier.None)
            if (currentMoney >= upgrades[index].cost)
            {
                for (int i = 0; i < dataToModify.Count; i++)
                    dataToModify[i] = upgrades[index].value[i];
                currentMoney -= upgrades[index].cost;

                currentTier = upgrades[index].tier;

                for (int i = 0; i < upgrades.Count(); i++)
                    upgrades[i].purchesed = false;

                upgrades[index].purchesed = true;
            }



        ApplyData();

        SaveUpgrade();
    }

    public void FindData()
    {
        dataToModify.Clear();

        switch (upgradeType)
        {
            case UpgradeType.Engine:
                dataToModify.Add(currentCarManager.mainCars[0].engine.horsepower);
                break;

            case UpgradeType.Brakes:
                dataToModify.Add(currentCarManager.mainCars[0].brakes.brakeTorque);
                break;

            case UpgradeType.Transmission:
                dataToModify.Add(currentCarManager.mainCars[0].transmission.numberOfGears);
                break;

            case UpgradeType.Driveshaft:
                dataToModify.Add(currentCarManager.mainCars[0].transmission.shifTime);
                break;

            case UpgradeType.Drivetrain:
                dataToModify.Add((int)currentCarManager.mainCars[0].transmission.drivetrain);
                break;

            case UpgradeType.Suspension:
                dataToModify.Add(currentCarManager.mainCars[0].suspension.maxSteeringAngle);
                break;

            case UpgradeType.Tyers:
                dataToModify.Add(currentCarManager.mainCars[0].tyres.forwardStiffnes);
                dataToModify.Add(currentCarManager.mainCars[0].tyres.sidewaysStiffnes);
                break;

            case UpgradeType.Nitrous:
                dataToModify.Add(currentCarManager.mainCars[0].nitrous.useNitrous ? 1 : 0);
                dataToModify.Add(currentCarManager.mainCars[0].nitrous.nitrousPower);
                dataToModify.Add(currentCarManager.mainCars[0].nitrous.nitrousCapacity);
                dataToModify.Add(currentCarManager.mainCars[0].nitrous.nitrousUseSpeed);
                dataToModify.Add(currentCarManager.mainCars[0].nitrous.nitrousRechargeSpeed);
                break;

        }
    }

    public void ApplyData()
    {
        switch (upgradeType)
        {
            case UpgradeType.Engine:
                currentCarManager.mainCars[0].engine.horsepower = (int)dataToModify[0];
                break;

            case UpgradeType.Brakes:
                currentCarManager.mainCars[0].brakes.brakeTorque = (int)dataToModify[0];
                break;

            case UpgradeType.Transmission:
                currentCarManager.mainCars[0].transmission.numberOfGears = (int)dataToModify[0];
                break;

            case UpgradeType.Driveshaft:
                currentCarManager.mainCars[0].transmission.shifTime = dataToModify[0];
                break;

            case UpgradeType.Drivetrain:
                currentCarManager.mainCars[0].transmission.drivetrain = (Drivetrain)(int)dataToModify[0];
                break;

            case UpgradeType.Suspension:
                currentCarManager.mainCars[0].suspension.maxSteeringAngle = (int)dataToModify[0];
                break;

            case UpgradeType.Tyers:
                currentCarManager.mainCars[0].tyres.forwardStiffnes = dataToModify[0];
                currentCarManager.mainCars[0].tyres.sidewaysStiffnes = dataToModify[1];
                break;

            case UpgradeType.Nitrous:
                currentCarManager.mainCars[0].nitrous.useNitrous = dataToModify[0] != 0;
                currentCarManager.mainCars[0].nitrous.nitrousPower = dataToModify[1];
                currentCarManager.mainCars[0].nitrous.nitrousCapacity = dataToModify[2];
                currentCarManager.mainCars[0].nitrous.nitrousUseSpeed = (int)dataToModify[3];
                currentCarManager.mainCars[0].nitrous.nitrousRechargeSpeed = (int)dataToModify[4];
                break;
        }
    }


    public void HandleUi()
    {

        LoadUpgrade();

        foreach (Upgrades upg in upgrades)
        {
            if (upg.showValue)
                if (upgradeType == UpgradeType.Drivetrain)
                    upg.uiTMP.dataTMP.text = (Drivetrain)upg.value[0] + "";
                else
                    upg.uiTMP.dataTMP.text = upg.value[0] + " " + upg.dataText;
            else
                upg.uiTMP.dataTMP.text = "";

            if (upg.showTier)
                upg.uiTMP.nameTMP.text = upg.tier + " " + upgradeType;
            else
                upg.uiTMP.nameTMP.text = upgradeType + "";


            if (upg.purchesed && (int)currentTier >= 0)
                upg.uiTMP.priceTMP.text = "Owned";
            else
                upg.uiTMP.priceTMP.text = upg.cost + " $";
        }
    }
}
