using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DinoController : MonoBehaviour {
    public Rigidbody2D body;
    public Animator animator;
    public GameObject grid;
    public GameObject groundTilemap;
    public float jumpSpeed;
    private bool didJump = false;
    private bool isGrounded = false;
    public float speedX = 1;

    void Start() {
    }

    void Update() {
        MoveGround();
        HandleInputs();
    }

    void HandleInputs() {
        if (Input.GetButtonDown("Fire1")) {
            speedX = speedX % 5 + 1;
        }
        if (Input.GetAxis("Horizontal") > 0) {
            Debug.Log(body.velocity);
        }
        if (CanJump()) {
            body.velocity += Vector2.up * jumpSpeed;
            didJump = true;
        }
    }

    void MoveGround() {
        Tilemap tilemap = groundTilemap.GetComponent<Tilemap>();
        tilemap.tileAnchor += Vector3.left * speedX * Time.deltaTime;
        if (tilemap.tileAnchor.x < 0) {
            tilemap.tileAnchor += Vector3.right;
        }
    }

    bool CanJump() {
        return IsInputJump() && isGrounded && !didJump;
    }
    bool IsInputJump() {
        return Input.GetAxis("Vertical") > Mathf.Epsilon ||
        Input.GetButtonDown("Jump");
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            isGrounded = true;
            animator.SetBool("IsGrounded", true);
            didJump = false;
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            isGrounded = false;
            animator.SetBool("IsGrounded", false);
        }
    }
}
