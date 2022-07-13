using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoController : MonoBehaviour {
    public GameController game;
    public Rigidbody2D body;
    public Animator animator;
    public GameObject grid;
    public float jumpSpeed;
    private bool didJump = false;
    private bool didIncreaseJumpGravity = false;
    private float defaultGravity;
    public float stickyJumpGravity = 1.2f;
    private bool isGrounded = false;

    void Start() {
        defaultGravity = body.gravityScale;
    }

    void Update() {
        HandleInputs();
        if (didJump) {
            ApplyStickyJumpGravity();
        }
    }

    void HandleInputs() {
        if (Input.GetAxis("Horizontal") > 0) {
            Debug.Log(body.velocity);
        }
        if (IsInputJump() && CanJump()) {
            Jump();
        }
    }

    bool CanJump() {
        return isGrounded && !didJump;
    }

    bool IsInputJump() {
        return Input.GetAxis("Vertical") > Mathf.Epsilon ||
        Input.GetButtonDown("Jump");
    }

    void Jump() {
        body.velocity += Vector2.up * jumpSpeed;
        didJump = true;
    }

    void ApplyStickyJumpGravity() {
        if (body.velocity.y < 0 && !didIncreaseJumpGravity) {
            body.gravityScale = stickyJumpGravity;
            didIncreaseJumpGravity = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            isGrounded = true;
            animator.SetBool("IsGrounded", true);
            didJump = false;
            // reset sticky jump gravity
            body.gravityScale = defaultGravity;
            didIncreaseJumpGravity = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Target") {
            Debug.Log("HIT TREE");
            game.deathText.enabled = true;
            Time.timeScale = 0;
            GetComponent<SpriteRenderer>().color = Color.red;
            other.GetComponent<SpriteRenderer>().color = Color.grey;
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            isGrounded = false;
            animator.SetBool("IsGrounded", false);
        }
    }
}
