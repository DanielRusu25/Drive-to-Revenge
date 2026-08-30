using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum SpeedUnit
{
    KPH,
    MPH,
}

public class CarDisplay : MonoBehaviour
{
    [Header("Rpm Gauge")]
    [Space(5)]

    public TMP_Text gearText;
    public TMP_Text speedText;
    public TMP_Text unitText;

    [Space(5)]

    public Transform rpmNeedle;
    public float maxNeedleRotation;
    public float minNeedleRotation;
    public SpeedUnit speedUnit;

    [Space(10)]
    [Header("Nitrous Slider")]
    [Space(5)]
    public Slider nitrousSlider;

    private CarController carController;

    public void Start()
    {
        carController = GetComponent<CarController>();
        unitText.text = speedUnit.ToString();

        if (carController.useNitrous == true)
        {
            nitrousSlider.gameObject.SetActive(true);
            nitrousSlider.minValue = 0;
            nitrousSlider.maxValue = carController.nitrousCapacity;
        }
        else
        {
            nitrousSlider.gameObject.SetActive(false);
        }
    }

    public void Update()
    {
        rpmNeedle.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(minNeedleRotation, maxNeedleRotation, carController.currentEngineRPM / (carController.redlineRPM * 1.1f)));
        gearText.text = (carController.currentGear == 0) ? "R" : carController.currentGear.ToString();
        nitrousSlider.value = carController.currentNitrousCapacity;

        if (speedUnit == SpeedUnit.KPH)
            speedText.text = carController.carSpeed.ToString();
        else
            speedText.text = (carController.carSpeed * 0.621371).ToString();
    }
}
