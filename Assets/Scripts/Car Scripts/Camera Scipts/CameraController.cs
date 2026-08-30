using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform carTransform;
    public Transform cameraTransform;
    [Range(1, 10)]
    public int CameraRotationSpeed = 4;
    public string[] noButtonControlSchemes;

    [System.Serializable]
    public struct CameraPositions
    {
        public bool shouldRotate;
        public Vector3 cameraPosition;
    }
    public CameraPositions[] cameraPositions;

    private int currentCameraIndex;
    private float CarY;

    private PlayerInput playerInput;
    private float rotationContextValue = 0;
    private bool rotateCamera;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();

        currentCameraIndex = 0;

        //Modify the Camera's position, which is the only child of the rotating object
        cameraTransform.localPosition = cameraPositions[currentCameraIndex].cameraPosition;

        //Set the angle of the rotating object to the one of the car
        CarY = carTransform.transform.eulerAngles.y;
        transform.eulerAngles = new Vector3(0, Mathf.Lerp(transform.eulerAngles.y, CarY, CameraRotationSpeed), 0);

    }

    public void ActivateRotationCamera(InputAction.CallbackContext context)
    {
        //Let the camera rotate or not, depending on the "shouldRotate" bool of the current camera
        if (cameraPositions[currentCameraIndex].shouldRotate)
            //Find what control scheme is currently in use and if we want to activate by button, 
            //search if it IS NOT inside the no button control schemes array
            if (!Array.Exists(noButtonControlSchemes, element => element == playerInput.currentControlScheme))
            {
                if (context.performed)
                    rotateCamera = true;

                if (context.canceled)
                    rotateCamera = false;
            }
    }

    public void RotateCamera(InputAction.CallbackContext context)
    {
        //Let the camera rotate or not, depending on the "shouldRotate" bool of the current camera
        if (cameraPositions[currentCameraIndex].shouldRotate)
            //Find what control scheme is currently in use and if we DO NOT want to activate by button, 
            //search if it IS inside the no button control schemes array
            if (Array.Exists(noButtonControlSchemes, element => element == playerInput.currentControlScheme))
            {
                if (context.performed)
                    rotateCamera = true;

                if (context.canceled)
                    rotateCamera = false;
            }


        if (rotateCamera == true)
            rotationContextValue = context.ReadValue<float>();
        else
        {
            CarY = carTransform.transform.eulerAngles.y;
            transform.eulerAngles = new Vector3(0, Mathf.Lerp(transform.eulerAngles.y, CarY, CameraRotationSpeed), 0);
        }
    }

    public void SwapCameraPOV(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            currentCameraIndex++;

            if (currentCameraIndex == cameraPositions.Length)
                currentCameraIndex = 0;

            //Modify the Camera's position, which is the only child of the rotating object
            cameraTransform.localPosition = cameraPositions[currentCameraIndex].cameraPosition;

            //Set the angle of the rotating object to the one of the car
            CarY = carTransform.eulerAngles.y;
            transform.eulerAngles = new Vector3(0, Mathf.Lerp(transform.eulerAngles.y, CarY, CameraRotationSpeed), 0);

        }
    }

    private void FixedUpdate()
    {
        if (rotateCamera == true)
        {
            if (rotationContextValue > 0)
            {
                transform.RotateAround(transform.position, transform.up, -CameraRotationSpeed );
                Vector3 currentRotation = transform.eulerAngles;
                transform.eulerAngles = new Vector3(0, currentRotation.y, 0);
            }
            else if (rotationContextValue < 0)
            {
                transform.RotateAround(transform.position, transform.up, CameraRotationSpeed );
                Vector3 currentRotation = transform.eulerAngles;
                transform.eulerAngles = new Vector3(0, currentRotation.y, 0);
            }
        }
    }


}

