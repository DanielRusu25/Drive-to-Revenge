using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum TuneSliderType
{
    MaxSteerAngle,
    RideHeight,
    SuspensionSpring,
    SuspensionDamper,
}

public class SliderUpgrades : MonoBehaviour
{
    [Header("Values")]
    public int cost;
    public float step;

    [Header("Tune Data")]
    public CurrentCarManager currentCarManager;
    public TuneSliderType tuneSliderType;

    [Header("Ui Elements")]
    public Slider slider;
    public TMP_Text costText;
    public TMP_Text uiText;

    private UpgradeTier upgradeTier;
    private float dataToModify;


    //TO DO - add the Upgrade type
    [Serializable]
    public class ConstrainValues
    {
        public UpgradeTier upgradeTier;
        public float valMin;
        public float valMax;
    }
    public List<ConstrainValues> constrainValues;

    public void OnEnable()
    {
        int typeIndex = currentCarManager.mainCars[0].upgrades.FindIndex(t => t.type == UpgradeType.Suspension);
        upgradeTier = currentCarManager.mainCars[0].upgrades[typeIndex].currentTier;

        int constrainIndex = constrainValues.FindIndex(t => t.upgradeTier == upgradeTier);
        slider.minValue = constrainValues[constrainIndex].valMin;
        slider.maxValue = constrainValues[constrainIndex].valMax;

        FindData();
        slider.value = dataToModify;
        HandleUi();
        if (slider != null)
        {
            //slider.wholeNumbers = true;
            slider.onValueChanged.AddListener(OnSliderChanged);

            uiText.text = slider.value + "";
        }
    }

    public void HandleUi()
    {
        if (slider.value == dataToModify)
            costText.text = "Selected";
        else
            costText.text = cost + "$";
    }

    public void Purchase()
    {
        if (currentCarManager.currentMoney >= cost)
        {
            currentCarManager.currentMoney -= cost;
            dataToModify = slider.value;
            ApplyData();
            HandleUi();
        }
    }

    private void OnSliderChanged(float value)
    {
        float snappedValue = Mathf.Round(value / step) * step;
        if (Mathf.Abs(slider.value - snappedValue) > Mathf.Epsilon)
        {
            slider.SetValueWithoutNotify(snappedValue); // prevents infinite loop
        }

        uiText.text = snappedValue + "";
        HandleUi();
    }

    public void FindData()
    {
        switch (tuneSliderType)
        {
            case TuneSliderType.MaxSteerAngle:
                dataToModify = currentCarManager.mainCars[0].suspension.maxSteeringAngle;
                break;

            case TuneSliderType.RideHeight:
                dataToModify = currentCarManager.mainCars[0].suspension.rideHeight;
                break;

            case TuneSliderType.SuspensionDamper:
                dataToModify = currentCarManager.mainCars[0].suspension.dampen;
                break;

            case TuneSliderType.SuspensionSpring:
                dataToModify = currentCarManager.mainCars[0].suspension.spring;
                break;
        }
    }

    public void ApplyData()
    {
        switch (tuneSliderType)
        {
            case TuneSliderType.MaxSteerAngle:
                currentCarManager.mainCars[0].suspension.maxSteeringAngle = (int)dataToModify;
                break;

            case TuneSliderType.RideHeight:
                currentCarManager.mainCars[0].suspension.rideHeight = dataToModify;
                break;

            case TuneSliderType.SuspensionDamper:
                currentCarManager.mainCars[0].suspension.dampen = (int)dataToModify;
                break;

            case TuneSliderType.SuspensionSpring:
                currentCarManager.mainCars[0].suspension.spring = (int)dataToModify;
                break;
        }
    }


}
