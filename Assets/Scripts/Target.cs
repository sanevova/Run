using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {
    const float OBSTACLE_CLEAR_X = -15f;

    void Update() {
        if (transform.position.x < OBSTACLE_CLEAR_X) {
            Destroy(gameObject);
        }
    }

    public virtual void DidSpawn() {
    }

    public virtual void DidUpdateGameSpeed(float newSpeed) {
        GetComponent<Rigidbody2D>().velocity = Vector2.left * newSpeed;
    }
}
