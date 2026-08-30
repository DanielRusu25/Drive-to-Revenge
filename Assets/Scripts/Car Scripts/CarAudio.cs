using UnityEngine;
using Random = UnityEngine.Random;



public class CarAudio : MonoBehaviour
{
    // This script reads some of the car's current properties and plays sounds accordingly.
    // The engine sound is a crossfaded blend of four clips
    // which represent the timbre of the engine
    // at different RPM and Throttle state.

    // the engine clips should all be a steady pitch, not rising or falling.

    // lowAccelClip : The engine at low revs, with throttle open (i.e. begining acceleration at very low speed)
    // highAccelClip : The engine at high revs, with throttle open (i.e. accelerating, but almost at max speed)
    // lowDecelClip : The engine at low revs, with throttle at minimum (i.e. idling or engine-braking at very low speed)
    // highDecelClip : The engine at high revs, with throttle at minimum (i.e. engine-braking at very high speed)

    // For proper crossfading, the clips pitches should all match, with an octave offset between low and high.

    public AudioClip lowAccelClip;                                              // Audio clip for low acceleration
    public AudioClip lowDecelClip;                                              // Audio clip for low deceleration
    public AudioClip highAccelClip;                                             // Audio clip for high acceleration
    public AudioClip highDecelClip;                                             // Audio clip for high deceleration
    public AudioClip engineBangClip;                                            // Audio clip for bang sound (when it shifts and makes a flame)

    public AudioClip tireSkidClip;                                              //Audio clip for tire skid

    public float volume = 1f;                                                   // Used to modify the volume of the sounds 
    public float pitchMultiplier = 1f;                                          // Used for altering the pitch of audio clips
    public float lowPitchMin = 1f;                                              // The lowest possible pitch for the low sounds
    public float lowPitchMax = 6f;                                              // The highest possible pitch for the low sounds
    public float highPitchMultiplier = 0.25f;                                   // Used for altering the pitch of high sounds
    public float maxRolloffDistance = 500;                                      // The maximum distance where rollof starts to take place
    public bool useDoppler = true;                                              // Toggle for using doppler
    public float dopplerLevel = 1;                                              // The mount of doppler effect used in the audio

    private AudioSource m_LowAccel; // Source for the low acceleration sounds
    private AudioSource m_LowDecel; // Source for the low deceleration sounds
    private AudioSource m_HighAccel; // Source for the high acceleration sounds
    private AudioSource m_HighDecel; // Source for the high deceleration sounds
    private AudioSource m_EngineBang; // Source for the engine bang (when it shifts and makes a flame)
    private AudioSource m_TireSkid; // Source for the tire skid sounds 

    private bool m_StartedEngineSound; // flag for knowing if we have started the engine sounds
    private bool m_StartedTireSounds; // flag for knowing if we have started the tire skid sounds

    private CarController carController; // Reference to car we are controlling

    private void Update()
    {
        // get the distance to main camera
        float camDist = (Camera.main.transform.position - transform.position).sqrMagnitude;

        // stop sound if the object is beyond the maximum roll off distance
        if (m_StartedEngineSound && camDist > maxRolloffDistance * maxRolloffDistance)
            StopSound();

        // start the sound if not playing and it is nearer than the maximum distance
        if (!m_StartedEngineSound && camDist < maxRolloffDistance * maxRolloffDistance)
            StartSound();

        //Controling the engine sounds
        EngineSounds();
        EngineBangSound();

        //Controlling the Tire Sounds
        TireSkidSounds();
    }

    private void StartSound()
    {
        // get the carcontroller ( this will not be null as we have require component)
        carController = GetComponent<CarController>();

        // setup the audio sources
        m_HighAccel = SetUpEngineAudioSource(highAccelClip);
        m_LowAccel = SetUpEngineAudioSource(lowAccelClip);
        m_LowDecel = SetUpEngineAudioSource(lowDecelClip);
        m_HighDecel = SetUpEngineAudioSource(highDecelClip);

        m_EngineBang = SetUpOtherAudioSource(engineBangClip);
        m_TireSkid = SetUpOtherAudioSource(tireSkidClip);


        // flag that we have started the sounds playing
        m_StartedEngineSound = true;
    }


