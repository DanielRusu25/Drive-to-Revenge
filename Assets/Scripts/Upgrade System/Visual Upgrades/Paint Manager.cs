using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PaintManager : MonoBehaviour
{
    public CarSwapper carSwapper;
    public CurrentCarManager currentCarManager;
    public int paintPrice;

    public Image buyButtonBackground;
    public TMP_Text infoText;
    public TMP_Text priceText;

    public string currentPaintHexColor;

    private void OnEnable()
    {
        foreach (var paintView in GetComponentsInChildren<PaintView>())
        {
            paintView.paintManager = this;
            paintView.carSwapper = carSwapper;
        }


        currentPaintHexColor = currentCarManager.mainCars[0].visualUpgrades.bodyHexColor;
        HandleUi(currentPaintHexColor);
    }

    public void OnDisable()
    {
        currentPaintHexColor = currentCarManager.mainCars[0].visualUpgrades.bodyHexColor;
        carSwapper.ChangeColor(carSwapper.phisicalCarComponents.bodyMeshRenderer, currentPaintHexColor); ;
    }

    public void BuyPaint()
    {
        if (currentCarManager.currentMoney >= paintPrice)
        {
            currentCarManager.mainCars[0].visualUpgrades.bodyHexColor = currentPaintHexColor;
            currentCarManager.currentMoney -= paintPrice;
            HandleUi(currentPaintHexColor);
        }
    }

    public void HandleUi(string paintHexColor)
    {
        if (currentCarManager.mainCars[0].visualUpgrades.bodyHexColor == paintHexColor)
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
            if (currentCarManager.currentMoney >= paintPrice)
            {
                //Switch to a green color
                Color newColor;
                ColorUtility.TryParseHtmlString("#00D832", out newColor);
                buyButtonBackground.color = newColor;

                priceText.text = paintPrice + " $";
                infoText.text = "Buy";
            }
            else
            {
                //Switch to a red color
                Color newColor;
                ColorUtility.TryParseHtmlString("#FF0000", out newColor);
                buyButtonBackground.color = newColor;

                priceText.text = paintPrice + " $";
                infoText.text = "Not enough";
            }
        }
    }
}
