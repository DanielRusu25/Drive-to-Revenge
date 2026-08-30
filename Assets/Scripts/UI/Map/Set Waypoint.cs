using Barmetler.RoadSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class SetWaypoint : MonoBehaviour
{
    [Header("UI Objects")]
    public Camera topDownCamera;
    public GameObject lineRenderer;

    [Header("Player Components")]
    public Transform playerTransform;
    public RoadSystemNavigator roadSystemNavigator;

    [Header("Goal Data")]
    public GameObject goalIcon;
    public int goalIconHeight = 200;
    public int arrivedDistance = 10;

    private bool goalArrived;

    void OnEnable()
    {
        goalIcon.SetActive(false);
        roadSystemNavigator.enabled = false;
        lineRenderer.SetActive(false);
    }

    void Update()
    {
        if (Vector3.Distance(playerTransform.position, roadSystemNavigator.Goal) <= arrivedDistance)
            goalArrived = true;

        if (goalArrived)
        {
            goalArrived = false;
            goalIcon.SetActive(false);
            lineRenderer.SetActive(false);
            roadSystemNavigator.enabled = false;
        }

    }

    public void SetGoal(InputAction.CallbackContext context)
    {
        if (gameObject.activeInHierarchy)
            if (context.performed)
                GetGoalPosition();
    }

    private void GetGoalPosition()
    {
        Vector2 screenPosition = Pointer.current.position.ReadValue();

        Ray ray = topDownCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 worldPos = hit.point;

            if (Vector3.Distance(playerTransform.position, worldPos) <= arrivedDistance)
                goalArrived = true;
            else
            {
                goalArrived = false;
                roadSystemNavigator.Goal = worldPos;
                roadSystemNavigator.enabled = true;
                lineRenderer.SetActive(true);

                goalIcon.SetActive(true);
                goalIcon.transform.position = new Vector3(worldPos.x, goalIconHeight, worldPos.z);

            }
        }
    }

}
