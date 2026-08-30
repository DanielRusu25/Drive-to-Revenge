using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public Transform lookPosition;

    private void LateUpdate ()
    {
        transform.LookAt(lookPosition);
    }
}
