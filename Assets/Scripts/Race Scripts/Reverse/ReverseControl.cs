using UnityEngine;

public class ReverseControl : MonoBehaviour
{
    //Going reverse detecion system
    public TrackCheckpoints trackCheckpoints;
    public CanvasGroup reversePanel;
    public int reverseFadeSpeed = 5;
    public int teleportDistance = 50;


    //Car reverse system
    private float carDot;
    private Transform carTransform;

    private void OnEnable()
    {
        carTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();

        //Setup for revrese panel
        reversePanel.alpha = 0;

        //Deactivate all the 'ReverseTeleports'
        foreach (Transform tr in GetComponentInChildren<Transform>())
            tr.gameObject.SetActive(false);
    }

    private void Update()
    {
        //This dot product checks if the car goes backwards
        Vector3 direction2 = carTransform.TransformDirection(Vector3.forward);
        Vector3 toCheckpoint = Vector3.Normalize(trackCheckpoints.currentCheckpointTransform.position - carTransform.position);
        carDot = Vector3.Dot(direction2, toCheckpoint);

        if (trackCheckpoints.checkpointDot < -10 && carDot < 0.2)
        {
            //Going backwards

            //If the race type is lap, while we go backwards we deactivate the lap system
            if (trackCheckpoints.raceType == RaceType.Lap)
                trackCheckpoints.lapSystem.PauseLapSystem();

            reversePanel.alpha += Time.deltaTime * reverseFadeSpeed;

            for (int i = 0; i < transform.childCount; i++)
            {
                //We activate all the reverse teleport position, without the ones in the 'teleportDistance'
                Transform child = transform.GetChild(i);
                if (Vector3.Distance(carTransform.position, child.position) >= teleportDistance
                    && child.gameObject.activeSelf == false)
                    child.gameObject.SetActive(true);
            }
        }
        else
        {
            //Going forwards

            //If the race type is lap, while we go forwards we activate the lap system
            if (trackCheckpoints.raceType == RaceType.Lap)
                trackCheckpoints.lapSystem.ResumeLapSystem();

            reversePanel.alpha -= Time.deltaTime * reverseFadeSpeed;

            for (int i = 0; i < transform.childCount; i++)
            {
                //We deactivate all the going backwards positions
                Transform child = transform.GetChild(i);
                if (child.gameObject.activeSelf == true)
                    child.gameObject.SetActive(false);
            }
        }

    }

    
}
