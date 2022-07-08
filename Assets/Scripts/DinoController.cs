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
    private bool isGrounded = false;
    public float speedX = 1;

    void Start() {
        // foreach (Transform gridItem in grid.transform) {
        //     gridItem.GetComponent<Rigidbody2D>().velocity = Vector2.left * speedX;
        // }
        // grid.GetComponent<Rigidbody2D>().velocity = Vector2.left * speedX;
        // body.velocity = new Vector2(0, body.velocity.y);
        // Debug.Log(grid.GetComponent<Rigidbody2D>().velocity);
    }

    void Update() {
        body.velocity = new Vector2(0, body.velocity.y);
        Tilemap tilemap = groundTilemap.GetComponent<Tilemap>();
        if (Input.GetAxis("Horizontal") > 0) {
            Debug.Log(body.velocity);
        }
        if (IsInputJump() && isGrounded) {
            body.velocity = Vector2.right;
            // body.velocity = new Vector2(body.velocity.x, jumpSpeed);
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
