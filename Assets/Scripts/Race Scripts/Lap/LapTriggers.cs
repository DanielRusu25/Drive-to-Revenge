using UnityEngine;

public class LapTriggers : MonoBehaviour
{
    private LapSystem lapSystem;

    void Start()
    {
        lapSystem = GetComponentInParent<LapSystem>();        
    }

    private void OnTriggerEnter (Collider collider)
    {
        if (collider.tag == "Player Body" && lapSystem.trackCheckpoints.raceType == RaceType.Lap)
            lapSystem.LapSetup(transform.GetSiblingIndex());
    }

}
