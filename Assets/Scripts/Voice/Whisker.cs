using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whisker : VoiceLine {

    public float y;

	protected override void Tick () {
		if (passenger == null) return; 
        var car = passenger.transform.parent;
        var v = car.GetComponent<Rigidbody2D>().velocity;
        Vector2 o = v.normalized + (Vector2)car.up * y;
        var k = v.magnitude + 0.675f;
        Debug.DrawRay(car.transform.position, o.normalized * k);
        foreach(var hit in Physics2D.RaycastAll(car.transform.position, o, k)) {
            if (!hit.collider.isTrigger) {
                Trigger(2);
                break;
            }
        }
	}
}
