using UnityEngine;

public class PlaceCheckpoint : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        RacerProgress rp = other.GetComponentInParent<RacerProgress>();
        if (rp != null)
        {
            int thisCheckpointIndex = transform.GetSiblingIndex();
            if (thisCheckpointIndex == rp.expectedCheckpointIndex)
            {
                rp.checkpointIndex = thisCheckpointIndex + 1;

                //When we arrive back at the first checkpoint, we increase the number of laps
                if (rp.expectedCheckpointIndex == 0)
                    rp.currentLap++;
                
                // If it's the last checkpoint, wrap around and add lap
                if (rp.checkpointIndex == transform.parent.childCount)
                    rp.expectedCheckpointIndex = 0;
                else
                    rp.expectedCheckpointIndex++;




                //SprintManager sprintManager = GetComponentInParent<SprintManager>();
                //sprintManager.UpdateRacers();
            }
        }
    }

}
