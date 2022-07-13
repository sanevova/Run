using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Target {
    public float relativeSpeed = 5;

    public override void DidSpawn() {
        ApplyRelativeVelocity();
    }

    public override void DidUpdateGameSpeed(float newSpeed) {
        base.DidUpdateGameSpeed(newSpeed);
        ApplyRelativeVelocity();
    }

    void ApplyRelativeVelocity() {
        GetComponent<Rigidbody2D>().velocity += Vector2.left * relativeSpeed;
    }
}
