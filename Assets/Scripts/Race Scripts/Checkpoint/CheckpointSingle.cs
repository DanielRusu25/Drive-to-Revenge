using Unity.VisualScripting;
using UnityEngine;

public class CheckpointSingle : MonoBehaviour
{
    private TrackCheckpoints trackCheckpoints;

    public void OnEnable()
    {
        trackCheckpoints = GetComponentInParent<TrackCheckpoints>();
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player Body")
        {
            //Activate and deactivate the requierd checkpoints
            trackCheckpoints.ManageCheckpoint(trackCheckpoints.currentCheckpointIndex);

            //Update the current checkpoint
            trackCheckpoints.currentCheckpointIndex = (1 + trackCheckpoints.currentCheckpointIndex) % trackCheckpoints.totalCheckpointNumber;
                
        }
    }

}
