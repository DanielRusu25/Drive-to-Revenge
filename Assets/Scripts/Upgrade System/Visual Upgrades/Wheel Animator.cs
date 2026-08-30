using UnityEngine;

public class WheelAnimator : MonoBehaviour
{
    public CarSwapper carSwapper;
    
    public void Update()
    {
        AnimateWheelMeshes();
    }


    // This method matches both the position and rotation of the WheelColliders with the WheelMeshes.
    void AnimateWheelMeshes()
    {

        foreach (var wheel in carSwapper.phisicalCarComponents.wheels)
        {
            // Get the WheelCollider's position and rotation
            Quaternion wheelRotation;
            Vector3 wheelPosition;
            wheel.wheelWithCollider.GetComponent<WheelCollider>().GetWorldPose(out wheelPosition, out wheelRotation);

            // Set the WheelMesh's position and rotation
            wheel.wheelWithMesh.transform.localPosition = wheel.wheelColliderTransform.localPosition;
            wheel.wheelWithMesh.transform.localRotation = wheel.wheelColliderTransform.localRotation;
        }


    }
}
