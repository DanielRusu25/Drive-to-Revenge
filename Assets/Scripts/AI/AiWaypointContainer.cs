using System.Collections.Generic;
using UnityEngine;

public class AiWaypointContainer : MonoBehaviour
{
    public List<Transform> aiWaypoints;

    void Awake()
    {
        foreach (Transform tr in gameObject.GetComponentsInChildren<Transform>() )
            aiWaypoints.Add(tr);

        aiWaypoints.Remove(aiWaypoints[0]);
    }

    
}
