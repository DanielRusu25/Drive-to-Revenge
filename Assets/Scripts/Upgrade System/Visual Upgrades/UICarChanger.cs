using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICarChanger : MonoBehaviour
{
    [Header("Logic Objects")]
    public CurrentCarManager currentCarManager;
    public CarSwapper carSwapper;

    [Space(10)]
    [Header("UiObjects")]
    public List<int> prices;
    public TMP_Text infoText;
    public TMP_Text priceText;
    public Image buttonBackround;


    public void OnEnable()
    {
        carSwapper.currentShowCarIndex = currentCarManager.mainCars[0].carIndex;
        carSwapper.ChangeCar(carSwapper.currentShowCarIndex);
        HandleUi();
        carSwapper.LoadVisualContainers();
    }

    public void OnDisable()
    {
        carSwapper.currentShowCarIndex = currentCarManager.mainCars[0].carIndex;
        carSwapper.ChangeCar(carSwapper.currentShowCarIndex);
        HandleUi();
    }

    public void OnRightClick()
    {
        if (carSwapper.currentShowCarIndex < currentCarManager.numberOfCars)
        {
            //Save the old car data
            currentCarManager.mainCars[currentCarManager.currentCarIndex] = currentCarManager.mainCars[0];
            carSwapper.LoadVisualContainers();


            carSwapper.currentShowCarIndex++;
            carSwapper.ChangeCar(carSwapper.currentShowCarIndex);
        }
    }

    public void OnLeftClick()
    {
        if (carSwapper.currentShowCarIndex > 1)
        {
            //Save the old car data
            currentCarManager.mainCars[currentCarManager.currentCarIndex] = currentCarManager.mainCars[0];
            carSwapper.LoadVisualContainers();

            carSwapper.currentShowCarIndex--;
            carSwapper.ChangeCar(carSwapper.currentShowCarIndex);
        }
    }

    public void OnSelectClick()
    {
        //int currentShowCarIndex = carSwapper.currentShowCarIndex;
        if (currentCarManager.mainCars[carSwapper.currentShowCarIndex].owned)
        {
            if (currentCarManager.mainCars[0].carIndex != carSwapper.currentShowCarIndex)
            {
                currentCarManager.currentCarIndex = carSwapper.currentShowCarIndex;
                currentCarManager.mainCars[0] = currentCarManager.mainCars[carSwapper.currentShowCarIndex];
                HandleUi();
                carSwapper.LoadVisualContainers();

            }
        }
        else
        {
            if (currentCarManager.currentMoney >= prices[carSwapper.currentShowCarIndex - 1])
            {
                currentCarManager.currentMoney -= prices[carSwapper.currentShowCarIndex - 1];
                currentCarManager.mainCars[carSwapper.currentShowCarIndex].owned = true;
                currentCarManager.currentCarIndex = carSwapper.currentShowCarIndex;
                HandleUi();
                carSwapper.LoadVisualContainers();
                OnSelectClick();
            }
        }

    }

    public void HandleUi()
    {
        if (currentCarManager.mainCars[carSwapper.currentShowCarIndex].owned)
        {
            if (currentCarManager.mainCars[0].carIndex == carSwapper.currentShowCarIndex)
            {
                //Switch to a light blue color
                Color newColor;
                ColorUtility.TryParseHtmlString("#12CBC4", out newColor);
                buttonBackround.color = newColor;

                priceText.text = "";
                infoText.text = "Selected";
            }
            else
            {
                //Switch to a dark blue color
                Color newColor;
                ColorUtility.TryParseHtmlString("#0652DD", out newColor);
                buttonBackround.color = newColor;

                priceText.text = "";
                infoText.text = "Select";
            }
        }
        else
        {
            if (currentCarManager.currentMoney >= prices[carSwapper.currentShowCarIndex - 1])
            {
                //Switch to a green color
                Color newColor;
                ColorUtility.TryParseHtmlString("#00D832", out newColor);
                buttonBackround.color = newColor;

                priceText.text = prices[carSwapper.currentShowCarIndex - 1] + "$";
                infoText.text = "Buy";
            }
            else
            {
                //Switch to a red color
                Color newColor;
                ColorUtility.TryParseHtmlString("#EA2027", out newColor);
                buttonBackround.color = newColor;

                priceText.text = prices[carSwapper.currentShowCarIndex - 1] + "$";
                infoText.text = "Not enough";
            }
        }
    }
}
