using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class DinoController : MonoBehaviour {
    const float TREE_SPAWN_INTERVAL_MIN = 0.8F;
    const float TREE_SPAWN_INTERVAL_MAX = 1.6F;
    const int SPEED_INCREASE_INTERVAL_GROWTH = 50;

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
    public float speedX = 10;
    public GameObject tree;
    public TMP_Text deathText;
    public TMP_Text scoreText;
    private float score = 0;
    private int nextSpeedIncreaseScore = SPEED_INCREASE_INTERVAL_GROWTH;
    private int speedIncreaseInterval = SPEED_INCREASE_INTERVAL_GROWTH;
    public float speedIncreaseFactor = 1.2f;
    private float lastTreeSpawnTime = 0;
    private float nextTreeSpawnTime = 0;

    void Start() {
        tree.SetActive(false);
        defaultGravity = body.gravityScale;
    }

    void Update() {
        MoveGround();
        SpawnTrees();
        HandleInputs();
        if (didJump) {
            ApplyStickyJumpGravity();
        }
        UpdateScore();
    }

    void HandleInputs() {
        if (Input.GetButtonDown("Fire2")) {
            // pause/unpause
            Time.timeScale = 1 - Time.timeScale;
            deathText.enabled = !deathText.enabled;
            GetComponent<SpriteRenderer>().color = Color.white;
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
        float x = tilemap.tileAnchor.x;
        if (x < 0) {
            tilemap.tileAnchor += Vector3.right * Mathf.Floor(Mathf.Abs(x) + 1);
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

    void SpawnTrees() {
        if (nextTreeSpawnTime > Time.time) {
            return;
        }
        SpawnTree();
        lastTreeSpawnTime = Time.time;
        float interval = Random.Range(TREE_SPAWN_INTERVAL_MIN, TREE_SPAWN_INTERVAL_MAX);
        nextTreeSpawnTime = lastTreeSpawnTime + interval;
    }

    void SpawnTree() {
        GameObject newTree = Instantiate(tree, tree.transform.position + Vector3.left, tree.transform.rotation);
        newTree.SetActive(true);
        newTree.GetComponent<Rigidbody2D>().velocity = Vector2.left * speedX;
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

    void UpdateScore() {
        score += Time.deltaTime * speedX;
        int displayScore = (int)Mathf.Floor(score);
        if (displayScore > nextSpeedIncreaseScore) {
            speedX *= speedIncreaseFactor;
            foreach (GameObject tree in GameObject.FindGameObjectsWithTag("Target")) {
                tree.GetComponent<Rigidbody2D>().velocity = Vector2.left * speedX;
            }
            nextSpeedIncreaseScore += speedIncreaseInterval;
            speedIncreaseInterval += SPEED_INCREASE_INTERVAL_GROWTH;
        }
        scoreText.text = string.Format("score: {0}\nspeed: {1:0.0}", displayScore.ToString(), speedX);
    }
}
