using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Passenger", menuName = "Passenger", order = 1)]
public class PassengerData : ScriptableObject {
	[System.Serializable]
	public class Directions {
		public AudioClip[] left, right, front;
	}

	[System.Serializable]
	public class TriggerClip {
		public AudioClip clip;
		public bool reapeatable = false;
	}

	[System.Serializable]
	public class DistanceClip {
		public float distance;
		public AudioClip clip;
		public bool reapeatable = false;
	}

    public string Name;

	public AudioClip[] StartSequence;
	public TriggerClip[] TriggerResponses;
	public DistanceClip[] DistanceResponses;
}
