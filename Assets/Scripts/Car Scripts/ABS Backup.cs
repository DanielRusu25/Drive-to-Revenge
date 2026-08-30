using UnityEngine;

public class ABSBackup : MonoBehaviour
{
    public WheelCollider[] wheelColliders;
    public float absSlipThreshold;
    public float[] brakePressures;
    public int brakeTorque;
    public int moveAxis;

    public void FootBrake()
  {

    float averageWheelSlip = 0;
    for (int i = 0; i < wheelColliders.Length; i++)
    {
      WheelHit wheelHit;
      wheelColliders[i].GetGroundHit(out wheelHit);
      averageWheelSlip += wheelHit.forwardSlip;
    }

    averageWheelSlip /= wheelColliders.Length;

    for (int i = 0; i < wheelColliders.Length; i++)
    {
      WheelHit wheelHit;
      wheelColliders[i].GetGroundHit(out wheelHit);
      float slip = (averageWheelSlip - wheelHit.forwardSlip) / averageWheelSlip;
      //Debug.Log("Wheel " + i + " :" + slip);

      //If it is above the "absSlipThreshold" we slowly lower the pressure of the wheel brake
      if (slip > absSlipThreshold)
      {
        brakePressures[i] -= 0.05f;
        if (brakePressures[i] < 0)
          brakePressures[i] = 0f;
      }
      else //We slowly raise the pressure of the wheel brake
      {
        brakePressures[i] += 0.05f;
        if (brakePressures[i] > 1)
          brakePressures[i] = 1f;
      }
      
    }


    //We finllay apply the brakes
    for (int i = 0; i < wheelColliders.Length; i++)
    {
      wheelColliders[i].brakeTorque = brakeTorque * brakePressures[i] * Mathf.Abs(moveAxis);
      //wheelColliders[i].brakeTorque = Mathf.Abs(brakeTorque * moveAxis) ;
    }
  }

}
