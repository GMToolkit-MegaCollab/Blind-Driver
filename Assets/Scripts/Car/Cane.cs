using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cane : MonoBehaviour {

	void Update () {
        transform.localRotation = Quaternion.AngleAxis(45 + 15 * Mathf.Sin(Mathf.PI * 2 * Time.time), Vector3.forward);
	}
}
