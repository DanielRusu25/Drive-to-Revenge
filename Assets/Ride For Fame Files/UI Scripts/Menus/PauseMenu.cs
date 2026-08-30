using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [Header("Enable/Disable")]
    public GameObject pauseMenu;
    public GameObject[] thingsThatNeedToBeDisabled;
    private bool state;

    [Header("Input system")]
    public PlayerInput playerInput;
    public string uiNavigationActionMap = "UI Navigation";
    public string playerActionMap = "Player";
    public EventSystem eventSystem;
    public GameObject firtsSelected;

    //private DisplayCurrencies displayCurrencies;

    void Start()
    {
        pauseMenu.SetActive(false);
        //displayCurrencies = GetComponent<DisplayCurrencies>();

        state = false;
    }

    public void PauseToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (state == false)
                state = true;
            else
                state = false;

            //Change the action map
            if (state)
                playerInput.SwitchCurrentActionMap(uiNavigationActionMap);
            else
                playerInput.SwitchCurrentActionMap(playerActionMap);

            //Adust the UI and sounds
            AudioListener.pause = state;
            foreach (GameObject gameObject in thingsThatNeedToBeDisabled)
                gameObject.SetActive(!state);
            pauseMenu.SetActive(state);
            eventSystem.firstSelectedGameObject = firtsSelected;

            //Pause the game
            if (state)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;

            //displayCurrencies.Displaying();
        }
    }

}
