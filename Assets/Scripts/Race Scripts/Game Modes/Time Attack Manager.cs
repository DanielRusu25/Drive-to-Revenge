using UnityEngine;
using TMPro;
using System.Collections;

public class TimeAttackManager : MonoBehaviour
{
    [Header("End Race Script")]
    public EndRace endRace;

    [Space(10)]
    [Header("Display Variables")]
    public TMP_Text infoText;
    public TMP_Text timeText;
    public string infoMessage;

    //Time struct
    [System.Serializable]
    public struct TimerStruct
    {
        public int minutes;
        public int seconds;
    }
    [Space(10)]
    public TimerStruct winTime;
    private TimerStruct currentTime;

    private void OnEnable()
    {
        infoText.text = infoMessage;

        currentTime.seconds = 0;
        currentTime.minutes = 0;

    }

    private void Update()
    {
        //Formatting the time text; 
        timeText.text = $"{currentTime.minutes:D2}:{currentTime.seconds:D2}";
    }

    public IEnumerator Timer()
    {
        while (endRace.raceFinished == false)
        {
            //Race is still going, we keep going with the countdown
            currentTime.seconds++;
            yield return new WaitForSeconds(1);

            if (currentTime.seconds == 60)
            {
                currentTime.minutes++;
                currentTime.seconds = 0;
            }
        }

        //Race ended, update the EndRace script and stop the countdown
        if (currentTime.minutes > winTime.minutes)
            endRace.raceWon = false;
        else if (currentTime.minutes == winTime.minutes)
            if (currentTime.seconds > winTime.seconds)
                endRace.raceWon = false;
            else
                endRace.raceWon = true;
        else if (currentTime.minutes < winTime.minutes)
            endRace.raceWon = true;

        endRace.raceInformation = $"{currentTime.minutes:D2}:{currentTime.seconds:D2}";

        yield break;
    }


}
