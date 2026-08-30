using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelManager : MonoBehaviour
{
    public CarSwapper carSwapper;
    public CurrentCarManager currentCarManager;
    public List<int> wheelprices;

    public Image buyButtonBackground;
    public TMP_Text infoText;
    public TMP_Text priceText;

    private int currentWheelIndex;

    private void OnEnable()
    {
        currentWheelIndex = currentCarManager.mainCars[0].visualUpgrades.wheelId;
        HandleUi(currentWheelIndex);
    }

    private void OnDisable()
    {
        currentWheelIndex = currentCarManager.mainCars[0].visualUpgrades.wheelId;
        // Change the wheels to the selected ones
        for (int i = 0; i < carSwapper.phisicalCarComponents.wheels.Count; i++)
        {
            if (carSwapper.phisicalCarComponents.wheels[i].rightSide)
                ChangeMesh(carSwapper.phisicalCarComponents.wheels[i].wheelMeshFilter, carSwapper.wheelContainer[currentWheelIndex].rightWheel);
            else
                ChangeMesh(carSwapper.phisicalCarComponents.wheels[i].wheelMeshFilter, carSwapper.wheelContainer[currentWheelIndex].leftWheel);
        }
    }

    public void BuyWheel()
    {
        if (currentCarManager.currentMoney >= wheelprices[currentWheelIndex])
        {
            currentCarManager.mainCars[0].visualUpgrades.wheelId = currentWheelIndex;
            currentCarManager.currentMoney -= wheelprices[currentWheelIndex];
            HandleUi(currentWheelIndex);
        }
    }

    public void OnViewWheelClick(int wheelIndex)
    {
        HandleUi(wheelIndex);
        for (int i = 0; i < carSwapper.phisicalCarComponents.wheels.Count; i++)
        {
            if (carSwapper.phisicalCarComponents.wheels[i].rightSide)
                ChangeMesh(carSwapper.phisicalCarComponents.wheels[i].wheelMeshFilter, carSwapper.wheelContainer[wheelIndex].rightWheel);
            else
                ChangeMesh(carSwapper.phisicalCarComponents.wheels[i].wheelMeshFilter, carSwapper.wheelContainer[wheelIndex].leftWheel);
        }
        currentWheelIndex = wheelIndex;
    }

    public void HandleUi(int wheelIndex)
    {
        if (currentCarManager.mainCars[0].visualUpgrades.wheelId == wheelIndex)
        {
            //Switch to a light blue color
            Color newColor;
            ColorUtility.TryParseHtmlString("#12CBC4", out newColor);
            buyButtonBackground.color = newColor;

            priceText.text = "";
            infoText.text = "Selected";
        }

        else
        {
            if (currentCarManager.currentMoney >= wheelprices[wheelIndex])
            {
                //Switch to a green color
                Color newColor;
                ColorUtility.TryParseHtmlString("#00D832", out newColor);
                buyButtonBackground.color = newColor;

                priceText.text = wheelprices[wheelIndex] + " $";
                infoText.text = "Buy";
            }
            else
            {
                //Switch to a red color
                Color newColor;
                ColorUtility.TryParseHtmlString("#EA2027", out newColor);
                buyButtonBackground.color = newColor;

                priceText.text = wheelprices[wheelIndex] + " $";
                infoText.text = "Not enough";
            }

            priceText.text = wheelprices[wheelIndex].ToString();

        }
    }

    public void ChangeMesh(MeshFilter meshFilterOfObject, Mesh newMesh)
    {
        meshFilterOfObject.mesh = newMesh;
    }

}
