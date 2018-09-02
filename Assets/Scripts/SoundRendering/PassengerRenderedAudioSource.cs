using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerRenderedAudioSource : RenderedAudioSource {

	protected new void Start () {
		this.overrideIcon = SoundIconSpriteManager.instance.data.Passenger;
		base.Start();
	}
}
