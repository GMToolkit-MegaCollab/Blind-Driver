using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Bob : MonoBehaviour {
    RectTransform trans;

    public float Size = 30f;
    public float Speed = 5f;
    Vector3 beginning;

	// Use this for initialization
	void Start () {
        trans = GetComponent<RectTransform>();
        beginning = trans.position;
	}
	
	// Update is called once per frame
	void Update () {
        trans.position = beginning + Vector3.up * Mathf.Sin(Time.time * Speed) * Size;
	}
}
