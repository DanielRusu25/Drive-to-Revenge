using UnityEngine;

public class RaceAudioManager : MonoBehaviour
{
    [Header("Settings")]
    [Range(0f , 1f)] public float volume;

    [Header("Track Checkpoints")]
    public AudioClip checkpointTriggerClip;
    public AudioClip countdownClip;

    [Header("Start Race")]
    public AudioClip startClip;
    public AudioClip middleClip;
    public AudioClip finishClip;

    //Audio Sources

    //Track Checkpoints
    [HideInInspector] public AudioSource checkpointTriggerSource;
    [HideInInspector] public AudioSource countdownSource;

    //Starting Race
    [HideInInspector] public AudioSource startSource;
    [HideInInspector] public AudioSource middleSource;
    [HideInInspector] public AudioSource finishSource;



    private void OnEnable()
    {
        //Track Checkpoints
        checkpointTriggerSource = SetUpAudioSource (checkpointTriggerClip);
        countdownSource = SetUpAudioSource (countdownClip);

        //Start Race
        startSource = SetUpAudioSource(startClip);
        middleSource = SetUpAudioSource(middleClip);
        finishSource = SetUpAudioSource(finishClip);
    }

    private AudioSource SetUpAudioSource(AudioClip clip)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.loop = false;
        source.playOnAwake = false;
        source.Pause();

        return source;
    }
}
