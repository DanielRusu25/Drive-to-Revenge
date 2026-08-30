using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System.Linq;

public class SprintManager : MonoBehaviour
{
    [Header("End Race Script")]
    public EndRace endRace;

    [Space(10)]
    [Header("Display Variables")]
    public TMP_Text infoText;
    public TMP_Text placeText;
    public string infoMessage;
    public int winningPlace;

    [Space(10)]
    [Header("Racer Containers")]
    public GameObject aiCarsContainer;
    public GameObject playerCar;

    [Header("Place Checkpoint Container")]
    public GameObject placeCheckpointConteiner;


    private List<Transform> placeCheckpoints = new List<Transform>();
    private List<RacerProgress> racerProgresses = new List<RacerProgress>();

    private void OnEnable()
    {
        //Set up UI
        infoText.text = infoMessage;

        SetUpRacers();

        //Set up checkpoints
        foreach (Transform transform in placeCheckpointConteiner.transform)
            placeCheckpoints.Add(transform);

        DistanceCheck();
        UpdateRacers();
    }

    private void Update()
    {
        DistanceCheck();
        UpdateRacers();
    }

    private void SetUpRacers()
    {
        AttachScript(playerCar);
        RacerProgress playerRp = playerCar.GetComponent<RacerProgress>();
        NullRacerProgress(playerRp);
        foreach (Transform child in aiCarsContainer.transform)
        {
            GameObject aiRacer = child.gameObject;
            AttachScript(aiRacer);
            NullRacerProgress(aiRacer.GetComponent<RacerProgress>());
        }
    }

    private void AttachScript(GameObject gO)
    {
        if (gO.GetComponent<RacerProgress>() == null)
            gO.AddComponent<RacerProgress>();

    }

    private void NullRacerProgress(RacerProgress racer)
    {
        racerProgresses.Add(racer);
        racer.racerName = racer.gameObject.name;
        racer.checkpointIndex = 0;
        racer.expectedCheckpointIndex = 0;
        racer.currentLap = 0;
        racer.distanceToNextCheckpoint = 0;
        racer.racePosition = 0;
    }

    public void UpdateRacers()
    {
        //We sort the racers so that the first one in the list is winning.
        //We do this by comparing different data depending on its importance.
        racerProgresses.Sort((a, b) =>
        {
            if (a.currentLap != b.currentLap)
                return b.currentLap.CompareTo(a.currentLap);

            if (a.checkpointIndex != b.checkpointIndex)
                return b.checkpointIndex.CompareTo(a.checkpointIndex);

            return a.distanceToNextCheckpoint.CompareTo(b.distanceToNextCheckpoint);
        });

        //Update the positions
        int playerPosition = 1;


        for (int i = 0; i < racerProgresses.Count; i++)
        {
            racerProgresses[i].racePosition = i + 1;
            if (racerProgresses[i].name == playerCar.name)
                playerPosition = racerProgresses[i].racePosition;
        }

        //Update the UI 
        placeText.text = $"{playerPosition} / {racerProgresses.Count} ";

        if (endRace.raceFinished)
            RaceHasEnded();
    }

    private void DistanceCheck()
    {
        for (int i = 0; i < racerProgresses.Count; i++)
            racerProgresses[i].distanceToNextCheckpoint = Vector3.Distance
            (
                racerProgresses[i].transform.position,
                placeCheckpoints[racerProgresses[i].expectedCheckpointIndex].transform.position
            );

    }

    private void RaceHasEnded()
    {
        int playerPlace = 0;

        /*
        Debug.Log(playerCar.name);
        for (int i = 0; i < racerProgresses.Count && playerPlace == 0; i++)
        {
            Debug.Log(racerProgresses[i].racerName);
            if (racerProgresses[i].racerName == playerCar.name)
                playerPlace = racerProgresses[i].racePosition;
        }
        */
        playerPlace = racerProgresses.FirstOrDefault(c => c.racerName.Contains(playerCar.name)).racePosition;

        if (playerPlace <= winningPlace)
            endRace.raceWon = true;
        else
            endRace.raceWon = false;

        endRace.raceInformation = GetOrdinal(playerPlace);
    }

    private string GetOrdinal(int number)
    {
        int lastTwoDigits = number % 100;
        if (lastTwoDigits >= 11 && lastTwoDigits <= 13)
            return number + "th";

        switch (number % 10)
        {
            case 1:
                return number + "st";
            case 2:
                return number + "nd";
            case 3:
                return number + "rd";
            default:
                return number + "th";
        }
    }
}
