using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundIconSpriteManager : MonoBehaviour {

	public static SoundIconSpriteManager instance;
	public Sprite Arrow;
	public Sprite Brackets;

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(this);
		}
	}
}