    private void StopSound()
    {
        //Destroy all audio sources on this object:
        foreach (var source in GetComponents<AudioSource>())
            Destroy(source);

        m_StartedEngineSound = false;
    }

    private void EngineSounds()
    {
        if (m_StartedEngineSound)
        {
            // The pitch is interpolated between the min and max values, according to the car's revs.
            float pitch = ULerp(lowPitchMin, lowPitchMax, carController.currentEngineRPM / carController.maxRPM);

            // clamp to minimum pitch (note, not clamped to max for high revs while burning out)
            pitch = Mathf.Min(lowPitchMax, pitch);

            // adjust the pitches based on the multipliers
            m_LowAccel.pitch = pitch * pitchMultiplier;
            m_LowDecel.pitch = pitch * pitchMultiplier;
            m_HighAccel.pitch = pitch * highPitchMultiplier * pitchMultiplier;
            m_HighDecel.pitch = pitch * highPitchMultiplier * pitchMultiplier;

            // get values for fading the sounds based on the acceleration
            float accFade = Mathf.Abs(carController.moveAxis);
            float decFade = 1 - accFade;

            // get the high fade value based on the cars revs
            float highFade = Mathf.InverseLerp(0.2f, 0.8f, carController.currentEngineRPM / carController.maxRPM);
            float lowFade = 1 - highFade;

            // adjust the values to be more realistic
            highFade = 1 - ((1 - highFade) * (1 - highFade));
            lowFade = 1 - ((1 - lowFade) * (1 - lowFade));
            accFade = 1 - ((1 - accFade) * (1 - accFade));
            decFade = 1 - ((1 - decFade) * (1 - decFade));

            // adjust the source volumes based on the fade values
            m_LowAccel.volume = lowFade * accFade;
            m_LowDecel.volume = lowFade * decFade;
            m_HighAccel.volume = highFade * accFade;
            m_HighDecel.volume = highFade * decFade;

        }

    }

    private void EngineBangSound()
    {
        if (m_EngineBang != null)
            if (carController.shouldNitro == false && carController.currentGear >= carController.numberOfGears / 2 && m_EngineBang.isPlaying == false)
            {
                //Depending on the transmision type, we calculate when to shift differently, 
                //so we have 2 cases when we play the 'bang' sound

                if (carController.transmisionType == TransmisionType.Manual) //Manual
                {
                    if (carController.shouldShift == true && carController.currentGear < carController.numberOfGears
                    && carController.currentEngineRPM > carController.redlineRPM / 2)
                        m_EngineBang.Play();
                }
                else                                                         //Automatic
                {
                    if (carController.gearState == GearState.Changing)
                        m_EngineBang.Play();
                }
            }

    }

    private void TireSkidSounds()
    {
        if (m_TireSkid != null)
            if ((carController.isDrifting || (carController.isTractionLocked && Mathf.Abs(carController.carSpeed) > 12f)) && m_StartedTireSounds == false)
            {
                m_TireSkid.Play();
                m_StartedTireSounds = true;
            }
            else if (!carController.isDrifting && (!carController.isTractionLocked || Mathf.Abs(carController.carSpeed) < 12f) && m_StartedTireSounds == true)
            {
                m_TireSkid.Stop();
                m_StartedTireSounds = false;
            }
    }


    // sets up and adds new audio source to the gane object
    private AudioSource SetUpEngineAudioSource(AudioClip clip)
    {
        // create the new audio source component on the game object and set up its properties
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = 0;
        source.loop = true;
        source.volume = volume;

        // start the clip from a random point
        source.time = Random.Range(0f, clip.length);
        source.Play();
        source.minDistance = 5;
        source.maxDistance = maxRolloffDistance;
        source.dopplerLevel = useDoppler ? dopplerLevel : 0;
        return source;
    }

    private AudioSource SetUpOtherAudioSource(AudioClip clip)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = 0.3f;
        source.loop = false;
        source.volume = volume;

        source.minDistance = 5;
        source.maxDistance = maxRolloffDistance;
        source.dopplerLevel = useDoppler ? dopplerLevel : 0;


        source.Pause();
        return source;
    }

    // unclamped versions of Lerp and Inverse Lerp, to allow value to exceed the from-to range
    private static float ULerp(float from, float to, float value)
    {
        return (1.0f - value) * from + value * to;
    }
}
