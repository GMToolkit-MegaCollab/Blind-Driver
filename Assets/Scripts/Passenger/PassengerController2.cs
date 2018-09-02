using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerController2 : MonoBehaviour {
    public AudioSource audioSource;
    public VoiceLine lastVoiceLineStarted;
	
	[SerializeField]
	private PassengerData data;

    public Emotion emotion;
    public enum Emotion {
        Neutral = 0
    }

    void Start () {
		
	}

	void Update () {
		
	}
}
