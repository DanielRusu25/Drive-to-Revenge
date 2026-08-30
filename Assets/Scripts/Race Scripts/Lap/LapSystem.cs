using UnityEngine;
using TMPro;

public class LapSystem : MonoBehaviour
{
    [Header("Other Scripts")]
    [Space(5)]
    public TrackCheckpoints trackCheckpoints;

    [Space(10)]
    [Header("Lap UI")]
    [Space(5)]
    public TMP_Text lapText;
    public TMP_Text counterText;
    public string lapMessage = "Laps:";

    [Space(10)]
    [Header("Controls")]
    [Space(5)]
    public EndRace endRace;
    public int numberOfLaps = 3;

    //Private variables
    /*
    This game object has 2 triggers as children,
    index 0 at the begginig of the lap
    index 1 at the middle of the lap
    when each trigger is hit, we take different action
    */
    public Transform[] triggers;
    private int lapsCompleted;
    private bool lapSystemEnabled;

    private void OnEnable()
    {
        lapSystemEnabled = true;
        if (trackCheckpoints.raceType == RaceType.Lap)
        {
            lapText.text = lapMessage;
            counterText.text = "0 / " + numberOfLaps;

            lapsCompleted = 0;

            triggers[0].gameObject.SetActive(true);
            triggers[1].gameObject.SetActive(false);


            endRace.gameObject.SetActive(false);
        }
        else
            gameObject.SetActive(false);
    }

    void Update()
    {
    }

    public void LapSetup(int childIndex)
    {
        if (lapSystemEnabled)
        {
            switch (childIndex)
            {
                case 0:
                    //This logic is called when the start trigger is hit

                    //Increase the number of laps
                    lapsCompleted++;

                    if (lapsCompleted <= numberOfLaps)
                        //If the race is still going, we update the UI
                        counterText.text = lapsCompleted + " / " + numberOfLaps;
                    else
                        //Win logic here
                        endRace.gameObject.SetActive(true);

                    //Deactivate the start trigger and activate the middle trigger
                    triggers[0].gameObject.SetActive(false);
                    triggers[1].gameObject.SetActive(true);

                    break;

                case 1:
                    //This logic is called when the middle trigger is hit

                    //Deactivate the middle trigger and activate the start trigger
                    triggers[1].gameObject.SetActive(false);
                    triggers[0].gameObject.SetActive(true);
                    break;

            }
        }
    }

    public void PauseLapSystem()
    {
        // Logic to pause the lap system
        lapSystemEnabled = false;
    }

    public void ResumeLapSystem()
    {
        // Logic to resume the lap system
        lapSystemEnabled = true;
    }
}
