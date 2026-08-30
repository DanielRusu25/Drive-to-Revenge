using UnityEngine;

public class CameraRotationRFF : MonoBehaviour
{
    public int CameraRotationSpeed = 2;
    void LateUpdate ()
    {
         if (Input.GetMouseButton(0) == true)
        {
            transform.RotateAround(transform.position, transform.up, -Input.GetAxis("Mouse X") * CameraRotationSpeed);
        }
    }
}
