using System.Collections;
using UnityEngine;

public enum RaceType
{
    Lap,
    PointToPoint
}

public enum GameMode
{
    TimeAttack,
    Sprint
}

public class TrackCheckpoints : MonoBehaviour
{


    //
    //Inspector Variables
    //

    //Race Type
    public RaceType raceType;
    public LapSystem lapSystem;

    /*
    These values ,  besides 'Game Mode' , 
    Are used only to make sure you add the correct scirpt and show NULL if it is wrong
    */
    public GameMode gameMode;
    public TimeAttackManager timeAttackManager;
    public SprintManager sprintManager;

    //Countdown system variables
    public CountdownPanelControl countdownPanelControl;
    public string messageToDisplay = "Missed Checkpoint";
    public int countdownTime = 5;

    //Black screen system variables;
    public CanvasGroup blackPanel;
    public int blackFadeSpeed = 7;

    //Teleport system variables
    public int missedCheckopointDistance = 5;
    public int checkpointTeleportDistance = 10;

    //Checkpoint control system variables
    public int numberOfActiveCheckpoints = 4;

    //Audio Controller
    public RaceAudioManager raceAudioManager;

    //
    //Variables that do not need to be given values
    //

    //Checkpoint count system
    private Transform[] checkpointTransforms;
    public Transform currentCheckpointTransform;
    public int currentCheckpointIndex;
    public int totalCheckpointNumber;
    public float checkpointDot;

    //Black Screen System
    public bool blackFadeStarted;

    //Countdown system
    public bool missedCheckpoint;

    //This is a struct containing all the data that we need from the car
    private struct Car
    {
        public CarEffects effects;
        public Transform transform;
        public Rigidbody rigidbody;
    }
    private Car car;

    public void OnEnable()
    {
        //Setup for the 'Car' struct
        car.effects = GameObject.FindWithTag("Player").GetComponent<CarEffects>();
        car.transform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        car.rigidbody = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();

        //Setup for the countdown system 
        missedCheckpoint = false;

        //Setup for the black screen system
        blackPanel.alpha = 0;

        //Setup for the checkpoint count system
        checkpointTransforms = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            checkpointTransforms[i] = transform.GetChild(i);

        currentCheckpointIndex = 0;
        currentCheckpointTransform = checkpointTransforms[0];
        totalCheckpointNumber = transform.childCount;

        //Setup for checkpoint control system
        //Here we deactivate all the checkpoints, leaving active only the first 'numberOfActiveCheckpoints'
        for (int i = numberOfActiveCheckpoints; i < totalCheckpointNumber; i++)
            checkpointTransforms[i].gameObject.SetActive(false);

    }

    public void Update()
    {
        //We update the current checkpoint transform
        currentCheckpointTransform = checkpointTransforms[currentCheckpointIndex];

        //This dot product checks if the car is in front of the checkpoint
        Vector3 direction = currentCheckpointTransform.TransformDirection(Vector3.forward);
        Vector3 toCar = car.transform.position - currentCheckpointTransform.position;
        checkpointDot = Vector3.Dot(direction, toCar);

        //If the car is in front of the checkpoint, than the player missed the checkpoint
        if (checkpointDot > missedCheckopointDistance && missedCheckpoint == false)
        {
            //If it misses, we start a countdown
            missedCheckpoint = true;
            StartCoroutine(StartCountdown());
        }

        //We fade in/out the paneles when needed
        BlackScreenFade();

    }

    public IEnumerator StartCountdown()
    {
        /*
        Here we :
        -put up the message (for example 'missed checkpoint' )
        -start the panel fade in
        -and play a sound
        */
        countdownPanelControl.shouldFade = true;
        countdownPanelControl.messageText.text = messageToDisplay;

        //Timer logic
        for (int i = countdownTime; i > 0; i--)
        {
            countdownPanelControl.countdownText.text = i.ToString();
            yield return new WaitForSeconds(1);

            if (raceAudioManager.enabled == true)
                raceAudioManager.countdownSource.Play();

            //If the car gets back in time, we stop the countdown
            if (checkpointDot < missedCheckopointDistance)
            {
                missedCheckpoint = false;
                countdownPanelControl.shouldFade = false;
                yield break;
            }

        }

        //If the car does not get back in time, we  disable the countdown panel and teleport the car
        StartCoroutine(TeleportToCheckpoint());
        yield break;
    }

    //This method gets the current checkpoint transform, and teleports the car to it
    public IEnumerator TeleportToCheckpoint()
    {
        //Disable the countdown panel
        countdownPanelControl.shouldFade = false;

        //Enable the black screen
        blackFadeStarted = true;
        yield return new WaitUntil(() => blackPanel.alpha == 1);

        //Freeze the car
        car.rigidbody.constraints = RigidbodyConstraints.FreezePosition;

        //Stop car's trail effects
        car.effects.rearLeftTireSkid.Clear();
        car.effects.rearRightTireSkid.Clear();

        /*
        Each checkpoint single has a child called 'Teleport Position'.
        We use this child to get the position where we want to teleport the player.
        This child is always on the 0 position.
        */
        car.transform.position = currentCheckpointTransform.GetChild(0).GetComponent<Transform>().position;
        car.transform.rotation = currentCheckpointTransform.rotation;

        //We are back on the road
        missedCheckpoint = false;

        //Unfreeze the car
        car.rigidbody.constraints = RigidbodyConstraints.None;

        //Revert the black screen
        blackFadeStarted = false;
    }

    //This method allows all the 'Checkpoint Single' to deactivate and activate the next checkpoint in line


    /*public void ManageCheckpoint(int checkpointIndex)
    {
        //Play a sound
        if (raceAudioManager.gameObject.activeSelf == true)
            raceAudioManager.checkpointTriggerSource.Play();

        //This deactivates the checkpoint the player just went through
        // In case it is the last checkpoint of the lap, we do some special logic
        if (currentCheckpointIndex == 0)
            checkpointTransforms[checkpointTransforms.Length - 1].gameObject.SetActive(false);
        else
            checkpointTransforms[currentCheckpointIndex - 1].gameObject.SetActive(false);

        //This activates the next checkpoint in line
        int nextCheckpoint = (checkpointIndex + numberOfActiveCheckpoints) % totalCheckpointNumber;
        checkpointTransforms[nextCheckpoint].gameObject.SetActive(true);
    }*/

    public void ManageCheckpoint(int checkpointIndex)
    {
        //Play a sound
        if (raceAudioManager.gameObject.activeSelf == true)
            raceAudioManager.checkpointTriggerSource.Play();

        //This deactivates the checkpoint the player just went through
        // In case it is the last checkpoint of the lap, we do some special logic
        checkpointTransforms[currentCheckpointIndex].gameObject.SetActive(false);

        //This activates the next checkpoint in line
        int nextCheckpoint = (checkpointIndex + numberOfActiveCheckpoints ) % totalCheckpointNumber;
        checkpointTransforms[nextCheckpoint].gameObject.SetActive(true);
    }

    //This method fades in/out the black panel depending on the 'blackFadeStarted' bool
    public void BlackScreenFade()
    {
        if (blackFadeStarted == true)
        {
            if (blackPanel.alpha < 1)
                blackPanel.alpha += Time.deltaTime * blackFadeSpeed;
        }
        else
        {
            if (blackPanel.alpha > 0)
                blackPanel.alpha -= Time.deltaTime * blackFadeSpeed;
        }
    }


}
