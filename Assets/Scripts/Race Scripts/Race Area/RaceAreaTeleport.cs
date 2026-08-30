using UnityEngine;

public class RaceAreaTeleport : MonoBehaviour
{
    //This script teleports the car, when this trigger is hit

    private TrackRaceAreas trackRaceAreas;

    private void OnEnable()
    {
        trackRaceAreas = GetComponentInParent<TrackRaceAreas>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player Body")
            StartCoroutine(trackRaceAreas.trackCheckpoints.TeleportToCheckpoint());
    }
}
