using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class DinoController : MonoBehaviour {
    public Rigidbody2D body;
    public Animator animator;
    public GameObject grid;
    public GameObject groundTilemap;
    public float jumpSpeed;
    private bool didJump = false;
    private bool didIncreaseJumpGravity = false;
    private float defaultGravity;
    public float stickyJumpGravity = 1.2f;
    private bool isGrounded = false;
    public float speedX = 1;
    public GameObject tree;
    public TMP_Text deathText;
    public TMP_Text scoreText;
    private int score = 0;

    void Start() {
        tree.SetActive(false);
        defaultGravity = body.gravityScale;
    }

    void Update() {
        MoveGround();
        HandleInputs();
        if (didJump) {
            ApplyStickyJumpGravity();
        }
        UpdateScore();
    }

    void HandleInputs() {
        if (Input.GetButtonDown("Fire1")) {
            GameObject newTree = Instantiate(tree, tree.transform.position + Vector3.left, tree.transform.rotation);
            newTree.SetActive(true);
            newTree.GetComponent<Rigidbody2D>().velocity = Vector2.left * speedX;
        }
        if (Input.GetButtonDown("Fire2")) {
            // pause/unpause
            Time.timeScale = 1 - Time.timeScale;
            deathText.enabled = !deathText.enabled;
        }
        if (Input.GetAxis("Horizontal") > 0) {
            Debug.Log(body.velocity);
        }
        if (IsInputJump() && CanJump()) {
            Jump();
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
            deathText.enabled = true;
            Time.timeScale = 0;
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Ground") {
            isGrounded = false;
            animator.SetBool("IsGrounded", false);
        }
    }

    void UpdateScore() {
        score = (int)Mathf.Floor(Time.time * speedX);
        scoreText.text = score.ToString();
    }
}
