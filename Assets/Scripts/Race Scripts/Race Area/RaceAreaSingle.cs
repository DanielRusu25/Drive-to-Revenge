using UnityEngine;

public class RaceAreaSingle : MonoBehaviour
{
    //This script warns the player that it is out of the race area
    private TrackRaceAreas trackRaceAreas;

    private void OnEnable()
    {
        trackRaceAreas = GetComponentInParent<TrackRaceAreas>();
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Player Body")
        {
            trackRaceAreas.countdownPanelControl.shouldFade = true;
            trackRaceAreas.countdownPanelControl.messageText.text = trackRaceAreas.messageToDisplay;
            trackRaceAreas.countdownPanelControl.countdownText.text = "";
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player Body")
            trackRaceAreas.countdownPanelControl.shouldFade = false;
    }
}

