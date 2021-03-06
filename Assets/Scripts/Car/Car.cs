﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioSource))]
public class Car : MonoBehaviour {

    public AudioClip crashSound;
    public AudioSource tire_sounds;
    public AudioSource engine;
    public new Rigidbody2D rigidbody;

    // Maybe have integer of how much velocity the car has an increase it by one or decrease by one 
    // when buttons are pressed
    // Then try and adjust velocity to fit this speed

    [SerializeField]
    private float wheelFriction = 10;
	[SerializeField]
    private float engineAcceleration = 1;

    public float time_since_turn = 0f;

    void FixedUpdate() {
        time_since_turn += Time.deltaTime;
        bool brakes = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.JoystickButton0);
        RotateCar(Input.GetAxisRaw("Horizontal"));
        Vector2 targetVelocity = brakes ? Vector2.zero : (Vector2)Vector3.ProjectOnPlane(rigidbody.velocity, transform.up);
        Vector2 delta = targetVelocity - rigidbody.velocity;
        float skid = delta.magnitude - Time.deltaTime * wheelFriction;
        if (skid > 0)
            delta = Vector2.ClampMagnitude(delta, Time.deltaTime * wheelFriction);
        rigidbody.AddForce(delta * rigidbody.mass, ForceMode2D.Impulse);
        if (!brakes)
            rigidbody.AddForce(transform.right * rigidbody.mass * Input.GetAxisRaw("Vertical") * Time.deltaTime * engineAcceleration, ForceMode2D.Impulse);

        if (tire_sounds != null)
            tire_sounds.volume = rigidbody.velocity.magnitude;
        if (engine != null)
            engine.volume = Mathf.Abs(Input.GetAxis("Vertical"));

        //todo: change engine noise based on Input.GetAxisRaw("Vertical")
        //todo: play steering wheel noise based on Input.GetAxisRaw("Vertical")
        //todo: play skid noise
        //todo: play brake pedal noise
        //todo: change wheel noise pitch based on velocity
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        PassOnCollisionEnter2DToChildren(transform, collision);
        if (crashSound != null) {
            foreach (var contact in collision.contacts)
                AudioSource.PlayClipAtPoint(crashSound, contact.point, contact.relativeVelocity.magnitude);
        }
    }

    private void PassOnCollisionEnter2DToChildren(Transform t, Collision2D collision) {
        for (int i = t.childCount - 1; i >= 0; i--) {
            var child = t.GetChild(i);
            foreach (var voiceLine in child.GetComponents<VoiceLine>())
                voiceLine.OnCollide(collision);
            PassOnCollisionEnter2DToChildren(child, collision);
        }
    }

	void OnTriggerEnter2D(Collider2D other) {
		this.BroadcastMessage("TriggerEnter", other);
	}

    public void RotateCar(float rotation) {
        if (rotation != 0) time_since_turn = 0;
        rigidbody.MoveRotation(rigidbody.rotation - transform.InverseTransformVector(rigidbody.velocity).x * Time.deltaTime * rotation * 60);
    }

    /*void OnTriggerStay2D(Collider2D other) {
        // See if that's a road surface, and if it is, play sounds
        Surface s = other.gameObject.GetComponent<Surface>();
        if(s != null && tire_sounds.clip != s.sound)
        {
            tire_sounds.clip = s.sound;
            tire_sounds.Play();
        }
    }*/
}
