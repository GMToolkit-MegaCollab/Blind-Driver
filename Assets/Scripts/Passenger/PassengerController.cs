using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerController : MonoBehaviour {

    [HideInInspector]
    public VoiceLine lastVoiceLineStarted;
    public AudioSource audioSource;

    public GPS gps;

    public Passenger passenger;

    public Emotion emotion;
    public enum Emotion {
        Neutral,
        Pissed
    }

    public float turn_around = -0.5f;
    public float turn = 0.5f;

    void Update()
    {
        if (gps != null && gps.Path != null)
        {
            GpsStatus(1, 0, transform.position);
        }
    }

    // Calculates the gps status of an offset
    public void GpsStatus(int path_offset, float dist, Vector3 from)
    {
        if(gps.Path.Length > path_offset)
        {
            // The point that you're going towards
            Vector3 next = gps.Path[path_offset];

            // See if you have to turn in order to get on track
            Vector2 wanted_angle = Quaternion.Inverse(transform.rotation) * (next - from).normalized;

            if (wanted_angle.x > turn)
            {
                // You're driving the right way.
                // Find the next turn that the player has to do
                if (gps.Path.Length > path_offset + 1)
                {
                    GpsStatus(path_offset + 1, Vector3.Distance(transform.position, gps.Path[path_offset]),
                        gps.Path[path_offset]);
                }
                else
                {
                    Debug.Log("This is ok " + path_offset);
                }
            }
            else if (wanted_angle.x < turn_around)
            {
                // You're driving the wrong way
                Debug.Log("TURN AROUND!!! in " + dist + " " + path_offset);
            }
            else
            {
                // You should turn
                if (wanted_angle.y > 0)
                {
                    Debug.Log("Turn left in " + dist + " " + path_offset);
                }
                else
                {
                    Debug.Log("Turn right in " + dist + " " + path_offset);
                }
            }
        }
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
