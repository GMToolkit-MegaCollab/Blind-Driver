using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Is driver
public class Driver : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		SetupRendering();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void SetupRendering() {
		SceneManager.LoadScene("Rendering", LoadSceneMode.Additive);
		SceneManager.SetActiveScene(this.gameObject.scene);
	}
}
