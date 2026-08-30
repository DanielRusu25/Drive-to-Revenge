using UnityEngine;
using UnityEngine.InputSystem;

public class CarLights : MonoBehaviour
{
    public Light[] headlights;
    public Light[] stoplights;
    public Light[] reverseLights;

    private CarController carController;
    private bool headlightsTrigger;

    void Start()
    {
        carController = GameObject.FindWithTag("Player").GetComponent<CarController>();

        headlightsTrigger = false;
        TurnOff(headlights);
        TurnOff(stoplights);
        TurnOff(reverseLights);
    }

    void Update()
    {
        //Brake Lights
        if (carController.moveContextValue < 0)
            TurnOn(stoplights);
        else
            TurnOff(stoplights);

        //Reverse Lights
        if(carController.currentGear == 0)
            TurnOn(reverseLights);
        else
            TurnOff(reverseLights);

        //Headlights
        if (headlightsTrigger == true)
            TurnOn(headlights);
        else
            TurnOff(headlights);
    }
    
    public void HeadLightsInput (InputAction.CallbackContext context)
    {
        if (context.performed)
            if(headlightsTrigger == false)
                headlightsTrigger = true;
            else
                headlightsTrigger = false;
    }

    private void TurnOff(Light[] lights)
    {
        if (lights[0].enabled == true)
            foreach (Light li in lights)
                li.enabled = false;
    }

    private void TurnOn(Light[] lights)
    {
        if(lights[0].enabled == false)
        foreach (Light li in lights)
            li.enabled = true;
    }


}
