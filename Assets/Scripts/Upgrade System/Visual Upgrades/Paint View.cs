using UnityEngine;
using UnityEngine.UI;

public class PaintView : MonoBehaviour
{

    public string paintHexColor;
    public Image paintImage;

    [HideInInspector] public PaintManager paintManager;
    [HideInInspector] public CarSwapper carSwapper;

    private void OnEnable()
    {
        paintImage = transform.GetChild(0).GetComponent<Image>();
        if (paintImage != null)
        {
            Color newColor;
            ColorUtility.TryParseHtmlString(paintHexColor, out newColor);
            paintImage.color = newColor;
        }
    }

    public void OnViewClick()
    {
        if (paintManager != null && carSwapper != null)
        {
            paintManager.currentPaintHexColor = paintHexColor;  
            paintManager.HandleUi(paintHexColor);
            carSwapper.ChangeColor(carSwapper.phisicalCarComponents.bodyMeshRenderer, paintHexColor);
        }
    }
}
