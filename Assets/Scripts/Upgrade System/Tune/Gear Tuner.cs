using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GearTuner : MonoBehaviour
{
    [Serializable]
    public class GearSliders
    {
        public GameObject gearContainer;
        public Slider slider;
        public TMP_Text gearText;
    }

    public CurrentCarManager currentCarManager;
    public GameObject gearStepContainer;
    public List<GearSliders> gearSliders;

    private int nrOfGears;


    public void OnEnable()
    {
        int i = 0;
        foreach (GearStep gearStep in gearStepContainer.GetComponentsInChildren<GearStep>())
        {
            gearStep.gear = i;
            i++;
        }

        nrOfGears = currentCarManager.mainCars[0].transmission.numberOfGears;
        HandleUi();

    }

    public void HandleUi()
    {
        foreach (GearSliders gear in gearSliders)
            gear.gearContainer.SetActive(false);


        gearSliders[0].gearContainer.SetActive(true);
        gearSliders[0].gearText.text = "Final Drive";

        for (int i = 1; i <= nrOfGears; i++)
        {
            gearSliders[i].gearContainer.SetActive(true);
            gearSliders[i].gearText.text = "Gear No. " + i;
        }
    }

}
