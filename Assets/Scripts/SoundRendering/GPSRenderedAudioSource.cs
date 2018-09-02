using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSRenderedAudioSource : RenderedAudioSource {

	protected new void Start () {
		this.overrideIcon = SoundIconSpriteManager.instance.data.GPS;
		base.Start();
	}
}
