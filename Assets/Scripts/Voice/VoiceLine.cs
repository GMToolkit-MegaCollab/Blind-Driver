using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLine : MonoBehaviour {

    public int priority;
    private PassengerController passenger;
    private float cooldown = 0;
    private Dictionary<PassengerController.Emotion, AudioClip> audioClipDictionary;

    [System.Serializable]
    public struct EmotionalAudioClip {
        public PassengerController.Emotion emotion;
        public AudioClip audioClip;
    }
    public EmotionalAudioClip[] audioClips = { new EmotionalAudioClip { } };

    private void Start() {
        audioClipDictionary = new Dictionary<PassengerController.Emotion, AudioClip>();
        foreach (EmotionalAudioClip e in audioClips) {
            if (audioClipDictionary.ContainsKey(e.emotion)) {
                Debug.LogWarning("Duplicate emotion ignored");
                continue;
            }
            if (e.audioClip == null) {
                Debug.LogWarning("Missing audio clip ignored");
                continue;
            }
            audioClipDictionary.Add(e.emotion, e.audioClip);
        }
        if (audioClipDictionary.Count == 0)
            Debug.LogWarning("No audio clips");
        passenger = GetPassenger(transform.parent);
        if (passenger == null)
            Debug.LogWarning("No associated passenger");
        audioClips = null;
    }

    PassengerController GetPassenger(Transform t) {
        if (t == null)
            return null;
        return t.GetComponent<PassengerController>() ?? GetPassenger(t.parent);
    }

    private void Update() {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
            Tick();
    }

    protected virtual void Tick() { }
    public virtual void OnCollide(Collision2D collision) { }

    protected bool Trigger(float cooldown = 0) {

        if (audioClipDictionary.Count == 0) {
            Debug.LogWarning("No audio clips");
            return false;
        }

        if (passenger == null) {
            Debug.LogWarning("No associated passenger");
            return false;
        }

        if (passenger.audioSource.isPlaying) {
            if (passenger.lastVoiceLineStarted != null && passenger.lastVoiceLineStarted.priority >= priority)
                return false;
        }

        AudioClip audioClip;
        if (audioClipDictionary.TryGetValue(passenger.emotion, out audioClip)) {

        } else if (audioClipDictionary.TryGetValue(PassengerController.Emotion.Neutral, out audioClip)) {

        } else {
            audioClip = new List<AudioClip>(audioClipDictionary.Values)[0];
        }

        passenger.audioSource.clip = audioClip;
        passenger.lastVoiceLineStarted = this;
        this.cooldown = cooldown + audioClip.length;
        return true;
    }
}
