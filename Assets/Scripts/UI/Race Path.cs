using Barmetler.RoadSystem;
using UnityEngine;

public class RacePath : MonoBehaviour
{
    [Header("UI Objects")]
    public TrackCheckpoints trackCheckpoints;
    public RoadSystemNavigator roadSystemNavigator;
    public SetWaypoint setWaypoint;
    public GameObject lineRenderer;

    private Transform goalTransform;

    public void OnEnable()
    {
        roadSystemNavigator.enabled = true;
        lineRenderer.SetActive(true);
        setWaypoint.enabled = false; // Disable SetWaypoint to prevent conflicts
    }


    public void OnDisable()
    {
        roadSystemNavigator.enabled = false;
        lineRenderer.SetActive(false);
        setWaypoint.enabled = true; // Re-enable SetWaypoint for future use
    }

    public void Update()
    {
        goalTransform = trackCheckpoints.currentCheckpointTransform;
        roadSystemNavigator.Goal = goalTransform.position;
    }

}
