using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CarController))]
public class InputApply : MonoBehaviour
{
    //This script takes all of the input of the calls from the "PlayerInput" (Unity's new input system)
    //and aplies the necessary data into the car "CarController" script
    private CarController carController;

    void Start()
    {
        carController = GetComponent<CarController>();
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        carController.moveContextValue = context.ReadValue<float>();
    }

    //This function updates the "currentGear" variable
    public void GearShiftInput(InputAction.CallbackContext context)
    {
        //Only if the transmision type is "Manual" we allow to manually change gears
        if (carController.transmisionType == TransmisionType.Manual)
        {
            carController.gearContextValue = Mathf.RoundToInt(context.ReadValue<float>());
            if (context.performed)
                carController.shouldShift = true;
        }
    }

    public void SteerInput(InputAction.CallbackContext context)
    {
        //This tells how much to steer
        carController.steerContextValue = context.ReadValue<float>();


        //This tells if it should be stearing at all
        if (context.performed)
            carController.shouldSteer = true;

        if (context.canceled)
            carController.shouldSteer = false;

    }

    public void HandbrakeInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            carController.shouldHandbrake = true;

        if (context.canceled)
            carController.shouldHandbrake = false;
    }

    public void NitrousInput(InputAction.CallbackContext context)
    {
        if (context.performed == true)
            carController.shouldNitro = true;

        if (context.canceled == true)
            carController.shouldNitro = false;
    }

}
