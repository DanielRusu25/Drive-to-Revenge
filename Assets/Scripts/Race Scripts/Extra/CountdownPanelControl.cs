using UnityEngine;
using TMPro;

public class CountdownPanelControl : MonoBehaviour
{
    public CanvasGroup countdownPanel;
    public int fadeSpeed = 5;
    public TMP_Text messageText;
    public TMP_Text countdownText;

    [HideInInspector] public bool shouldFade;

    private void OnEnable()
    {
        if (countdownPanel == null)
            countdownPanel = GetComponent<CanvasGroup>();
        countdownPanel.alpha = 0;
    }

    private void Update()
    {
        CountdownPanelFade();
    }


    //This method fades in/out the countdown panel depending on the 'countdownStarted' bool
    public void CountdownPanelFade()
    {
        if (shouldFade == true)
        {
            if (countdownPanel.alpha < 1)
                countdownPanel.alpha += Time.deltaTime * fadeSpeed;
        }
        else
        {
            if (countdownPanel.alpha > 0)
                countdownPanel.alpha -= Time.deltaTime * fadeSpeed;
        }
    }
}
