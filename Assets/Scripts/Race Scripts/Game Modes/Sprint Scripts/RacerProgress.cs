using UnityEngine;

//This script is attached to all racers (Player or AI) 
//in order for each one to have special information about the race
public class RacerProgress : MonoBehaviour
{
    public string racerName; // Player or AI name
    public int currentLap;
    public int checkpointIndex;
    public int expectedCheckpointIndex;
    public float distanceToNextCheckpoint;
    public int racePosition;

}

