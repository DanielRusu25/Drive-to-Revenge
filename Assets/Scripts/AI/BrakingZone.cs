using UnityEngine;

public class BrakingZone : MonoBehaviour
{
    public void OnTriggerEnter (Collider collider)
    {
        AiLogic aiLogic = collider.GetComponentInParent<AiLogic>();
        if (aiLogic)
            aiLogic.shouldBrake = true;
    }

    public void OnTriggerExit (Collider collider)
    {
        AiLogic aiLogic = collider.GetComponentInParent<AiLogic>();
        if (aiLogic)
            aiLogic.shouldBrake = false;
    }
}
