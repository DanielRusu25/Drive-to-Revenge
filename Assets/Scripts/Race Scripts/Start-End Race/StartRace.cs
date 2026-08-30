using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class StartRace : MonoBehaviour
{

    //Display
    public CountdownPanelControl countdownPanelControl;
    public string messageToDisplay = "Starting in:";

    //UI
    public GameObject[] thingsToEnable;
    public GameObject[] thingsToDisable;

    //Lights
    //They must be put in reverse order of them turning on
    public Light[] lights;

    //Player Input variables
    public string[] controlActions;
    private PlayerInput playerInput;

    //Other scripts
    public TrackCheckpoints trackCheckpoints;
    public RaceAudioManager raceAudioManager;

    //In case the race is 'Sprint', we need some extrea stuff
    public GameObject aiCarsContainer;
    private List<CarController> aiCarControllers = new List<CarController>();

    public void OnEnable()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();

        if (thingsToEnable.Length > 0)
            foreach (GameObject go in thingsToEnable)
                go.SetActive(true);

        if (thingsToDisable.Length > 0)
            foreach (GameObject go in thingsToDisable)
                go.SetActive(false);

        foreach (Light li in lights)
            li.gameObject.SetActive(false);

        if (trackCheckpoints.gameMode == GameMode.Sprint)
            foreach (CarController carController in aiCarsContainer.GetComponentsInChildren<CarController>())
            {
                aiCarControllers.Add(carController);
                carController.enabled = false;
            }

        countdownPanelControl.shouldFade = true;
        foreach (string st in controlActions)
            playerInput.actions.FindAction(st).Disable();


        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        countdownPanelControl.messageText.text = messageToDisplay;
        yield return new WaitUntil(() => countdownPanelControl.countdownPanel.alpha == 1);


        for (int i = 3; i > 0; i--)
        {
            countdownPanelControl.countdownText.text = i.ToString();
            lights[i - 1].gameObject.SetActive(true);

            if (raceAudioManager.enabled == true)
            {
                switch (i)
                {
                    case 3:
                        raceAudioManager.startSource.Play();
                        break;

                    case 2:
                        raceAudioManager.middleSource.Play();
                        break;

                    case 1:
                        raceAudioManager.finishSource.Play();
                        break;
                }
            }
            yield return new WaitForSeconds(1);

            lights[(i - 1)].gameObject.SetActive(false);

        }

        foreach (string st in controlActions)
            playerInput.actions.FindAction(st).Enable();
        countdownPanelControl.shouldFade = false;

        Rigidbody playerRigidbody = playerInput.gameObject.GetComponent<Rigidbody>();
        playerRigidbody.constraints = RigidbodyConstraints.None;

        if (trackCheckpoints.gameMode == GameMode.TimeAttack)
            StartCoroutine(trackCheckpoints.timeAttackManager.Timer());
        else if (trackCheckpoints.gameMode == GameMode.Sprint)
            foreach (CarController carController in aiCarControllers)
                carController.enabled = true;

        yield break;
    }



}
