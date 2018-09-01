using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bumpers : MonoBehaviour
{
    public enum Direction { Left, Right, Front }
    public enum IntensityLevel { Moderate, Intense, Extreme }
    public IntensityLevel intensity = IntensityLevel.Intense;

    public float LookAhead = 10f;
    public float CubeMagnifier = 3f;
    public float DeltaAngle = 10f;
    public float BoxSize = 0.5f;
    public PassengerController passenger;

    Rigidbody2D rb2d;
    
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Get the car velocity in local space
        Vector2 localVel = transform.InverseTransformDirection(rb2d.velocity);
        if (localVel.x > 0f)
        {
            // Do the box casts
            bool front_hit = Physics2D.Raycast(
                rb2d.position,
                Quaternion.Euler(0, 0, rb2d.rotation) * Vector2.right,
                LookAhead * localVel.x, 1 << 8).transform != null;
            bool right_hit = Physics2D.Raycast(
                rb2d.position,
                Quaternion.Euler(0, 0, rb2d.rotation - DeltaAngle) * Vector2.right,
                LookAhead * localVel.x, 1 << 8).transform != null;
            bool left_hit = Physics2D.Raycast(
                rb2d.position,
                Quaternion.Euler(0, 0, rb2d.rotation + DeltaAngle) * Vector2.right,
                LookAhead * localVel.x, 1 << 8).transform != null;

            // If it hits, calculate how the passenger suggests you avoid it
            if (front_hit || right_hit || left_hit)
            {
                // See if the left or the right side or the front was hit
                if (right_hit == left_hit && front_hit)
                {
                    // Frontal hit
                    passenger.BumperCall(Direction.Front, intensity);
                }
                else if (!left_hit && right_hit && front_hit)
                {
                    // Right hit(Even if the front is hit, we should still call out a wall to the right)
                    passenger.BumperCall(Direction.Right, intensity);
                }
                else if (!right_hit && left_hit && front_hit)
                {
                    // Left hit!
                    passenger.BumperCall(Direction.Left, intensity);
                }
            }
        }
	}

    void OnDrawGizmos()
    {
        if (rb2d == null) return;

        Vector2 localVel = transform.InverseTransformDirection(rb2d.velocity);
        if (localVel.x > 0)
        {
            Gizmos.DrawLine(rb2d.position,
                rb2d.position +
                (Vector2)(Quaternion.Euler(0, 0, rb2d.rotation) *
                Vector2.right * LookAhead * localVel.x));
            Gizmos.DrawLine(rb2d.position,
                rb2d.position +
                (Vector2)(Quaternion.Euler(0, 0, rb2d.rotation - DeltaAngle) *
                Vector2.right * LookAhead * localVel.x));
            Gizmos.DrawLine(rb2d.position,
                rb2d.position +
                (Vector2)(Quaternion.Euler(0, 0, rb2d.rotation + DeltaAngle) *
                Vector2.right * LookAhead * localVel.x));
        }
    }
}
