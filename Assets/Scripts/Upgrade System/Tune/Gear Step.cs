using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GearStep : MonoBehaviour
{
    public int gear;

    public CurrentCarManager currentCarManager;

    public TMP_Text uiText;
    private Slider slider;
    public float step = 100;

    public int minValue = 0;
    public int maxValue = 5;

    [HideInInspector] public float snappedValue;

    void OnEnable()
    {
        slider = GetComponent<Slider>();

        slider.minValue = minValue;
        slider.maxValue = maxValue;

        FindData();
        uiText.text = slider.value + "";

        if (slider != null)
            slider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        snappedValue = Mathf.Round(value / step) * step;
        if (Mathf.Abs(slider.value - snappedValue) > Mathf.Epsilon)
        {
            slider.SetValueWithoutNotify(snappedValue); // prevents infinite loop
        }

        uiText.text = snappedValue + "";
        ApplyData();
    }


    public void FindData()
    {
        if (gear == 0)
            slider.value = currentCarManager.mainCars[0].transmission.finalDrive;
        else
            slider.value = currentCarManager.mainCars[0].transmission.gearRatios[gear];
    }

    public void ApplyData()
    {
        if (gear == 0)
            currentCarManager.mainCars[0].transmission.finalDrive = snappedValue;
        else
            currentCarManager.mainCars[0].transmission.gearRatios[gear] = snappedValue;
    }

}
