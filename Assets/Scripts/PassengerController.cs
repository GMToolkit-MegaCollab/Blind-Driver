using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerController : MonoBehaviour {

    [HideInInspector]
    public VoiceLine lastVoiceLineStarted;
    public AudioSource audioSource;

    public Passenger passenger;

    public Emotion emotion;
    public enum Emotion {
        Neutral,
        Pissed
    }
    
    public void BumperCall(Bumpers.Direction direction, Bumpers.IntensityLevel intensity)
    {
        // These responses are mostly panic based. If the intensity is moderate, then the emotion takes part,
        // but any closer and it's pure panic
        Passenger.Directions sound_pool = null;
        switch (intensity)
        {
            case Bumpers.IntensityLevel.Moderate:
                break;
            case Bumpers.IntensityLevel.Intense:
                sound_pool = passenger.intense;
                break;
            case Bumpers.IntensityLevel.Extreme:

                break;
        }

        if (sound_pool == null) return;

        switch (direction) {
            case Bumpers.Direction.Front:
                PlaySound(sound_pool.front);
                break;
            case Bumpers.Direction.Right:
                PlaySound(sound_pool.right);
                break;
            case Bumpers.Direction.Left:
                PlaySound(sound_pool.left);
                break;
        }
    }

    public void PlaySound(AudioClip[] clips)
    {
        if(!audioSource.isPlaying)
            audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
}
