using UnityEngine;
using UnityEngine.InputSystem;

public class BigMapController : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float zoomSpeed = 50f;
    public float minZoom = 10f;
    public float maxZoom = 100f;

    [Header("Pan Settings")]
    public float panSpeed = 0.5f;

    [Header("Pan Bounds (X-Z plane)")]
    public Vector3 minPanBounds = new Vector3(-100f, 0f, -100f);
    public Vector3 maxPanBounds = new Vector3(100f, 0f, 100f);

    private Camera cam;
    private bool isPanning = false;

    private float zoomInput = 0f;
    private Vector2 panInput = Vector2.zero;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = maxZoom; // Start fully zoomed out
    }

    private void Update()
    {
        // Zoom
        if (Mathf.Abs(zoomInput) > 0.1f)
        {
            cam.orthographicSize -= zoomInput * zoomSpeed * Time.deltaTime;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }

        // Pan (while panning is active)
        if (isPanning && panInput.sqrMagnitude > 0.01f)
        {
            Vector3 delta = new Vector3(-panInput.x, -panInput.y, 0f) * panSpeed * cam.orthographicSize * 0.01f;
            cam.transform.Translate(delta, Space.Self);
        }
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            zoomInput = context.ReadValue<float>();
        }
    }

    public void OnPan(InputAction.CallbackContext context)
    {
        // Always store pan input, whether from stick or D-pad
        if (context.performed || context.canceled)
        {
            panInput = context.ReadValue<Vector2>();
        }
    }

    public void OnPanClick(InputAction.CallbackContext context)
    {
        if (context.performed)
            isPanning = true;
        else if (context.canceled)
            isPanning = false;
    }

}
