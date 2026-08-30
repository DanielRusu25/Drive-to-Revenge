using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Transform zoomObject;
    public float distance = 5f;
    public float zoomSpeed = 2f;

    public float minDistance = 1f;
    public float maxDistance = 10f;

    void Update ()
    {
        float zoomInput = Input.GetAxis ("Mouse ScrollWheel");

        distance -= zoomInput * zoomSpeed;
        distance = Mathf.Clamp (distance , minDistance , maxDistance);

        Vector3 direction = (transform.position - zoomObject.position).normalized;
        transform.position = zoomObject.position + direction * distance;

        transform.LookAt(zoomObject.position);
    }
}
