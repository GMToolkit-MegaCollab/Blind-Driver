using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Handles the core of the scene
[RequireComponent(typeof(Camera))]
public class SoundCamera : MonoBehaviour {

	public static SoundCamera soundCamera;
	public new Camera camera;
	public GameObject origin;
	public Canvas canvas;

	//Singleton
	void Awake() {
		if (soundCamera == null) {
			soundCamera = this;
		} else if (soundCamera != this) {
			SceneManager.UnloadSceneAsync(this.gameObject.scene);
			throw new System.Exception("Error: Duplicate Sound rendering scene.");
		}
		//SceneManager.SetActiveScene(this.gameObject.scene);
		this.camera = this.GetComponent<Camera>();
		origin = new GameObject("origin");
		GameObject c = new GameObject("Canvas", new System.Type[] {typeof(Canvas)});
		canvas = c.GetComponent<Canvas>();
		c.transform.SetParent(this.transform);
		//SceneManager.SetActiveScene(FindObjectOfType<Driver>().gameObject.scene);
		canvas.renderMode = RenderMode.ScreenSpaceCamera;
		canvas.worldCamera = camera;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
