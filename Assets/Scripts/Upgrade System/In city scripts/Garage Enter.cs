using System.Collections;
using UnityEngine;

public class GarageEnter : MonoBehaviour
{
   [HideInInspector] public GarageManager garageManager;

    
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player Body")
        {
            StartCoroutine(garageManager.ShowGarageEnterMenu());
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player Body")
        {
            StartCoroutine(garageManager.HideGarageEnterMenu());
        }
    }

    
    
}
