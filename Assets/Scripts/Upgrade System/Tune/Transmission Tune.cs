using System;
using TMPro;
using UnityEngine;

public class TransmissionTune : MonoBehaviour
{
    public CurrentCarManager currentCarManager;
    [Serializable]
    public class Buttons
    {
        public TMP_Text selectedText;
        public TransmisionType transmisionType;
    }

    public Buttons[] buttons;

    public void HandleUi()
    {
        for (int i = 0; i < buttons.Length; i++)
            if (currentCarManager.mainCars[0].transmission.type == buttons[i].transmisionType)
                buttons[i].selectedText.text = "Selected";
            else
                buttons[i].selectedText.text = "";
    }

    public void TransmissionSwap(int index)
    {
        currentCarManager.mainCars[0].transmission.type = (TransmisionType)index;
        HandleUi();
    }

}
