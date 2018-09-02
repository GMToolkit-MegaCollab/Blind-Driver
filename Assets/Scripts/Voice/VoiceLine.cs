using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLine : MonoBehaviour {

    public int priority;
    [HideInInspector]
    public PassengerController2 passenger;
    private float cooldown = 0f;
	public AudioClip[] audioClips;

    private void Awake() {
        if (audioClips.Length == 0)
            Debug.LogWarning("No audio clips");
        passenger = GetPassenger(transform.parent);
        if (passenger == null)
            Debug.LogWarning("No associated passenger");
    }

    PassengerController2 GetPassenger(Transform t) {
        if (t == null)
            return null;
        return t.GetComponent<PassengerController2>() ?? GetPassenger(t.parent);
    }

    private void Update() {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
            Tick();
    }

    protected virtual void Tick() { }
    public virtual void OnCollide(Collision2D collision) { }

    public bool Trigger(float cooldown = 0) {

        if (this.cooldown > 0)
            return false;
		Debug.Log(gameObject.name);
        if (audioClips.Length == 0) {
            Debug.LogWarning("No audio clips");
            return false;
        }

        if (passenger == null) {
            Debug.LogWarning("No associated passenger");
            return false;
        }

		//Play random clip
        AudioClip audioClip = audioClips[Random.Range(0, audioClips.Length)];
		if (audioClip == null) return false;
        passenger.Play(audioClip);
        passenger.lastVoiceLineStarted = this;
        this.cooldown = cooldown + audioClip.length;
        return true;
    }
}
