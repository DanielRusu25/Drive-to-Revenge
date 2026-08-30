using UnityEngine;

public class CarEffects : MonoBehaviour
{
    // The following particle systems are used as tire smoke when the car drifts.
    public ParticleSystem rearLeftSmoke;
    public ParticleSystem rearRightSmoke;

    // The following trail renderers are used as tire skids when the car loses traction.
    public TrailRenderer rearLeftTireSkid;
    public TrailRenderer rearRightTireSkid;

    //The following particle systems are used as exausth "bang" / fire when we shift gears
    public bool useYellowParticles;
    public ParticleSystem leftYellowExausth;
    public ParticleSystem rightYellowExausth;

    //The following particle systems are used to display the use of nitrous, 
    //through some blue particles exeting the exausth
    public bool useNitrousParticles;
    public ParticleSystem leftNitrousExausth;
    public ParticleSystem rightNitrousExausth;

    private CarController carController;
    private AiCarController aiCarController; //We use this in case we want to control an AI;

    public void Start()
    {
        carController = GetComponent<CarController>();

        if (rearLeftSmoke != null)
            rearLeftSmoke.Stop();
        if (rearRightSmoke != null)
            rearRightSmoke.Stop();


        if (rearLeftTireSkid != null)
            rearLeftTireSkid.emitting = false;
        if (rearRightTireSkid != null)
            rearRightTireSkid.emitting = false;

        if (leftYellowExausth != null)
            leftYellowExausth.Stop();
        if (rightYellowExausth != null)
            rightYellowExausth.Stop();


        if (leftNitrousExausth != null)
            leftNitrousExausth.Stop();
        if (rightNitrousExausth != null)
            rightNitrousExausth.Stop();

    }

    public void Update()
    {
        //If the forces aplied to the rigidbody in the 'x' asis are greater than
        //3f, it means that the car is losing traction, then the car will start emitting particle systems.

        if (Mathf.Abs(carController.localVelocityX) > 2.5f)
            carController.isDrifting = true;
        else
            carController.isDrifting = false;

        DriftCarPS();

        YellowExausthPS();
        NitrousPS();
    }

    // This function is used to emit both the particle systems of the tires' smoke and the trail renderers of the tire skids
    // depending on the value of the bool variables 'isDrifting' and 'isTractionLocked'.
    private void DriftCarPS()
    {
        if (carController.isDrifting)
        {
            rearLeftSmoke.Play();
            rearRightSmoke.Play();
        }
        else if (!carController.isDrifting)
        {
            rearLeftSmoke.Stop();
            rearRightSmoke.Stop();
        }


        if ((carController.isTractionLocked || Mathf.Abs(carController.localVelocityX) > 5f) && Mathf.Abs(carController.carSpeed) > 12f)
        {
            rearLeftTireSkid.emitting = true;
            rearRightTireSkid.emitting = true;
        }
        else
        {
            rearLeftTireSkid.emitting = false;
            rearRightTireSkid.emitting = false;
        }


    }

    private void YellowExausthPS()
    {
        if (useYellowParticles == true)
        {
            if (carController.shouldNitro == false && carController.currentGear >= carController.numberOfGears / 2 
            && leftYellowExausth.isPlaying == false)
            {
                //Depending on the transmision type, we calculate when to shift differently, 
                //so we have 2 cases when we play the 'bang' sound
                if (carController.transmisionType == TransmisionType.Manual)
                {
                    if (carController.shouldShift == true && carController.currentGear < carController.numberOfGears 
                    && carController.currentEngineRPM > carController.redlineRPM/2)
                    {
                        leftYellowExausth.Play();
                        rightYellowExausth.Play();
                    }
                }
                else if (carController.transmisionType == TransmisionType.Automatic)
                {
                    if (carController.gearState == GearState.Changing)
                    {
                        leftYellowExausth.Play();
                        rightYellowExausth.Play();
                    }
                }
            }
        }
    }

    private void NitrousPS()
    {
        if (useNitrousParticles == true)
        {
            if (carController.shouldNitro == true)
                if (leftNitrousExausth.isPlaying == false)
                {
                    leftNitrousExausth.Play();
                    rightNitrousExausth.Play();
                }
                else return;
            else
            {
                leftNitrousExausth.Stop();
                rightNitrousExausth.Stop();
            }
        }
    }
}
