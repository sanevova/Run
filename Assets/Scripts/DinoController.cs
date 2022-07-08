using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoController : MonoBehaviour {
    public Rigidbody2D body;
    public Animator animator;
    public GameObject grid;
    public float jumpSpeed;
    private bool isGrounded = false;
    public float speedX = 1;

    void Start() {
        grid.GetComponent<Rigidbody2D>().velocity = Vector2.left * speedX;
        body.velocity = new Vector2(speedX, body.velocity.y);
    }

    void Update() {
        body.velocity = new Vector2(speedX, body.velocity.y);
        if (IsInputJump() && isGrounded) {
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        }
    }

    bool IsInputJump() {
        return Input.GetAxis("Vertical") > Mathf.Epsilon ||
        Input.GetButtonDown("Jump");
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            isGrounded = true;
            animator.SetBool("IsGrounded", true);
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            isGrounded = false;
            animator.SetBool("IsGrounded", false);
        }
    }
}
