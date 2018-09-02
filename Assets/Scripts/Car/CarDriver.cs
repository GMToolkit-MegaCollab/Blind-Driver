using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Is driver
[RequireComponent(typeof(AudioListener))]
public class CarDriver : MonoBehaviour {

	public CarDriver instance;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(this);
		}
		SetupRendering();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void SetupRendering() {
		GameObject go = new GameObject("SoundCamera", new System.Type[] {typeof(SoundCamera), typeof(Camera)});
	}
}
