using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Is driver
[RequireComponent(typeof(AudioListener))]
public class CarDriver : MonoBehaviour {

	public enum RotationAxes {
		MouseXAndY = 0, 
		MouseX = 1, 
		MouseY = 2
	}

	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 3f;
	public float sensitivityY = 3f;

	public float minX = -360f;
	public float maxX = 360f;

	public float minY = -60f;
	public float maxY = 60f;

	float rotX = 0f;
	float rotY = 0f;

	private List<float> rotArrX = new List<float>();
	private List<float> rotArrY = new List<float>();
	float rotAvgX = 0f;	
	float rotAvgY = 0f;

	public float frameCounter = 10;

	Quaternion originalRotation;

	void Awake() {
		SetupRendering();
	}

	private void SetupRendering() {
		GameObject go = new GameObject("SoundCamera", new System.Type[] {typeof(SoundCamera)});
		originalRotation = transform.localRotation;
	}

	void Update() {
		if (Input.GetKey(KeyCode.Mouse0)) {
			Cursor.lockState = CursorLockMode.Locked;
		} else {
			Cursor.lockState = CursorLockMode.None;
			return;
		}

		rotAvgY = 0f;
		rotAvgX = 0f;
		
		rotY += Input.GetAxis("Mouse Y") * sensitivityY;
		rotX += Input.GetAxis("Mouse X") * sensitivityX;

		rotArrY.Add(rotY);
		rotArrX.Add(rotX);
		if (rotArrY.Count >= frameCounter) {
			rotArrY.RemoveAt(0);
		}
		if (rotArrX.Count >= frameCounter) {
			rotArrX.RemoveAt(0);
		}

		for (int j = 0; j < rotArrY.Count; j++) {
			rotAvgY += rotArrY[j];
		}
		for (int i = 0; i < rotArrX.Count; i++) {
			rotAvgX += rotArrX[i];
		}
		
		rotAvgY /= rotArrY.Count;
		rotAvgX /= rotArrX.Count;
		rotAvgY = -ClampAngle(rotAvgY, minY, maxY);
		rotAvgX = -ClampAngle(rotAvgX, minX, maxX);

		Quaternion yQuaternion = Quaternion.AngleAxis(rotAvgY, Vector3.left);
		Quaternion xQuaternion = Quaternion.AngleAxis(rotAvgX, Vector3.up);

		transform.localRotation = originalRotation * xQuaternion * yQuaternion;
	}

	public static float ClampAngle(float angle, float min, float max) {
		angle = angle % 360;
		if ((angle >= -360f) &&(angle <= 360f)) {
			if (angle < -360f) {
				angle += 360f;
			}
			if (angle > 360f) {
				angle -= 360f;
			}			
		}
		return Mathf.Clamp(angle, min, max);
	}
}
