using UnityEngine;

public class DisableTracker : MonoBehaviour
{
    void OnDisable()
    {
        Debug.LogWarning($"[DisableTracker] {gameObject.name} was disabled!", this);
        Debug.LogWarning("Call Stack:\n" + System.Environment.StackTrace);
    }
}
