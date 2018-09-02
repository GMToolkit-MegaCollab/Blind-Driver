using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			Destroy(this.transform.gameObject);
			throw new System.Exception("Error: Duplicate Sound rendering scene.");
		}
		this.camera = this.GetComponent<Camera>();
		this.camera.cullingMask = 1 << 5;
		this.camera.backgroundColor = Color.black;
		GameObject root = new GameObject("Root");
		this.transform.SetParent(root.transform);
		this.transform.parent.position = Vector3.zero;
		this.transform.position = Vector3.zero;
		origin = new GameObject("origin");
		origin.transform.SetParent(this.transform.parent);
		origin.transform.position = Vector3.zero;
		GameObject c = new GameObject("Canvas", new System.Type[] {typeof(Canvas)});
		c.transform.SetParent(this.transform);
		canvas = c.GetComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceCamera;
		canvas.worldCamera = camera;
		canvas.gameObject.layer = 5;
		c.transform.localPosition = new Vector3(0f,0f,100f);
	}

	void OnDestroy() {
		if (soundCamera == this) soundCamera = null;
	}
}
