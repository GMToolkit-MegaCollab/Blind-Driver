using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Trigger : MonoBehaviour {

	public int id = 0;

	// Use this for initialization
	void Awake() {
		this.GetComponent<BoxCollider2D>().isTrigger = true;
	}
}
