using UnityEngine;

public class ReverseTeleport : MonoBehaviour
{
    private ReverseControl reverseControl;

    private void OnEnable()
    {
        reverseControl = GetComponentInParent<ReverseControl>();        
    }

    private void OnTriggerEnter (Collider collider)
    {
        if (collider.tag == "Player Body")
        {
            StartCoroutine( reverseControl.trackCheckpoints.TeleportToCheckpoint());
        }
    }
}
