using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {
    const float OBSTACLE_CLEAR_X = -15f;

    void Start() {
    }

    void Update() {
        if (transform.position.x < OBSTACLE_CLEAR_X) {
            Destroy(gameObject);
        }
    }
}
