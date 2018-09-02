using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger : PassengerController2 {
	
	[SerializeField]
	private PassengerData data;

	private AudioClip start;
	private AudioClip end;
	private bool[] usedTriggers;
	private bool[] usedDistances;
	private GPS gps;

	// Use this for initialization
	void Awake() {
		audio.AddLast(this.GetComponent<AudioSource>());
		Debug.Log(data);
		start = Combine(data.StartSequence);
		end = Combine(data.EndSequence);
		Whisker f = this.transform.GetChild(0).GetComponent<Whisker>();
		Whisker r = this.transform.GetChild(0).GetComponent<Whisker>();
		Whisker l = this.transform.GetChild(0).GetComponent<Whisker>();
		f.audioClips = data.Wall.front;
		r.audioClips = data.Wall.right;
		l.audioClips = data.Wall.left;
		Play(start);
		usedTriggers = new bool[data.TriggerResponses.Length];
		usedTriggers = new bool[data.DistanceResponses.Length];
	}

	void Start() {
		
	}
	
	new void Update () {
		base.Update();
	}
	
	public void TriggerEnter(Collider2D other) {
		if (other.GetComponent<Trigger>() != null) {
			int id = other.GetComponent<Trigger>().id;
			PassengerData.TriggerClip triggered = this.data.TriggerResponses[id];
			if (triggered.reapeatable || !usedTriggers[id]) {
				usedTriggers[id] = true;
				Play(triggered.clip);
			}
		}
	}
}
