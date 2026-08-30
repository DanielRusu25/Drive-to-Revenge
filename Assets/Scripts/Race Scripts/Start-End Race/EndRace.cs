using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;


public class EndRace : MonoBehaviour
{
    [Header("Other Scripts")]
    [Space(5)]
    //Used to get information about the game mode
    public TrackCheckpoints trackCheckpoints;
    //Used only to get the control actions that need to be disabled at the end of the race
    public StartRace startRace;

    [Space(10)]
    [Header("UI Stuff")]
    [Space(5)]

    public GameObject endRaceMenu;
    public GameObject inRaceUI;
    public GameObject carUI; 

    [Space(5)]
    private CanvasGroup endRacePanel;
    public TMP_Text raceWonText;
    public TMP_Text informationText;

    [Space(10)]
    [Header("Fade Speeds")]
    [Space(5)]

    public float soundFade = 0.1f;
    public float panelFade = 0.05f;

    //
    //These variables contain information, given from other scripts, about the race
    //

    //If this bool is false, the race was lost and if it is true the race was won;
    [HideInInspector] public bool raceWon;

    //This string holds information about the race, such as the place the player finished
    //or the time that it took to finish the race
    [HideInInspector] public string raceInformation;

    public struct PlayerCar
    {
        public GameObject gameObject;
        public CarAudio carAudio;
        public float initialVolume;
        public PlayerInput playerInput;
    }
    public PlayerCar playerCar;

    //A bool used to know if the finish trigger is hit
    [HideInInspector] public bool raceFinished;

    private void OnEnable()
    {
        //Setup for the struct containing all the variables that are on the player car
        playerCar.gameObject = GameObject.FindWithTag("Player");
        playerCar.carAudio = GameObject.FindWithTag("Player").GetComponent<CarAudio>();
        playerCar.initialVolume = playerCar.carAudio.volume;
        playerCar.playerInput = playerCar.gameObject.GetComponent<PlayerInput>();

        //Disable the end race menu
        endRacePanel = endRaceMenu.GetComponentInChildren<CanvasGroup>();
        endRacePanel.gameObject.SetActive(false);
        endRacePanel.interactable = false;
        endRacePanel.alpha = 0;

        raceFinished = false;
    }

    public void Update()
    {
        if (raceFinished)
        {

            // UI
            inRaceUI.SetActive(false);
            carUI.SetActive(false);
            endRaceMenu.SetActive(true);

            //Audio
            playerCar.carAudio.enabled = false;
            foreach (AudioSource audioSource in playerCar.gameObject.GetComponents<AudioSource>())
                if (audioSource.volume > 0)
                    audioSource.volume -= soundFade * Time.deltaTime;

            //Disable controls
            foreach (string st in startRace.controlActions)
                playerCar.playerInput.actions.FindAction(st).Disable();

            //Stop the car
            Rigidbody playerRigidbody = playerCar.gameObject.GetComponent<Rigidbody>();
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;

            HandleUI();

            //Activate the menu
            StartCoroutine(EndMenu());
        }
    }

    public void OnDisable()
    {
        //Enenable the UI
        inRaceUI.SetActive(false);
        carUI.SetActive(true);
        endRaceMenu.SetActive(false);

        //Enable controls
        foreach (string st in startRace.controlActions)
            playerCar.playerInput.actions.FindAction(st).Enable();

        //Enable the car audio
        playerCar.carAudio.enabled = true;
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player Body")
            raceFinished = true;

    }

    //This enumerator is needed because after the race is done, 
    //the system might need a little time to update the data
    //So we add a buffer before we turn on the menu
    private IEnumerator EndMenu()
    {
        endRacePanel.gameObject.SetActive(true);
        endRacePanel.interactable = true;
        yield return new WaitForSeconds(1f);
        if (endRacePanel.alpha < 1)
            endRacePanel.alpha += panelFade * Time.deltaTime;
    }

    private void HandleUI()
    {
        //Race Won/Lost
        if (raceWon == true)
        {
            raceWonText.text = "You Won!";
            raceWonText.color = Color.green;
        }
        else
        {
            raceWonText.text = "You Lost!";
            raceWonText.color = Color.red;
        }

        //Information
        if (trackCheckpoints.gameMode == GameMode.TimeAttack)
            informationText.text = "Completed in: " + raceInformation;
        else if (trackCheckpoints.gameMode == GameMode.Sprint)
            informationText.text = "Finished: " + raceInformation;
    }
}
