using System.Collections;
using UnityEngine;

public class GarageManager : MonoBehaviour
{
    public GameObject garageEnterMenu;
    private CanvasGroup garageEnterMenuPanel;
    private bool shouldFade;
    public float fadeSpeed = 2f;

    public void Start()
    {
        foreach (GarageEnter garageEnter in GetComponentsInChildren<GarageEnter>())
        {
            garageEnter.garageManager = this;
        }

        garageEnterMenu.SetActive(false);
        garageEnterMenuPanel = garageEnterMenu.GetComponentInChildren<CanvasGroup>();
        garageEnterMenuPanel.alpha = 0f;
        shouldFade = false;
    }

    public void Update()
    {
        FadeMenu();
    }

    public IEnumerator ShowGarageEnterMenu()
    {
        shouldFade = true;
        garageEnterMenu.SetActive(true);
        yield return new WaitUntil(() => garageEnterMenuPanel.alpha >= 1f);
    } 


    public IEnumerator HideGarageEnterMenu()
    {
        shouldFade = false;
        yield return new WaitUntil(() => garageEnterMenuPanel.alpha <= 0f);
        garageEnterMenu.SetActive(false);
    }

    
    public void FadeMenu()
    {
        if (shouldFade)
        {
            if (garageEnterMenuPanel.alpha < 1)
                garageEnterMenuPanel.alpha += Time.deltaTime * fadeSpeed;
        }
        else
        {
            if (garageEnterMenuPanel.alpha > 0)
                garageEnterMenuPanel.alpha -= Time.deltaTime * fadeSpeed;
        }
    }
}
